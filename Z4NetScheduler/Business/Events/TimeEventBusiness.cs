using System;
using Z4NetScheduler.Dto.Events;

namespace Z4NetScheduler.Business.Events
{
    /// <summary>
    /// Time event business.
    /// </summary>
    internal class TimeEventBusiness : AEventBusiness
    {

        #region Internal methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="evt">Event.</param>
        internal TimeEventBusiness(EventDto evt) : base(evt)
        {
        }

        /// <summary>
        /// Test the event.
        /// </summary>
        /// <returns>Event result.</returns>
        internal override bool Test()
        {
            var timeEvent = (TimeEventDto)Event;
            var now = DateTime.Now;
            var result = false;

            if (timeEvent.DayTimes.ContainsKey(now.DayOfWeek))
            {
                var times = timeEvent.DayTimes[now.DayOfWeek];
                foreach (var t in times)
                {
                    var testDate = new DateTime(now.Year, now.Month, now.Day, t.Hours, t.Minutes, t.Seconds);
                    if (Event.LastExecution <= testDate && now >= testDate) result = true;
                }
            }

            return result;
        }

        #endregion

    }
}
