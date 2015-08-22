using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Z4NetScheduler.Dto.Events
{
    /// <summary>
    /// Specific time event.
    /// </summary>
    [Table("Scheduler_TimeEvents")]
    public class TimeEventDto : EventDto
    {

        /// <summary>
        /// Days and times of the event.
        /// </summary>
        [NotMapped]
        public Dictionary<DayOfWeek, List<TimeSpan>> DayTimes
        {
            get { return ConvertToDays(DayTimesProxy); }
            set { DayTimesProxy = ConvertToString(value); }
        }

        /// <summary>
        /// Proxy to save day times.
        /// </summary>
        [Column("TIMES")]
        public string DayTimesProxy { get; set; }

        #region Private methods

        /// <summary>
        /// Serialize data.
        /// </summary>
        /// <param name="data">Days to serialize.</param>
        /// <returns>Serialized data.</returns>
        private string ConvertToString(Dictionary<DayOfWeek, List<TimeSpan>> data)
        {
            var values = from d in DayTimes.Keys
                let times = string.Join(",", DayTimes[d].Select(x => x.Ticks))
                select string.Concat((int) d, ":", times);

            return string.Join(";", values);
        }

        /// <summary>
        /// Deserialize data.
        /// </summary>
        /// <param name="value">Serialized data.</param>
        /// <returns>Deserializes data.</returns>
        private Dictionary<DayOfWeek, List<TimeSpan>> ConvertToDays(string value)
        {
            var result = new Dictionary<DayOfWeek, List<TimeSpan>>();

            var days = value.Split(';');
            foreach (var d in days)
            {
                var dayData = d.Split(':');
                int dayOfWeek;
                if (dayData.Length == 2 && int.TryParse(dayData[0], out dayOfWeek))
                {
                    // convert times
                    var times = dayData[1].Split(',');
                    var listTimes = new List<TimeSpan>();
                    foreach (var t in times)
                    {
                        TimeSpan tmpTime;
                        if (TimeSpan.TryParse(t, out tmpTime)) listTimes.Add(tmpTime);
                    }

                    // add data
                    var finalDayOfWeek = (DayOfWeek) dayOfWeek;
                    if (listTimes.Count > 0 && !result.ContainsKey(finalDayOfWeek)) result.Add((DayOfWeek)dayOfWeek, listTimes);
                }
            }

            return result;
        }

        #endregion

    }
}
