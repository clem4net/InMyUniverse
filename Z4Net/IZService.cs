using System.Collections.Generic;
using System.ServiceModel;
using Z4Net.Dto.Devices;

namespace Z4Net
{
    /// <summary>
    /// Z service interface.
    /// </summary>
    [ServiceContract]
    public interface IZService
    {

        /// <summary>
        /// Close connection.
        /// </summary>
        [OperationContract]
        void Close();

        /// <summary>
        /// Configure a device.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="device">Device to configure.</param>
        /// <param name="parameter">Parameter identifier.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Configuration result.</returns>
        bool Configure(ControllerDto controller, DeviceDto device, byte parameter, List<byte> value);

        /// <summary>
        /// Connect a controller.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        [OperationContract]
        ControllerDto Connect(ControllerDto controller);

        /// <summary>
        /// Get device value.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned node.</param>
        /// <returns>True if value is got..</returns>
        bool Get(ControllerDto controller, DeviceDto device);

        /// <summary>
        /// Get controller plugged to the system.
        /// </summary>
        /// <returns>Controller list.</returns>
        [OperationContract]
        List<ControllerDto> GetControllers();

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="device">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        [OperationContract]
        bool Set(ControllerDto controller, DeviceDto device, List<byte> value);

    }
}
