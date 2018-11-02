using CQRSHelper;
using System;
using VinylSale.WebShop;
using NUnit.Framework;
using Events.WebShop;
using System.Collections.Generic;

namespace VinylSaleTests
{
    [TestFixture]
    public class OrderTests : BDDTest<OrderAggregate>
    {
        private Guid testId;
        private string testCustomer;
        private OrderedItem testItem1, testItem2;

        [SetUp]
        public void Setup()
        {
            testId = Guid.NewGuid();
            testCustomer = "John";

            testItem1 = new OrderedItem
            {
                ItemNumber = 1,
                Artist = "Genesis",
                Title = "Trespass",
                ReleasedYear = 1970,
            };

            testItem2 = new OrderedItem
            {
                ItemNumber = 2,
                Artist = "Genesis",
                Title = "Trespass",
                ReleasedYear = 1970,
            };
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

        [Test]
        public void CannotAddItemToCartWithUnopenedCart()
        {
            Test(
                Given(),
                When(new AddItemToCart
                {
                    Id = testId,
                    Item = testItem1,
                }),
                ThenFailWith<CartNotOpen>());
        }

        [Test]
        public void CanPlaceOrderWithOneItem()
        {
            Test(
                Given(new CartOpened
                {
                    Id = testId,
                    Customer = testCustomer,
                }),
                When(new AddItemToCart
                {
                    Id = testId,
                    Item = testItem1,
                }),
                Then(new ItemAddedToCart
                {
                    Id = testId,
                    Item = testItem1,
                }));
        }

        [Test]
        public void CanRemoveAddedItem()
        {
            Test(
                Given(new CartOpened
                {
                    Id = testId,
                    Customer = testCustomer,
                },
                new ItemAddedToCart
                {
                    Id = testId,
                    Item = testItem1,
                }),
                When(new RemoveItemFromCart
                {
                    Id = testId,
                    ItemNumber = testItem1.ItemNumber,
                }),
                Then(new ItemRemovedFromCart
                {
                    Id = testId,
                    ItemNumber = testItem1.ItemNumber,
                }));
        }

        [Test]
        public void CannotRemoveNotAddedItem()
        {
            Test(
                Given(new CartOpened
                {
                    Id = testId,
                    Customer = testCustomer,
                },
                new ItemAddedToCart
                {
                    Id = testId,
                    Item = testItem1,
                }),
                When(new RemoveItemFromCart
                {
                    Id = testId,
                    ItemNumber = testItem2.ItemNumber,
                }),
                ThenFailWith<ItemNotInCart>()
                );
        }
    }
}
