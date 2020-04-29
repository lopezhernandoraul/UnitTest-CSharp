using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    class ProductTests
    {
        [Test]

        // Example 1 - Test with UnitTest

        //Whenever we can, we will use example1 since it allows us to read more easily and in fewer lines.
        public void GetPrice_GoldCustomer_Apply30PercentDiscount()
        {
            var product = new Product{ ListPrice = 100 };
            var result = product.GetPrice(new Customer { IsGold = true });

            Assert.That(result,Is.EqualTo(70));
        }

        // Example 2 - Test with Mock Test (The same test as the one above)
        public void GetPrice_GoldCustomer_Apply30PercentDiscount2()
        {
            //
            //We have to create a simulated object with "Mock" because we derive from an interface
            var customer = new Mock<ICustomer>();
            customer.Setup(c => c.IsGold).Returns(true);
            var product = new Product { ListPrice = 100 };
             var result = product.GetPrice(new Customer { IsGold = true });
            Assert.That(result, Is.EqualTo(70));
        }
    }
}
