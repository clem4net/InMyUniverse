using System;
using System.Linq;
using Z4Net.Dto.Services;
using Z4Net.DtoFactory.Services;

namespace Z4Net.Business.Services
{
    /// <summary>
    /// Manage node parameters.
    /// </summary>
    internal class NodeParameterBusiness
    {

        #region Internal methods

        /// <summary>
        /// Get the complete node parameter.
        /// </summary>
        /// <param name="node">Concerned node.</param>
        /// <param name="parameter">Parameter from configuration request.</param>
        /// <returns>Complete parameter.</returns>
        internal static NodeParameterDto Get(NodeDto node, NodeParameterDto parameter)
        {
            // find parameter from database
            var result = node?.Parameters?.FirstOrDefault(x => x.Identifier == parameter.Identifier);

            // if not exists, create new
            if (result == null)
            {
                var definitionParameter = node?.Product?.Parameters?.FirstOrDefault(x => x.Identifier == parameter.DefinitionIdentifier);
                if (definitionParameter != null)
                {
                    result = new NodeParameterDto
                    {
                        Definition = definitionParameter,
                        DefinitionIdentifier = definitionParameter.Identifier,
                        Node = node,
                        NodeIdentifier = node.Identifier,
                        Update = DateTime.Now,
                        Value = parameter.Value
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Create or update the node parameter.
        /// </summary>
        /// <param name="parameter">Parameter to save.</param>
        /// <returns>Process result.</returns>
        internal static bool Save(NodeParameterDto parameter)
        {
            parameter.Update = DateTime.Now;

            bool result;
            using (var ctx = new NodeParameterDtoFactory())
            {
                result = ctx.Save(parameter);
            }
            return result;
        }

        #endregion

    }
}
