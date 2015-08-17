using System.Data.Entity;
using Technical;
using Z4Net.Dto.Services;

namespace Z4Net.DtoFactory.Services
{
    /// <summary>
    /// Node parameter factory.
    /// </summary>
    public class NodeParameterDtoFactory : BaseFactory<ServicesContext>
    {

        #region Public methods

        /// <summary>
        /// Create or update the node parameter.
        /// </summary>
        /// <param name="parameter">Parameter to save.</param>
        /// <returns>Process result.</returns>
        public bool Save(NodeParameterDto parameter)
        {
            Context.Nodes.Attach(parameter.Node);

            if (parameter.Identifier != 0)
            {
                Context.NodeParameters.Attach(parameter);
                Context.Entry(parameter).State = EntityState.Modified;
            }
            else
            {
                Context.NodeParameters.Add(parameter);
            }

            return Context.SaveChangesAsyncEx();
        }

        #endregion

    }
}
