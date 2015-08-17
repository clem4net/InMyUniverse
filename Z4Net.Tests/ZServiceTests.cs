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

        #region Disconnect

        /// <summary>
        /// Test close.
        /// </summary>
        [TestMethod]
        public void Disconnect_Nominal()
        {
            // Initialize

            // Execute
            Service.Disconnect();

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
            var switchBinary = Service.GetNodes().FirstOrDefault(x => x.Type == DeviceClassGeneric.SwitchBinary);

            // get auto off parameter of fibaro switch
            var parameter = switchBinary?.Product.Parameters.FirstOrDefault(x => x.ZIdentifier == 4);
            if (parameter == null) return;
            var nodeParameter = new NodeParameterDto
            {
                DefinitionIdentifier = parameter.Identifier,
                NodeIdentifier = switchBinary.Identifier,
                Value = new List<byte> {10} // 1 sec
            };

            // Execute
            var test = Service.Configure(nodeParameter);

            // Test
            Assert.IsTrue(test);

            switchBinary.Value = new List<byte> {0xFF};
            test = Service.Set(switchBinary);
            Assert.IsTrue(test);
            Assert.IsTrue(switchBinary.Value.SequenceEqual(new List<byte> { 0xFF }));
            Thread.Sleep(1200);

            // Clean
            nodeParameter.Value = new List<byte> { 0x00 }; // off
            Service.Configure(nodeParameter);
            Service.Disconnect();
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
            Service.Disconnect();
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
            Service.Disconnect();
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
