using System.Threading;
using System.Threading.Tasks;
using Z4Net.Dto.Devices;

namespace Z4Net.Dto.Messaging
{
    /// <summary>
    /// Represent a message queue.
    /// </summary>
    public class MessageProcessDto
    {

        /// <summary>
        /// Port linked to the queue.
        /// </summary>
        public ControllerDto Controller { get; set; }

        /// <summary>
        /// Message received.
        /// </summary>
        public MessageFromDto MessageFrom { get; set; }

        /// <summary>
        /// Message to send.
        /// </summary>
        public MessageToDto MessageTo { get; set; }

        /// <summary>
        /// State of the receive task.
        /// </summary>
        public bool TaskContinue { get; set; }

        /// <summary>
        /// Thread used to receive messages.s
        /// </summary>
        public Task TaskReceive { get; set; }

        /// <summary>
        /// Event used to wait the end of the message process.
        /// </summary>
        public EventWaitHandle WaitMessageSending { get; } = new EventWaitHandle(true, EventResetMode.AutoReset);

        /// <summary>
        /// Event use to present the process of 2 request in same time.
        /// </summary>
        public EventWaitHandle WaitMessageReception { get; } = new EventWaitHandle(true, EventResetMode.AutoReset);

    }
}
