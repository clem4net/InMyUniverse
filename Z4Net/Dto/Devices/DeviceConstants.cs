using System;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class DeviceConstants
    {

        /// <summary>
        /// Timeout of wait event.
        /// </summary>
#if DEBUG
        internal static readonly TimeSpan WaitEventTimeout = new TimeSpan(0, 5, 0);
#else
        internal static readonly TimeSpan WaitEventTimeout = new TimeSpan(0, 0, 3);
#endif
    }
}
