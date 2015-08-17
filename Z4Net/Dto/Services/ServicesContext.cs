using System.Data.Entity;
using Z4Net.Dto.Services.Definitions;

namespace Z4Net.Dto.Services
{
    /// <summary>
    /// Service database context.
    /// </summary>
    public class ServicesContext : DbContext
    {

        /// <summary>
        /// Initialize.
        /// </summary>
        public ServicesContext() : base("name=Z4NetDevices")
        {
        }

        /// <summary>
        /// Labels.
        /// </summary>
        public DbSet<LabelDto> Labels { get; set; }

        /// <summary>
        /// Nodes.
        /// </summary>
        public DbSet<NodeDto> Nodes { get; set; }

        /// <summary>
        /// Parameters of nodes.
        /// </summary>
        public DbSet<NodeParameterDto> NodeParameters { get; set; }

        /// <summary>
        /// Product definitions.
        /// </summary>
        public DbSet<ProductDto> ProductDefinitions { get; set; }

    }
}
