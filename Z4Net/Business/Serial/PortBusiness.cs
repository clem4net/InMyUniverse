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
        private static Dictionary<string, PortDto> Ports { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// Close all ports.
        /// </summary>
        internal static void Close()
        {
            if (Ports != null)
            {
                using (var ctx = new PortDtoFactory())
                {
                    Ports.Values.ToList().ForEach(x => ctx.Close(x));
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
            if (Ports == null) Ports = new Dictionary<string, PortDto>();

            // search if port is already existing
            var portName = port.Name?.ToUpperInvariant() ?? string.Empty;
            var result = Ports.Where(x => x.Key == portName).Select(x => x.Value).FirstOrDefault();

            // else add new port
            if (result == null)
            {
                result = new PortDto {Name = portName};
                Ports.Add(portName, result);
            }

            // connect port
            if (result.IsOpen == false)
            {
                using (var ctx = new PortDtoFactory())
                {
                    result = ctx.Connect(result);
                }
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
        internal static SerialMessageDto Receive(PortDto port)
        {
            SerialMessageDto result;
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
        internal static bool Send(PortDto port, SerialMessageDto message)
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
