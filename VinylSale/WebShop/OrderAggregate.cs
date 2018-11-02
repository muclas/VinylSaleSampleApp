using CQRSHelper;
using Events.WebShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VinylSale.WebShop
{
    public class OrderAggregate : Aggregate,
        IHandleCommand<OpenCart>,
        IHandleCommand<AddItemToCart>,
        IHandleCommand<ModifyItemInCart>,
        IHandleCommand<RemoveItemFromCart>,
        IHandleCommand<CloseCart>,
        IApplyEvent<CartOpened>,
        IApplyEvent<ItemAddedToCart>,
        IApplyEvent<ItemInCartModified>,
        IApplyEvent<ItemRemovedFromCart>,
        IApplyEvent<CartClosed>
    {
        private bool open;
        private List<ItemInCart> itemsInCart = new List<ItemInCart>();

        public IEnumerable Handle(OpenCart c)
        {
            yield return new CartOpened
            {
                Id = c.Id,
                Customer = c.Customer,
            };
        }

        public IEnumerable Handle(AddItemToCart c)
        {
            if (!open)
            {
                throw new CartNotOpen();
            }

            yield return new ItemAddedToCart
            {
                Id = c.Id,
                Item = c.Item,
            };
        }

        public IEnumerable Handle(ModifyItemInCart c)
        {
            if (!open)
            {
                throw new CartNotOpen();
            }

            if (!itemsInCart.Any(i => i.ItemNumber == c.ItemNumber))
            {
                throw new ItemNotInCart();
            }

            yield return new ItemInCartModified
            {
                Id = c.Id,
                ItemNumber = c.ItemNumber,
                Count = c.Count,
            };
        }

        public IEnumerable Handle(RemoveItemFromCart c)
        {
            if (!open)
            {
                throw new CartNotOpen();
            }

            if (!itemsInCart.Any(i => i.ItemNumber == c.ItemNumber))
            {
                throw new ItemNotInCart();
            }

            yield return new ItemRemovedFromCart
            {
                Id = c.Id,
                ItemNumber = c.ItemNumber,
            };
        }

        public IEnumerable Handle(CloseCart c)
        {
            if (!open)
            {
                throw new CartNotOpen();
            }

            yield return new CartClosed
            {
                Id = c.Id,
                TotalAmount = c.TotalAmount,
            };
        }

        public void Apply(CartOpened e)
        {
            open = true;
        }

        public void Apply(ItemAddedToCart e)
        {
            var item = itemsInCart.FirstOrDefault(i => i.ItemNumber == e.Item.ItemNumber);
            if (item != null)
            {
                item.Count++;
            }
            else
            {
                itemsInCart.Add(new ItemInCart
                {
                    Artist = e.Item.Artist,
                    Count = 1,
                    ItemNumber = e.Item.ItemNumber,
                    ReleasedYear = e.Item.ReleasedYear,
                    Title = e.Item.Title,
                });
            }
        }

        public void Apply(ItemInCartModified e)
        {
            var item = itemsInCart.First(i => i.ItemNumber == e.ItemNumber);
            item.Count = e.Count;
        }

        public void Apply(ItemRemovedFromCart e)
        {
            var item = itemsInCart.First(i => i.ItemNumber == e.ItemNumber);
            itemsInCart.Remove(item);
        }

        public void Apply(CartClosed e)
        {
            open = false;
        }
    }
}
