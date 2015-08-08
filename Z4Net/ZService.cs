using System.Collections.Generic;
using Z4Net.Business.Devices;
using Z4Net.Dto.Devices;

namespace Z4Net
{
    /// <summary>
    /// Z service implementation.
    /// </summary>
    public class ZService : IZService
    {

        #region Public methods

        /// <summary>
        /// Close controllers.
        /// </summary>
        public void Close()
        {
            DevicesBusiness.Close();
        }

        /// <summary>
        /// Configure a device.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="device">Device to configure.</param>
        /// <param name="parameter">Parameter identifier.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Configuration result.</returns>
        public bool Configure(ControllerDto controller, DeviceDto device, byte parameter, List<byte> value)
        {
            return DevicesBusiness.Configure(controller, device, parameter, value);
        }

        /// <summary>
        /// Connect a controller.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        public ControllerDto Connect(ControllerDto controller)
        {
            if (controller?.Port == null) controller = new ControllerDto {Port = new Dto.Serial.PortDto()};
            return DevicesBusiness.Connect(controller);
        }

        /// <summary>
        /// Get device value.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned node.</param>
        /// <returns>True if value is got.</returns>
        public bool Get(ControllerDto controller, DeviceDto device)
        {
            return DevicesBusiness.Get(controller, device);
        }

        /// <summary>
        /// Get controller plugged to the system.
        /// </summary>
        /// <returns>Controller list.</returns>
        public List<ControllerDto> GetControllers()
        {
            return DevicesBusiness.ListControllers();
        }

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="device">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        public bool Set(ControllerDto controller, DeviceDto device, List<byte> value)
        {
            return DevicesBusiness.Set(controller, device, value);
        }

        #endregion

    }
}
