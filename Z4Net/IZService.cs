using System.Collections.Generic;
using System.ServiceModel;
using Z4Net.Dto.Services;

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
        /// <param name="device">Device to configure.</param>
        /// <param name="parameter">Parameter identifier.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Configuration result.</returns>
        [OperationContract]
        bool Configure(NodeDto device, byte parameter, List<byte> value);

        /// <summary>
        /// Get controller plugged to the system.
        /// </summary>
        /// <returns>Controller list.</returns>
        [OperationContract]
        List<NodeDto> GetNodes();

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="node">Node to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        [OperationContract]
        bool Set(NodeDto node);

    }
}
