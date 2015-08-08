using System.Collections.Generic;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;

namespace Z4Net.Business.Devices
{
    /// <summary>
    /// Device interface.
    /// </summary>
    internal interface IDevice
    {

        /// <summary>
        /// Get the value of the device.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <returns>True if device value is completed.</returns>
        bool Get(ControllerDto controller, DeviceDto device);

        /// <summary>
        /// Set the device value.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted.</returns>
        bool Set(ControllerDto controller, DeviceDto device, List<byte> value);

        /// <summary>
        /// Acknowlegment received.
        /// </summary>
        /// <param name="message">Received message.</param>
        void AcknowlegmentReceived(MessageFromDto message);

        /// <summary>
        /// A response a received from node.
        /// </summary>
        /// <param name="resposne">Response message.</param>
        void ResponseReceived(MessageFromDto resposne);

        /// <summary>
        /// A request is recevied from node.
        /// </summary>
        /// <param name="request">Received message.</param>
        void RequestRecevied(MessageFromDto request);

    }
}
