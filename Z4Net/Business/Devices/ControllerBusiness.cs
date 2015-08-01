using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Z4Net.Business.Messaging;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;

namespace Z4Net.Business.Devices
{
    /// <summary>
    /// Controller business.
    /// </summary>
    internal static class ControllerBusiness
    {

        #region Private data

        /// <summary>
        /// Wait event from node.
        /// </summary>
        private static readonly EventWaitHandle WaitEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        #endregion

        #region Internal methods

        /// <summary>
        /// Close controller.
        /// </summary>
        internal static void Close()
        {
            MessageQueueBusiness.Close();
        }

        /// <summary>
        /// Detect ports corresponding to a controller.
        /// </summary>
        /// <returns>Port list.s</returns>
        internal static List<ControllerDto> List()
        {
            // get existing ports
            var ports = MessageQueueBusiness.ListPorts();

            // try to connect
            var result = new List<ControllerDto>();
            foreach (var p in ports)
            {
                var initPort = MessageQueueBusiness.Initialize(p);
                if (initPort.IsOpen)
                {
                    var controller = new ControllerDto
                    {
                        DeviceClass = DeviceClass.StaticController,
                        DeviceClassGeneric = DeviceClassGeneric.StaticController,
                        Port = p
                    };

                    // get home id to valid controller
                    controller.IsReady = GetHomeId(controller);
                    if (controller.IsReady) result.Add(controller);
                }

                MessageQueueBusiness.Close();
            }

            return result;
        }

        /// <summary>
        /// Initialize the controller on specified port.
        /// </summary>
        /// <param name="controller">Port to initialize.</param>
        /// <returns>Controller of the port.</returns>
        internal static ControllerDto Initialize(ControllerDto controller)
        {
            // open port
            controller.Port = MessageQueueBusiness.Initialize(controller.Port);

            if (controller.Port.IsOpen)
            {
                controller.IsReady = GetZVersion(controller);
                if (controller.IsReady) controller.IsReady = GetHomeId(controller);
                if (controller.IsReady) controller.IsReady = GetControllerNodes(controller);
                if (controller.IsReady)
                {
                    foreach (var x in controller.Nodes)
                    {
                        controller.IsReady = GetNodeProtocol(controller, x);
                    }
                }
            }
            else
            {
                controller.IsReady = false;
            }

            return controller;
        }

