using System.Collections.Generic;
using System.Linq;
using Z4NetScheduler.Business.Events;
using Z4NetScheduler.Dto;
using Z4NetScheduler.DtoFactory;

namespace Z4NetScheduler.Business
{
    /// <summary>
    /// Task management.
    /// </summary>
    internal static class TaskBusiness
    {

        #region Internal methods

        internal static void Process()
        {
            var tasks = List();

            foreach (var t in tasks)
            {
                if (CanTrigger(t))
                {
                    
                }
            }
        }

        #endregion

        #region Private method

        /// <summary>
        /// List tasks.
        /// </summary>
        /// <returns>Task list with events.</returns>
        private static List<TaskDto> List()
        {
            List<TaskDto> result;
            using (var ctx = new TaskDtoFactory())
            {
                result = ctx.List();
            }
            return result;
        }

        /// <summary>
        /// Test events of the task.
        /// </summary>
        /// <param name="task">Task to test.</param>
        /// <returns>True if event can be triggered, else false.</returns>
        private static bool CanTrigger(TaskDto task)
        {
            return task.Events.Select(AEventBusiness.GetBusiness)
                .Aggregate(false, (current, business) => current | business.Test());
        }

        #endregion

    }
}
