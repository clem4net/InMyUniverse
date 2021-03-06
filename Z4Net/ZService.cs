﻿using System.Collections.Generic;
using Z4Net.Business.Services;
using Z4Net.Dto.Services;

namespace Z4Net
{
    /// <summary>
    /// Z service implementation.
    /// </summary>
    public class ZService : IZService
    {
        /// <summary>
        /// Close connection.
        /// </summary>
        public void Disconnect()
        {
            NodeBusiness.Close();
        }

        /// <summary>
        /// Configure a device.
        /// </summary>
        /// <param name="parameter">Parameter to configure.</param>
        /// <returns>Configuration result.</returns>
        public bool Configure(NodeParameterDto parameter)
        {
            return NodeBusiness.Configure(parameter);
        }

        /// <summary>
        /// Get controller plugged to the system.
        /// </summary>
        /// <returns>Controller list.</returns>
        public List<NodeDto> GetNodes()
        {
            return NodeBusiness.Initialize();
        }

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="node">Node to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        public bool Set(NodeDto node)
        {
            return NodeBusiness.Set(node);
        }
    }
}