        /// <summary>
        /// Called when a message is received.
        /// </summary>
        /// <param name="requestMessage">Request message.</param>
        /// <param name="receivedMessage">Recevied message.</param>
        /// <returns>Next state of request message.</returns>
        internal static void ResponseReceived(MessageDto requestMessage, MessageDto receivedMessage)
        {
            if (receivedMessage?.Node != null)
            {
                var controller = (ControllerDto) receivedMessage.Node;

                switch (receivedMessage.Command)
                {
                    case MessageCommand.GetApiCapabilities:
                        GetApiCapabilitiesResponse(controller, receivedMessage);
                        break;
                    case MessageCommand.GetHomeId:
                        GetHomeIdResponse(controller, receivedMessage);
                        break;
                    case MessageCommand.GetVersion:
                        GetZVersionResponse(controller, receivedMessage);
                        break;
                    case MessageCommand.GetControllerNodes:
                        GetControllerNodesResponse(controller, receivedMessage);
                        break;
                    case MessageCommand.GetNodeProtocol:
                        GetNodeProtocolResponse(controller, receivedMessage);
                        break;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the controller home identifier.
        /// </summary>
        /// <param name="controller">Controller to get.</param>
        /// <returns>True if home id is got, else false.</returns>
        private static bool GetHomeId(ControllerDto controller)
        {
            // send message
            MessageQueueBusiness.Send(controller, CreateCommandMessage(MessageCommand.GetHomeId, controller));

            // wait for response
            WaitEvent.WaitOne();

            // get result
            return string.IsNullOrEmpty(controller.HomeIdentifier) == false;
        }

        /// <summary>
        /// Execute command "Get home id" and node identifier.
        /// </summary>
        /// <param name="controller">Concerned controller.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetHomeIdResponse(ControllerDto controller, MessageDto receivedMessage)
        {
            // complete controller
            if (receivedMessage.Content.Count >= 5)
            {
                controller.HomeIdentifier = BitConverter.ToString(receivedMessage.Content.Take(4).ToArray());
                controller.ZIdentifier = receivedMessage.Content.Last();
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get Z version.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <returns>True if identifier is got.</returns>
        private static bool GetZVersion(ControllerDto controller)
        {
            // send message
            MessageQueueBusiness.Send(controller, CreateCommandMessage(MessageCommand.GetVersion, controller));

            // wait for response
            WaitEvent.WaitOne();

            // get result
            return !string.IsNullOrEmpty(controller.ZVersion);
        }

        /// <summary>
        /// Execute command "Get version".
        /// </summary>
        /// <param name="controller">Concerned controller.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetZVersionResponse(ControllerDto controller, MessageDto receivedMessage)
        {
            // complete controller
            if (receivedMessage.Content.Count >= 3)
            {
                var end = receivedMessage.Content.TakeWhile(b => b != '\0').Count();
                controller.ZVersion = Encoding.ASCII.GetString(receivedMessage.Content.Take(end).ToArray());
                controller.DeviceClass = (DeviceClass)receivedMessage.Content[receivedMessage.Content.Count - 1];
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get API capabilities.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <returns>True if identifier is got.</returns>
        /// <remarks>Not used because I don't how to use the response.</remarks>
        private static bool GetApiCapabilities(ControllerDto controller)
        {
            // send message
            MessageQueueBusiness.Send(controller, CreateCommandMessage(MessageCommand.GetApiCapabilities, controller));

            // wait for response
            WaitEvent.WaitOne();

            // get result
            return !string.IsNullOrEmpty(controller.ManufacturerIdentifier) &&
                   !string.IsNullOrEmpty(controller.ProductType) &&
                   !string.IsNullOrEmpty(controller.ProductIdentifier);
        }

        /// <summary>
        /// Execute command to get COM capabilitis.
        /// </summary>
        /// <param name="controller">Concerned controller.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetApiCapabilitiesResponse(ControllerDto controller, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 6)
            {
                decimal version;
                decimal.TryParse(string.Concat(receivedMessage.Content[0], ",", receivedMessage.Content[1]), out version);
                controller.ApiVersion = version;

                controller.ManufacturerIdentifier = string.Concat(receivedMessage.Content[2].ToString("00"), receivedMessage.Content[3].ToString("00"));
                controller.ProductType = string.Concat(receivedMessage.Content[3].ToString("00"), receivedMessage.Content[4].ToString("00"));
                controller.ProductIdentifier = string.Concat(receivedMessage.Content[5].ToString("00"), receivedMessage.Content[6].ToString("00"));
                controller.ApiCapabilities = receivedMessage.Content.Skip(7).ToList();
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get nodes known by the controller.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <returns>Process result.</returns>
        private static bool GetControllerNodes(ControllerDto controller)
        {
            // send message
            MessageQueueBusiness.Send(controller, CreateCommandMessage(MessageCommand.GetControllerNodes, controller));

            // wait for response
            WaitEvent.WaitOne();

            // get result
            var result = controller.Nodes != null;
            if (!result) controller.Nodes = new List<DeviceDto>();

            return result;
        }

        /// <summary>
        /// Execute command to get note list.
        /// </summary>
        /// <param name="controller">Concerned controller.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetControllerNodesResponse(ControllerDto controller, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 29)
            {
                controller.Nodes = new List<DeviceDto>();
                for (var i = 3; i < 32; i++)
                {
                    var byteData = receivedMessage.Content[i];
                    for (var b = 0; b < 8; b++)
                    {
                        if ((byteData & (byte) Math.Pow(2, b)) == (byte) Math.Pow(2, b))
                        {
                            controller.Nodes.Add(new DeviceDto
                            {
                                HomeIdentifier = controller.HomeIdentifier,
                                ZIdentifier = ((i - 3)*8) + b + 1
                            });
                        }
                    }
                }
            }
            else
            {
                receivedMessage.Node = null;
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get node protocol.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="node">Z node to complete.</param>
        /// <returns>Process result.</returns>
        private static bool GetNodeProtocol(ControllerDto controller, DeviceDto node)
        {
            // send message
            MessageQueueBusiness.Send(controller, CreateCommandMessage(MessageCommand.GetNodeProtocol, controller, node.ZIdentifier));

            // wait for response
            WaitEvent.WaitOne();

            // get result
            node.HomeIdentifier = controller.HomeIdentifier;
            return node.DeviceClass != DeviceClass.Unknown &&
                         node.DeviceClassGeneric != DeviceClassGeneric.Other;
        }

        /// <summary>
        /// Process the response to get a node protocol.
        /// </summary>
        /// <param name="controller">Concerned controller.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetNodeProtocolResponse(ControllerDto controller, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 5)
            {
                var node = controller.Nodes.FirstOrDefault(x => x.ZIdentifier == receivedMessage.ZIdentifier);
                if (node != null)
                {
                    node.DeviceClass = (DeviceClass) receivedMessage.Content[3];
                    node.DeviceClassGeneric = (DeviceClassGeneric) receivedMessage.Content[4];
                }
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Create a command message.
        /// </summary>
        /// <param name="controller">Contextual node of the message.</param>
        /// <param name="command">Command to process.</param>
        /// <param name="zId">Node identifier to send in message. 0x00 if no node is concerned.</param>
        /// <returns>Message.</returns>
        private static MessageDto CreateCommandMessage(MessageCommand command, DeviceDto controller, int zId = 0)
        {
            var result = new MessageDto
            {
                Command = command,
                IsValid = true,
                Node = controller,
                ZIdentifier = (byte)zId,
                Type = MessageType.Request,
            };

            return result;
        }

        #endregion

    }
}
