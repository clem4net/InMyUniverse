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
        /// Connect a controller.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        public ControllerDto Connect(ControllerDto controller)
        {
            return ControllerBusiness.Initialize(controller);
        }

        /// <summary>
        /// Get controller plugged to the system.
        /// </summary>
        /// <returns>Controller list.</returns>
        public List<ControllerDto> GetControllers()
        {
            return ControllerBusiness.List();
        }

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="controller">Controller to use.</param>
        /// <param name="node">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        public bool Set(ControllerDto controller, DeviceDto node, List<byte> value)
        {
            return DevicesBusiness.Set(controller, node, value);
        }

        #endregion

    }
}
