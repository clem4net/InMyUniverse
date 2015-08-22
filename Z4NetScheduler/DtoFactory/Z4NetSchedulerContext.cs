using System.Data.Entity;
using Z4NetScheduler.Dto;

namespace Z4NetScheduler.DtoFactory
{
    /// <summary>
    /// Service database context.
    /// </summary>
    public class Z4NetSchedulerContext : DbContext
    {

        /// <summary>
        /// Initialize.
        /// </summary>
        public Z4NetSchedulerContext() : base("name=Z4NetDevices")
        {
        }

        /// <summary>
        /// Tasks.
        /// </summary>
        public DbSet<TaskDto> Tasks { get; set; }

    }
}
