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
        /// Connect a controller.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        [OperationContract]
        ControllerDto Connect(ControllerDto controller);

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
        /// <param name="node">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        [OperationContract]
        bool Set(ControllerDto controller, DeviceDto node, List<byte> value);

    }
}
