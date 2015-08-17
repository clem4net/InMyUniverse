using System.Collections.Generic;
using System.ServiceModel;
using Z4Net.Dto.Services;
using Z4Net.Dto.Services.Definitions;

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
        void Disconnect();

        /// <summary>
        /// Update an existing parameter.
        /// </summary>
        /// <param name="parameter">Parameter to configure.</param>
        /// <returns>Configuration result.</returns>
        [OperationContract]
        bool Configure(NodeParameterDto parameter);

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
