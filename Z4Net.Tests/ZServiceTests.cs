using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Serial;

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
            // Initialize
            var controller = Service.GetControllers().FirstOrDefault();
            if (controller == null) return;
            controller = Service.Connect(controller);
            Assert.IsTrue(controller.IsReady);
            if (controller.Nodes.Count == 0) return;
            var node = controller.Nodes.FirstOrDefault(x => x.DeviceClassGeneric == DeviceClassGeneric.SwitchBinary);
            if (node == null) return;

            // Execute
            var test = Service.Configure(controller, node, 0x04, new List<byte> {15});
            Assert.IsTrue(test);

            // Test
            test = Service.Set(controller, node, new List<byte> { 0xFF });
            Assert.IsTrue(test);
            Assert.AreEqual(node.Value, "FF");
            Thread.Sleep(1500);
            test = Service.Get(controller, node);
            Assert.IsTrue(test);
            Assert.AreEqual(node.Value, "00");

            // Clean
            Service.Configure(controller, node, 0x04, new List<byte> { 0 });
            Service.Close();
        }

        #endregion

        #region Connect

        /// <summary>
        /// Connect to controller in nominal case.
        /// </summary>
        [TestMethod]
        public void Connect_Nominal()
        {
            // Initialize
            var controller = Service.GetControllers().FirstOrDefault();
            if (controller == null) return;

            // Execute
            var test = Service.Connect(controller);

            // Test
            Assert.IsTrue(test.IsReady);
            Assert.IsFalse(string.IsNullOrEmpty(test.HomeIdentifier));
            Assert.IsNotNull(test.Nodes);

            // Clean
            Service.Close();
        }

        /// <summary>
        /// Test to connect with a bad controller.
        /// </summary>
        [TestMethod]
        public void Connect_BadController()
        {
            // Initialize
            var controller = new ControllerDto
            {
                Port = new PortDto { Name = "COM1000" },
                DeviceClass = DeviceClass.StaticController,
                DeviceClassGeneric = DeviceClassGeneric.StaticController
            };

            // Execute
            var test = Service.Connect(controller);

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
            test = Service.Connect(new ControllerDto());
            Assert.IsFalse(test.IsReady);

            // Execute & test
            test = Service.Connect(new ControllerDto { Port = new PortDto() });
            Assert.IsFalse(test.IsReady);
        }

        #endregion

        #region GetControllers

        /// <summary>
        /// Test to get the list of controllers.
        /// </summary>
        [TestMethod]
        public void GetControllers_Nominal()
        {
            // Initialize
            var ports = SerialPort.GetPortNames();

            // Execute
            var test = Service.GetControllers();

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
            var controller = Service.GetControllers().FirstOrDefault();
            if (controller == null) return;
            controller = Service.Connect(controller);
            Assert.IsTrue(controller.IsReady);
            if (controller.Nodes.Count == 0) return;
            var node = controller.Nodes.FirstOrDefault(x => x.DeviceClassGeneric == DeviceClassGeneric.SwitchBinary);
            if (node == null) return;
            var conf = Service.Configure(controller, node, 0x04, new List<byte> { 0 });
            Assert.IsTrue(conf);

            // Execute
            var value = new List<byte> {0xFF};
            var test = Service.Set(controller, node, value);
            
            // Test
            Assert.IsTrue(test);
            Assert.AreEqual(node.Value, "FF");
            Assert.IsTrue(Service.Get(controller, node));
            Assert.AreEqual(node.Value, "FF");

            // Clean
            Thread.Sleep(500);
            Service.Set(controller, node, new List<byte> {0x00});
            Service.Close();
        }

        /// <summary>
        /// Set a value to a non connected controller.
        /// </summary>
        [TestMethod]
        public void Set_BadController()
        {
            // Initialize
            var controller = Service.GetControllers().FirstOrDefault();
            if (controller == null) return;

            // Execute
            var test = Service.Set(controller,
                new DeviceDto
                {
                    DeviceClass = DeviceClass.StaticController,
                    DeviceClassGeneric = DeviceClassGeneric.StaticController,
                    ZIdentifier = 2
                }, new List<byte> {0xFF});

            // Test
            Assert.IsFalse(test);

            // Clean
            Service.Close();
        }

        /// <summary>
        /// Set value with bad error.
        /// </summary>
        [TestMethod]
        public void Set_Error()
        {
            // Execute & test
            var test = Service.Set(null, null, null);
            Assert.IsFalse(test);

            // Execute & test
            test = Service.Set(new ControllerDto(), null, null);
            Assert.IsFalse(test);

            // Execute & test
            test = Service.Set(new ControllerDto(), new DeviceDto(), null);
            Assert.IsFalse(test);

            // Execute & test
            test = Service.Set(new ControllerDto(), new DeviceDto(), new List<byte>());
            Assert.IsFalse(test);
        }

        #endregion

    }
}
