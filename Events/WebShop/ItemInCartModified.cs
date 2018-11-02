using System;
using System.Collections.Generic;
using System.Text;

namespace Events.WebShop
{
    public class ItemInCartModified
    {
        public Guid Id;
        public int ItemNumber;
        public int Count;
    }
}
