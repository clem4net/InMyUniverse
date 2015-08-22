using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Technical;
using Z4NetScheduler.Dto;

namespace Z4NetScheduler.DtoFactory
{
    /// <summary>
    /// Task factory.
    /// </summary>
    public class TaskDtoFactory : BaseFactory<Z4NetSchedulerContext>
    {

        #region Public methods

        /// <summary>
        /// List tasks.
        /// </summary>
        /// <returns>List of tasks.</returns>
        public List<TaskDto> List()
        {
            var sel = from t in Context.Tasks.Include(x => x.Events).Include(x => x.Actions)
                      select t;

            return sel.ToListEx();
        }

        #endregion

    }
}
