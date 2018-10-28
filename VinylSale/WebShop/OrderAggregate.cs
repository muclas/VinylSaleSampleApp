using CQRSHelper;
using Events.WebShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VinylSale.WebShop
{
    public class OrderAggregate : Aggregate,
        IHandleCommand<OpenCart>
    {
        public IEnumerable Handle(OpenCart c)
        {
            yield return new CartOpened
            {
                Id = c.Id,
                Customer = c.Customer,
            };
        }
    }
}
