using System.Data.Entity;
using Z4Net.Dto.Services;

namespace Z4Net.DtoFactory.Services
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
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Nodes.
        /// </summary>
        public DbSet<NodeDto> Nodes { get; set; }

    }
}
