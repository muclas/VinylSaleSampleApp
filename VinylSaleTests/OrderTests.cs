using CQRSHelper;
using System;
using VinylSale.WebShop;
using NUnit.Framework;
using Events.WebShop;

namespace VinylSaleTests
{
    [TestFixture]
    public class OrderTests : BDDTest<OrderAggregate>
    {
        private Guid testId;
        private string testCustomer;

        [SetUp]
        public void Setup()
        {
            testId = Guid.NewGuid();
            testCustomer = "John";
        }

        [Test]
        public void CanOpenACart()
        {
            Test(
                Given(),
                When(new OpenCart
                {
                    Id = testId,
                    Customer = testCustomer,
                }),
                Then(new CartOpened
                {
                    Id = testId,
                    Customer = testCustomer,
                }));
        }
    }
}
