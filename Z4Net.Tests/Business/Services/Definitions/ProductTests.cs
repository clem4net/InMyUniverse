using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z4Net.Business.Services.Definitions;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Services;
using DeviceDto = Z4Net.Dto.Devices.DeviceDto;

namespace Z4Net.Tests.Business.Services.Definitions
{
    [TestClass]
    public class ProductTests
    {

        #region Get

        /// <summary>
        /// Test to get a product.
        /// </summary>
        [TestMethod]
        public void Get_Nominal()
        {
            // initialize
            var node = new Dto.Services.NodeDto
            {
                ConstructorIdentifier = "010F",
                ProductIdentifier = "100A"
            };

            // execute
            var test = ProductBusiness.Get(node);

            // test
            Assert.AreEqual(test.Name, "Commutateur 16A - FGS211");

            Assert.IsNotNull(test.Constructor);
            Assert.AreEqual(test.Constructor.Name, "FIBARO");

            Assert.AreNotEqual(test.Parameters.Count, 0);
            var prm = test.Parameters.FirstOrDefault(x => x.Identifier == 1);
            Assert.IsNotNull(prm);
            Assert.IsFalse(string.IsNullOrEmpty(prm.Data));
            Assert.IsFalse(string.IsNullOrEmpty(prm.DefaultValue));
            Assert.IsFalse(string.IsNullOrEmpty(prm.Description));
            Assert.IsFalse(string.IsNullOrEmpty(prm.Name));

            // clean
        }

        [TestMethod]
        public void Get_NonExistent()
        {
            // initialize
            var node = new NodeDto
            {
                ConstructorIdentifier = "ZZZZ",
                ProductIdentifier = "ZZZZ"
            };

            // execute
            var test = ProductBusiness.Get(node);

            // test
            Assert.IsTrue(string.IsNullOrEmpty(test.Name));

            // clean       
        }

        #endregion

    }
}