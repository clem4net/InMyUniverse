using System.Collections.Generic;
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
                    result = ReflectionHelper.ExecuteStaticMethod<bool>(processAttr.ClassType, "Get", controller, node);
                }
            }

            return result;
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

            if (controller != null && node != null && value != null && value.Count > 0 && node.DeviceClassGeneric != DeviceClassGeneric.Other)
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(node.DeviceClassGeneric);
                if (processAttr != null)
                {
                    result = ReflectionHelper.ExecuteStaticMethod<bool>(processAttr.ClassType, "Set", controller, node, value);
                }
            }

            return result;
        }


        /// <summary>
        /// Process a received message.
        /// Dispatch message to concerned device.
        /// </summary>
        /// <param name="request">Message request.</param>
        /// <param name="response">Message received.</param>
        /// <returns>Next state of request message.</returns>
        internal static void ResponseReceived(MessageDto request, MessageDto response)
        {
            if (response?.Node != null)
            {
                var processAttr = ReflectionHelper.GetEnumValueAttribute<DeviceClassGeneric, DataReceivedAttribute>(response.Node.DeviceClassGeneric);
                if (processAttr != null)
                {
                    ReflectionHelper.ExecuteStaticMethod<object>(processAttr.ClassType,"ResponseReceived", request, response);
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
                    ReflectionHelper.ExecuteStaticMethod<object>(processAttr.ClassType, "RequestRecevied", received);
                }
            }
        }

        #endregion

    }
}
