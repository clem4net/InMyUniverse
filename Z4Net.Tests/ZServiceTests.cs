using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Services;

namespace Z4Net.Tests
{
    /// <summary>
    /// Test Z service.
    /// </summary>
    [TestClass]
    public class ZServiceTests
    {

        #region Private properties

        /// <summary>
        /// Z Service access.
        /// </summary>
        private ZService Service { get; } = new ZService();

        #endregion

        #region Close

        /// <summary>
        /// Test close.
        /// </summary>
        [TestMethod]
        public void Close_Nominal()
        {
            // Initialize

            // Execute
            Service.Close();

            // Test

            // Clean
        }

        #endregion

        #region Configure

        /// <summary>
        /// Test configure.
        /// </summary>
        [TestMethod]
        public void Configure_Nominal()
        {
            //// Initialize
            //var controller = Service.GetControllers().FirstOrDefault();
            //if (controller == null) return;
            //controller = Service.Connect(controller);
            //Assert.IsTrue(controller.IsReady);
            //if (controller.Nodes.Count == 0) return;
            //var node = controller.Nodes.FirstOrDefault(x => x.DeviceClassGeneric == DeviceClassGeneric.SwitchBinary);
            //if (node == null) return;

            //// Execute
            //var test = Service.Configure(controller, node, 0x04, new List<byte> {15});
            //Assert.IsTrue(test);

            //// Test
            //test = Service.Set(controller, node, new List<byte> { 0xFF });
            //Assert.IsTrue(test);
            //Assert.AreEqual(node.Value, "FF");
            //Thread.Sleep(1500);
            //test = Service.Get(controller, node);
            //Assert.IsTrue(test);
            //Assert.AreEqual(node.Value, "00");

            //// Clean
            //Service.Configure(controller, node, 0x04, new List<byte> { 0 });
            //Service.Close();
        }

        #endregion

        #region GetNodes

        /// <summary>
        /// Get node list.
        /// </summary>
        [TestMethod]
        public void GetNodes_Nominal()
        {
            // Initialize
            var ports = SerialPort.GetPortNames();
            if (ports.Length == 0) return;

            // Execute
            var test = Service.GetNodes();

            // Test
            Assert.AreNotEqual(test.Count, 0);

            // clean
            Service.Close();
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
            var nodes = Service.GetNodes();
            var switchNode = nodes.FirstOrDefault(x => x.Type == DeviceClassGeneric.SwitchBinary);
            if (switchNode == null) return;

            // Execute
            switchNode.Value = new List<byte> { 0xFF };
            var test = Service.Set(switchNode);

            // Test
            Assert.IsTrue(test);

            // Clean
            Thread.Sleep(500);
            switchNode.Value = new List<byte> { 0x00 };
            Service.Set(switchNode);
            Service.Close();
        }

        /// <summary>
        /// Set value with bad error.
        /// </summary>
        [TestMethod]
        public void Set_Error()
        {
            // Execute & test
            var test = Service.Set(null);
            Assert.IsFalse(test);

            // Execute & test
            test = Service.Set(new NodeDto());
            Assert.IsFalse(test);
        }

        #endregion

    }
}
