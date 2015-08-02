using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Serial;

namespace Z4Net.Tests
{
    [TestClass]
    public class ZServiceTests
    {

        #region Private properties

        /// <summary>
        /// Z Service access.
        /// </summary>
        private ZService Service { get; } = new ZService();

        #endregion

        #region Connect

        /// <summary>
        /// Connect to controler in nominal case.
        /// </summary>
        [TestMethod]
        public void Connect_Nominal()
        {
            // Initialize
            var controler = Service.GetControlers().FirstOrDefault();
            if (controler == null) return;

            // Execute
            var test = Service.Connect(controler);

            // Test
            Assert.IsTrue(test.IsReady);
            Assert.IsFalse(string.IsNullOrEmpty(test.ZVersion));
            Assert.IsNotNull(test.Nodes);

            // Clean
            Service.Close();
        }

        /// <summary>
        /// Test to connect with a bad controler.
        /// </summary>
        [TestMethod]
        public void Connect_BadControler()
        {
            // Initialize
            var controler = new ControlerDto
            {
                Port = new PortDto { Name = "COM1000" },
                DeviceClass = DeviceClass.StaticControler,
                DeviceClassGeneric = DeviceClassGeneric.StaticControler
            };

            // Execute
            var test = Service.Connect(controler);

            // Test
            Assert.IsFalse(test.IsReady);
            Assert.IsTrue(string.IsNullOrEmpty(test.ZVersion));

            // Clean
            Service.Close();
        }

        /// <summary>
        /// Try to connect with null arguments.
        /// </summary>
        [TestMethod]
        public void Connect_Error()
        {
            // Execute & test
            var test = Service.Connect(null);
            Assert.IsFalse(test.IsReady);

            // Execute & test
            test = Service.Connect(new ControlerDto());
            Assert.IsFalse(test.IsReady);

            // Execute & test
            test = Service.Connect(new ControlerDto { Port = new PortDto() });
            Assert.IsFalse(test.IsReady);
        }

        #endregion

        #region GetControlers

        /// <summary>
        /// Test to get the list of controlers.
        /// </summary>
        [TestMethod]
        public void GetControlers_Nominal()
        {
            // Initialize
            var ports = SerialPort.GetPortNames();

            // Execute
            var test = Service.GetControlers();

            // Test
            Assert.AreEqual(test.Count, ports.Length);
            if (test.Count != 0)
            {
                foreach (var p in ports)
                {
                    var testPort = test.FirstOrDefault(x => x.Port.Name == p);
                    Assert.IsNotNull(testPort);
                    Assert.IsFalse(string.IsNullOrEmpty(testPort.HomeIdentifier));
                    Assert.AreNotEqual(testPort.ZIdentifier, 0);
                }
            }
        }

        #endregion

        #region Set

        /// <summary>
        /// Set value to a node.
        /// </summary>
        [TestMethod]
        public void Set_Nominal()
        {
            // Initialize
            var controler = Service.GetControlers().FirstOrDefault();
            if (controler == null) return;
            controler = Service.Connect(controler);
            Assert.IsTrue(controler.IsReady);
            if (controler.Nodes.Count == 0) return;
            var node = controler.Nodes.FirstOrDefault(x => x.DeviceClassGeneric == DeviceClassGeneric.SwitchBinary);
            if (node == null) return;

            // Execute
            var value = node.Value == "FF" ? new List<byte> {0x00} : new List<byte> {0xFF};
            var test = Service.Set(controler, node, value);
            
            // Test
            Assert.IsTrue(test);

            // Clean
            Service.Close();
        }

        #endregion

    }
}
