namespace Z4Net.Dto.Messaging
{
    /// <summary>
    /// Z constants.
    /// </summary>
    public static class MessageConstants
    {

        /// <summary>
        /// The time in MS before timeout when a message response is waiting.
        /// </summary>
        public static int WaitResponseTimeout => 5000;

        /// <summary>
        /// The time in MS to wait between 2 loops when a response is waiting.
        /// </summary>
        public static int WaitResponseLoop => 25;

        /// <summary>
        /// Time in MS to wait between 2 loops to receive messages.
        /// </summary>
        public static int WaitSendTask => 50;
    }
}
