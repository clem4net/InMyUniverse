using System.Collections.Generic;
using System.Threading.Tasks;
using Z4Net.Dto.Serial;

namespace Z4Net.Dto.Messaging
{
    /// <summary>
    /// Represent a message queue.
    /// </summary>
    public class MessageQueueDto
    {

        /// <summary>
        /// Initialize.
        /// </summary>
        public MessageQueueDto()
        {
            Messages = new Queue<MessageDto>();
        }

        /// <summary>
        /// Message queue.
        /// </summary>
        public Queue<MessageDto> Messages { get; set; }

        /// <summary>
        /// Message in process.
        /// </summary>
        public MessageDto ProcessingMessage { get; set; }

        /// <summary>
        /// State of the queue.
        /// </summary>
        public QueueState State { get; set; }

        /// <summary>
        /// Port linked to the queue.
        /// </summary>
        public PortDto Port { get; set; }

        /// <summary>
        /// Thread used to receive messages.s
        /// </summary>
        public Task TaskReceive { get; set; }

        /// <summary>
        /// State of the receive task.
        /// </summary>
        public bool TaskContinue { get; set; }

        /// <summary>
        /// True if task has been launched.
        /// </summary>
        public bool TaskLaunch { get; set; }


    }
}
