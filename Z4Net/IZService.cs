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
        /// Connect a controler.
        /// </summary>
        /// <param name="controler">Controler to connect.</param>
        /// <returns>Connected controler.</returns>
        [OperationContract]
        ControlerDto Connect(ControlerDto controler);

        /// <summary>
        /// Get controler plugged to the system.
        /// </summary>
        /// <returns>Controler list.</returns>
        [OperationContract]
        List<ControlerDto> GetControlers();

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="controler">Controler to use.</param>
        /// <param name="node">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        [OperationContract]
        bool Set(ControlerDto controler, DeviceDto node, List<byte> value);

    }
}
