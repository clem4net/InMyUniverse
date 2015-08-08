    namespace Z4Net.Dto.Messaging
{
    /// <summary>
    /// Message send to node.
    /// </summary>
    public class MessageToDto : AMessageDto
    {

        /// <summary>
        /// True if message is a configuration message.
        /// </summary>
        public bool IsConfiguration { get; set; }

    }
}
