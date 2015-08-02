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
    /// Controler business.
    /// </summary>
    internal static class ControlerBusiness
    {

        #region Private data

        /// <summary>
        /// Wait event from node.
        /// </summary>
        private static readonly EventWaitHandle WaitEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        #endregion

        #region Internal methods

        /// <summary>
        /// Close controler.
        /// </summary>
        internal static void Close()
        {
            MessageQueueBusiness.Close();
        }

        /// <summary>
        /// Detect ports corresponding to a controler.
        /// </summary>
        /// <returns>Port list.s</returns>
        internal static List<ControlerDto> List()
        {
            // get existing ports
            var ports = MessageQueueBusiness.ListPorts();

            // try to connect
            var result = new List<ControlerDto>();
            foreach (var p in ports)
            {
                var initPort = MessageQueueBusiness.Connect(p);
                if (initPort.IsOpen)
                {
                    var controler = new ControlerDto
                    {
                        DeviceClass = DeviceClass.StaticControler,
                        DeviceClassGeneric = DeviceClassGeneric.StaticControler,
                        Port = p
                    };

                    // get home id to valid controler
                    controler.IsReady = GetHomeId(controler);
                    if (controler.IsReady) result.Add(controler);
                }

                MessageQueueBusiness.Close();
            }

            return result;
        }

        /// <summary>
        /// Initialize the controler on specified port.
        /// </summary>
        /// <param name="controler">Port to initialize.</param>
        /// <returns>Controler of the port.</returns>
        internal static ControlerDto Connect(ControlerDto controler)
        {
            // open port
            controler.Port = MessageQueueBusiness.Connect(controler.Port);

            if (controler.Port.IsOpen)
            {
                controler.IsReady = GetZVersion(controler);
                if (controler.IsReady) controler.IsReady = GetHomeId(controler);
                if (controler.IsReady) controler.IsReady = GetControlerNodes(controler);
                if (controler.IsReady)
                {
                    foreach (var x in controler.Nodes)
                    {
                        controler.IsReady = GetNodeProtocol(controler, x);
                    }
                }
            }
            else
            {
                controler.IsReady = false;
            }

            return controler;
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
                var controler = (ControlerDto) receivedMessage.Node;

                switch (receivedMessage.Command)
                {
                    case MessageCommand.GetApiCapabilities:
                        GetApiCapabilitiesResponse(controler, receivedMessage);
                        break;
                    case MessageCommand.GetHomeId:
                        GetHomeIdResponse(controler, receivedMessage);
                        break;
                    case MessageCommand.GetVersion:
                        GetZVersionResponse(controler, receivedMessage);
                        break;
                    case MessageCommand.GetControlerNodes:
                        GetControlerNodesResponse(controler, receivedMessage);
                        break;
                    case MessageCommand.GetNodeProtocol:
                        GetNodeProtocolResponse(controler, receivedMessage);
                        break;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get the controler home identifier.
        /// </summary>
        /// <param name="controler">Controler to get.</param>
        /// <returns>True if home id is got, else false.</returns>
        private static bool GetHomeId(ControlerDto controler)
        {
            // send message
            MessageQueueBusiness.Send(controler, CreateCommandMessage(MessageCommand.GetHomeId, controler));

            // wait for response
            WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);

            // get result
            return string.IsNullOrEmpty(controler.HomeIdentifier) == false;
        }

        /// <summary>
        /// Execute command "Get home id" and node identifier.
        /// </summary>
        /// <param name="controler">Concerned controler.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetHomeIdResponse(ControlerDto controler, MessageDto receivedMessage)
        {
            // complete controler
            if (receivedMessage.Content.Count >= 5)
            {
                controler.HomeIdentifier = BitConverter.ToString(receivedMessage.Content.Take(4).ToArray());
                controler.ZIdentifier = receivedMessage.Content.Last();
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get Z version.
        /// </summary>
        /// <param name="controler">Concerned controler.</param>
        /// <returns>True if identifier is got.</returns>
        private static bool GetZVersion(ControlerDto controler)
        {
            // send message
            MessageQueueBusiness.Send(controler, CreateCommandMessage(MessageCommand.GetVersion, controler));

            // wait for response
            WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);

            // get result
            return !string.IsNullOrEmpty(controler.ZVersion);
        }

        /// <summary>
        /// Execute command "Get version".
        /// </summary>
        /// <param name="controler">Concerned controler.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetZVersionResponse(ControlerDto controler, MessageDto receivedMessage)
        {
            // complete controler
            if (receivedMessage.Content.Count >= 3)
            {
                var end = receivedMessage.Content.TakeWhile(b => b != '\0').Count();
                controler.ZVersion = Encoding.ASCII.GetString(receivedMessage.Content.Take(end).ToArray());
                controler.DeviceClass = (DeviceClass)receivedMessage.Content[receivedMessage.Content.Count - 1];
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get API capabilities.
        /// </summary>
        /// <param name="controler">Concerned controler.</param>
        /// <returns>True if identifier is got.</returns>
        /// <remarks>Not used because I don't how to use the response.</remarks>
        private static bool GetApiCapabilities(ControlerDto controler)
        {
            // send message
            MessageQueueBusiness.Send(controler, CreateCommandMessage(MessageCommand.GetApiCapabilities, controler));

            // wait for response
            WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);

            // get result
            return !string.IsNullOrEmpty(controler.ManufacturerIdentifier) &&
                   !string.IsNullOrEmpty(controler.ProductType) &&
                   !string.IsNullOrEmpty(controler.ProductIdentifier);
        }

        /// <summary>
        /// Execute command to get COM capabilitis.
        /// </summary>
        /// <param name="controler">Concerned controler.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetApiCapabilitiesResponse(ControlerDto controler, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 6)
            {
                decimal version;
                decimal.TryParse(string.Concat(receivedMessage.Content[0], ",", receivedMessage.Content[1]), out version);
                controler.ApiVersion = version;

                controler.ManufacturerIdentifier = string.Concat(receivedMessage.Content[2].ToString("00"), receivedMessage.Content[3].ToString("00"));
                controler.ProductType = string.Concat(receivedMessage.Content[3].ToString("00"), receivedMessage.Content[4].ToString("00"));
                controler.ProductIdentifier = string.Concat(receivedMessage.Content[5].ToString("00"), receivedMessage.Content[6].ToString("00"));
                controler.ApiCapabilities = receivedMessage.Content.Skip(7).ToList();
            }

            // release event
            WaitEvent.Set();
        }


        /// <summary>
        /// Get nodes known by the controler.
        /// </summary>
        /// <param name="controler">Controler.</param>
        /// <returns>Process result.</returns>
        private static bool GetControlerNodes(ControlerDto controler)
        {
            // send message
            MessageQueueBusiness.Send(controler, CreateCommandMessage(MessageCommand.GetControlerNodes, controler));

            // wait for response
            WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);

            // get result
            var result = controler.Nodes != null;
            if (!result) controler.Nodes = new List<DeviceDto>();

            return result;
        }

        /// <summary>
        /// Execute command to get note list.
        /// </summary>
        /// <param name="controler">Concerned controler.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetControlerNodesResponse(ControlerDto controler, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 29)
            {
                controler.Nodes = new List<DeviceDto>();
                for (var i = 3; i < 32; i++)
                {
                    var byteData = receivedMessage.Content[i];
                    for (var b = 0; b < 8; b++)
                    {
                        if ((byteData & (byte) Math.Pow(2, b)) == (byte) Math.Pow(2, b))
                        {
                            controler.Nodes.Add(new DeviceDto
                            {
                                HomeIdentifier = controler.HomeIdentifier,
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
        /// <param name="controler">Controler.</param>
        /// <param name="node">Z node to complete.</param>
        /// <returns>Process result.</returns>
        private static bool GetNodeProtocol(ControlerDto controler, DeviceDto node)
        {
            // send message
            MessageQueueBusiness.Send(controler, CreateCommandMessage(MessageCommand.GetNodeProtocol, controler, node.ZIdentifier));

            // wait for response
            WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);

            // get result
            node.HomeIdentifier = controler.HomeIdentifier;
            return node.DeviceClass != DeviceClass.Unknown &&
                         node.DeviceClassGeneric != DeviceClassGeneric.Other;
        }

        /// <summary>
        /// Process the response to get a node protocol.
        /// </summary>
        /// <param name="controler">Concerned controler.s</param>
        /// <param name="receivedMessage">Message received.</param>
        private static void GetNodeProtocolResponse(ControlerDto controler, MessageDto receivedMessage)
        {
            if (receivedMessage.Content.Count >= 5)
            {
                var node = controler.Nodes.FirstOrDefault(x => x.ZIdentifier == receivedMessage.ZIdentifier);
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
        /// <param name="controler">Contextual node of the message.</param>
        /// <param name="command">Command to process.</param>
        /// <param name="zId">Node identifier to send in message. 0x00 if no node is concerned.</param>
        /// <returns>Message.</returns>
        private static MessageDto CreateCommandMessage(MessageCommand command, DeviceDto controler, int zId = 0)
        {
            var result = new MessageDto
            {
                Command = command,
                IsValid = true,
                Node = controler,
                ZIdentifier = (byte)zId,
                Type = MessageType.Request,
            };

            return result;
        }

        #endregion

    }
}
