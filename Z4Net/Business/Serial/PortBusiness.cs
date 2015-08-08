using System.Collections.Generic;
using System.Linq;
using Z4Net.Dto.Serial;
using Z4Net.DtoFactory.Serial;

namespace Z4Net.Business.Serial
{
    /// <summary>
    /// COM port management.s
    /// </summary>
    internal static class PortBusiness
    {

        #region Private properties

        /// <summary>
        /// Port used.
        /// </summary>
        private static Dictionary<string, PortDto> Ports { get; } = new Dictionary<string, PortDto>();

        #endregion

        #region Internal methods

        /// <summary>
        /// Close all ports.
        /// </summary>
        /// <param name="port">Optional port to close. If null, close all ports.</param>
        internal static void Close(PortDto port = null)
        {
            if (Ports != null)
            {
                using (var ctx = new PortDtoFactory())
                {
                    if (port == null)
                    {
                        Ports.Values.ToList().ForEach(x => ctx.Close(x));
                    }
                    else
                    {
                        ctx.Close(port);
                    }
                }
                Ports.Clear();
            }
        }

        /// <summary>
        /// Connect to port.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <returns>Port.</returns>
        internal static PortDto Connect(PortDto port)
        {
            PortDto result;

            // search if port is already existing
            var portName = port.Name?.ToUpperInvariant() ?? string.Empty;

            if (!string.IsNullOrEmpty(portName))
            {
                // add port
                if (!Ports.ContainsKey(portName)) Ports.Add(portName, new PortDto {Name = portName});

                // connect port
                result = Ports[portName];
                if (result.IsOpen == false)
                {
                    using (var ctx = new PortDtoFactory())
                    {
                        result = ctx.Connect(result);
                    }
                }
            }
            else
            {
                result = new PortDto();
            }

            return result;
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
