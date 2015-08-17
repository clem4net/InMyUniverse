using System.Collections.Generic;
using System.Linq;
using Technical;
using Z4Net.Dto.Attributes;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;

namespace Z4Net.Business.Devices
{
    /// <summary>
    /// Device business.
    /// </summary>
    internal static class DevicesBusiness
    {

        #region Internal methods

        /// <summary>
        /// Acknowledgment received.
        /// </summary>
        /// <param name="messageProcessing">Message processing.</param>
        internal static void AcknowledgmentReceived(MessageProcessDto messageProcessing)
        {
            IDevice iDevice = null;

            if (messageProcessing.MessageTo != null)
            {
                // configuration command
                if (messageProcessing.MessageTo.IsConfiguration)
                {
                    iDevice = new ConfigurationBusiness();
                }
                // constructor command
                else if (messageProcessing.MessageTo.IsConstructor)
                {
                    iDevice = new ConstructorBusiness();
                }
                // generic response
                else
                {
                    var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(messageProcessing.MessageTo.Node.DeviceClassGeneric);
                    if (processAttr != null) iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                }

                // execute
                iDevice?.AcknowlegmentReceived(messageProcessing.MessageFrom);
            }
        }

        /// <summary>
        /// Close all devices.
        /// </summary>
        /// <param name="controller">Controller to close.</param>
        internal static void Close(ControllerDto controller)
        {
            ControllerBusiness.Close(controller);
        }

        /// <summary>
        /// Configure a device.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="node">Node to configure.</param>
        /// <param name="parameter">Parameter identifier.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Configure result.</returns>
        internal static bool Configure(ControllerDto controller, DeviceDto node, byte parameter, List<byte> value)
        {
            return ConfigurationBusiness.Set(controller, node, parameter, value);
        }

        /// <summary>
        /// Get the current value of a node.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="node">Node to call.</param>
        /// <returns>Node value.</returns>
        internal static bool Get(ControllerDto controller, DeviceDto node)
        {
            var result = false;

            if (node.DeviceClassGeneric != DeviceClassGeneric.Other)
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(node.DeviceClassGeneric);
                if (processAttr != null)
                {
                    var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                    if(iDevice != null) result = iDevice.Get(controller, node);
                }
            }

            return result;
        }

        /// <summary>
        /// List controllers.
        /// </summary>
        /// <returns></returns>
        internal static List<ControllerDto> ListControllers()
        {
            // list controllers
            var result = ControllerBusiness.List();

            // get nodes value
            foreach(var c in result)
            {
                c.Nodes.Where(x => x.DeviceClassGeneric != DeviceClassGeneric.StaticController).ToList().ForEach(x =>
                {
                    Get(c, x);
                    (new ConstructorBusiness()).Get(c, x);
                });
            }

            return result;
        }

        /// <summary>
        /// Process a received message.
        /// Dispatch message to concerned device.
        /// </summary>
        /// <param name="messageProcessing">Message process.</param>
        /// <returns>Next state of request message.</returns>
        internal static void ResponseReceived(MessageProcessDto messageProcessing)
        {
            IDevice iDevice = null;

            // configuration command
            if (messageProcessing.MessageTo.IsConfiguration)
            {
                iDevice = new ConfigurationBusiness();
            }
            // constructor command
            else if (messageProcessing.MessageTo.IsConstructor)
            {
                iDevice = new ConstructorBusiness();
            }
            // generic response
            else
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(messageProcessing.MessageFrom.Node.DeviceClassGeneric);
                if (processAttr != null) iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
            }

            // execute
            iDevice?.ResponseReceived(messageProcessing.MessageFrom);
        }

        /// <summary>
        /// Process a request received.
        /// </summary>
        /// <param name="received">Recevied message.</param>
        /// <returns>Process state.</returns>
        internal static void RequestReceived(MessageFromDto received)
        {
            // process request
            var processAttr = ReflectionHelper.GetEnumValueAttribute<RequestCommandClass, DataReceivedAttribute>(received.RequestCommand);
            if (processAttr != null)
            {
                var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                iDevice?.RequestRecevied(received);
            }
        }

        /// <summary>
        /// Basic set action.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="node">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted.</returns>
        internal static bool Set(ControllerDto controller, DeviceDto node, List<byte> value)
        {
            var result = false;

            if (controller != null && controller.IsReady && node != null && value != null && value.Count > 0 && node.DeviceClassGeneric != DeviceClassGeneric.Other)
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(node.DeviceClassGeneric);
                if (processAttr != null)
                {
                    var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                    if (iDevice != null) result = iDevice.Set(controller, node, value);
                }
            }

            return result;
        }

        #endregion

    }
}
