using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQRSHelper
{
    public interface IHandleCommand<TCommand>
    {
        IEnumerable Handle(TCommand c);
    }
}
