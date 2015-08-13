using System.Collections.Generic;
using Z4Net.Dto.Serial;
using Z4Net.DtoFactory.Serial;

namespace Z4Net.Business.Serial
{
    /// <summary>
    /// COM port management.s
    /// </summary>
    internal static class PortBusiness
    {

        #region Internal methods

        /// <summary>
        /// Close all ports.
        /// </summary>
        /// <param name="port">Port to close.</param>
        internal static void Close(PortDto port)
        {
            using (var ctx = new PortDtoFactory())
            {
                ctx.Close(port);
            }
        }

        /// <summary>
        /// Connect to port.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <returns>Port.</returns>
        internal static PortDto Connect(PortDto port)
        {
            var portName = port.Name?.ToUpperInvariant() ?? string.Empty;

            if (!string.IsNullOrEmpty(portName))
            {
                if (port.IsOpen == false)
                {
                    using (var ctx = new PortDtoFactory())
                    {
                        port = ctx.Connect(port);
                    }
                }
            }
            else
            {
                port.IsOpen = false;
            }

            return port;
        }

        /// <summary>
        /// List existing serial ports.
        /// </summary>
        internal static List<PortDto> List()
        {
            List<PortDto> result;

            using (var ctx = new PortDtoFactory())
            {
                result = ctx.List();
            }

            return result;
        }

        /// <summary>
        /// Get the next complete message.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <return>Received message.</return>
        internal static MessageDto Receive(PortDto port)
        {
            MessageDto result;
            do
            {
                using (var ctx = new PortDtoFactory())
                {
                    result = ctx.Receive(port);
                }
            } while (result.IsComplete == false && result.Size != 0);

            return result;
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="port">Port to use containing message.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Send result.</returns>
        internal static bool Send(PortDto port, MessageDto message)
        {
            bool result;

            using (var ctx = new PortDtoFactory())
            {
                result = ctx.Send(port, message);
            }

            return result;
        }

        #endregion

    }
}
