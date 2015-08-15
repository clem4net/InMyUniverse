using System.Collections.Generic;
using System.Linq;
using Z4Net.Dto.Services;
using Z4Net.Dto.Services.Definitions;
using Z4Net.DtoFactory.Services.Definitions;

namespace Z4Net.Business.Services.Definitions
{
    /// <summary>
    /// Product definition business.
    /// </summary>
    internal static class ProductBusiness
    {

        #region Internal methods

        /// <summary>
        /// Get product linked to the device.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Product result.</returns>
        internal static ProductDto Get(NodeDto node)
        {
            ProductDto result;
            using (var ctx = new ProductDtoFactory())
            {
                result = ctx.Get(node) ?? new ProductDto {Constructor = new ConstructorDto()};
            }
            if (result.Parameters == null) result.Parameters = new List<ProductParameterDto>();

            // complete labels
            var labelIds = new List<int> {result.NameIdentifier};
            labelIds.AddRange(result.Parameters.Select(x => x.DescriptionIdentifier));
            labelIds.AddRange(result.Parameters.Select(x => x.NameLabelIdentifier));
            var labels = LabelBusiness.Get(labelIds.ToArray());

            result.Name = labels[result.NameIdentifier];
            foreach (var p in result.Parameters)
            {
                p.Description = labels[p.DescriptionIdentifier];
                p.Name = labels[p.NameLabelIdentifier];
            }

            return result;
        }

        #endregion

    }
}
