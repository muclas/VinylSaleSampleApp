using System;
using System.Collections.Generic;
using System.Text;

namespace Events.WebShop
{
    public class ItemAddedToCart
    {
        public Guid Id;
        public OrderedItem Item;
    }
}
