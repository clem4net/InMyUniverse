using System.Linq;
using System.Data.Entity;
using Technical;
using Z4Net.Dto.Services;
using Z4Net.Dto.Services.Definitions;

namespace Z4Net.DtoFactory.Services.Definitions
{
    /// <summary>
    /// Product factory.
    /// </summary>
    public class ProductDtoFactory : BaseFactory<ServicesContext>
    {

        #region Public methods

        /// <summary>
        /// Get product definition.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Product definition.</returns>
        public ProductDto Get(NodeDto node)
        {
            var sel = from p in Context.ProductDefinitions.Include(x => x.Constructor).Include(x => x.Parameters)
                where
                    p.ProductIdentifier == node.ProductIdentifier &&
                    p.ConstructorIdentifier == node.ConstructorIdentifier
                select p;

            return sel.FirstOrDefaultEx();
        }

        #endregion

    }
}
