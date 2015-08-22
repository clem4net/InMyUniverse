using Z4NetScheduler.Dto.Events;

namespace Z4NetScheduler.Business.Events
{
    /// <summary>
    /// Event business interface.
    /// </summary>
    internal abstract class AEventBusiness
    {

        #region Internal properties

        /// <summary>
        /// Event concerned.
        /// </summary>
        internal EventDto Event { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="evt">Event.</param>
        internal AEventBusiness(EventDto evt)
        {
            Event = evt;
        }

        /// <summary>
        /// Test the event.
        /// </summary>
        /// <returns>Event result.</returns>
        internal abstract bool Test();

        #endregion

        #region Internal static methods

        /// <summary>
        /// Get business class corresponding to an event.
        /// </summary>
        /// <param name="evt">Event.</param>
        /// <returns>Busines class.</returns>
        internal static AEventBusiness GetBusiness(EventDto evt)
        {
            AEventBusiness result;

            if (evt is TimeEventDto)
            {
                result = new TimeEventBusiness(evt);
            }
            else
            {
                result = null;
            }

            return result;
        }

        #endregion

    }
}
