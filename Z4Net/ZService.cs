using System.Collections.Generic;
using Z4Net.Business.Devices;
using Z4Net.Dto.Devices;

namespace Z4Net
{
    /// <summary>
    /// Z service implementation.
    /// </summary>
    public class ZService : IZService
    {

        #region Public methods

        /// <summary>
        /// Close controlers.
        /// </summary>
        public void Close()
        {
            ControlerBusiness.Close();
        }

        /// <summary>
        /// Connect a controler.
        /// </summary>
        /// <param name="controler">Controler to connect.</param>
        /// <returns>Connected controler.</returns>
        public ControlerDto Connect(ControlerDto controler)
        {
            if (controler?.Port == null) controler = new ControlerDto {Port = new Dto.Serial.PortDto()};
            return ControlerBusiness.Connect(controler);
        }

        /// <summary>
        /// Get controler plugged to the system.
        /// </summary>
        /// <returns>Controler list.</returns>
        public List<ControlerDto> GetControlers()
        {
            return ControlerBusiness.List();
        }

        /// <summary>
        /// Set the value of a node.
        /// </summary>
        /// <param name="controler">Controler to use.</param>
        /// <param name="node">Node to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted, else false.</returns>
        public bool Set(ControlerDto controler, DeviceDto node, List<byte> value)
        {
            return DevicesBusiness.Set(controler, node, value);
        }

        #endregion

    }
}
