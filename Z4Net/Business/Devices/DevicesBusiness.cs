using System.Collections.Generic;
using System.Linq;
using Technical;
using Z4Net.Dto.Attributes;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;

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
        /// <param name="message">Concerned message.</param>
        internal static void AcknowledgmentReceived(MessageDto message)
        {
            var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(message.Node.DeviceClassGeneric);
            if (processAttr != null)
            {
                var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                iDevice?.AcknowlegmentReceived((MessageHeader)message.Content[0]);
            }
        }

        /// <summary>
        /// Close all devices.
        /// </summary>
        internal static void Close()
        {
            ControllerBusiness.Close();
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
        /// Connect to a controller.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        internal static ControllerDto Connect(ControllerDto controller)
        {
            // initialize controller
            var result = ControllerBusiness.Connect(controller);

            // get nodes value
            if (result.IsReady)
            {
                result.Nodes.Where(x => x.DeviceClassGeneric != DeviceClassGeneric.StaticController).ToList().ForEach(x => Get(result, x));
            }

            return result;
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
            return ControllerBusiness.List();
        }

        /// <summary>
        /// Process a received message.
        /// Dispatch message to concerned device.
        /// </summary>
        /// <param name="response">Message received.</param>
        /// <returns>Next state of request message.</returns>
        internal static void ResponseReceived(MessageDto response)
        {
            if (response?.Node != null)
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(response.Node.DeviceClassGeneric);
                if (processAttr != null)
                {
                    var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                    iDevice?.ResponseReceived(response);
                }
            }
        }

        /// <summary>
        /// Process a request received.
        /// </summary>
        /// <param name="received">Recevied message.</param>
        /// <param name="commandClass">Cillabd ckass?</param>
        /// <returns>Process state.</returns>
        internal static void RequestReceived(MessageDto received, CommandClass commandClass)
        {
            if (received != null && commandClass != CommandClass.None)
            {
                // process request
                var processAttr = ReflectionHelper.GetEnumValueAttribute<CommandClass, DataReceivedAttribute>(commandClass);
                if (processAttr != null)
                {
                    var iDevice = ReflectionHelper.CreateInstance<IDevice>(processAttr.ClassType);
                    iDevice?.RequestRecevied(received);
                }
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
