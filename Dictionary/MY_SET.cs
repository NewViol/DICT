using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public class MySet<TVal> : BaseTree<TVal> where TVal : IComparable<TVal>
    {
        public MySet() : base(true)
        {
        }
    }

    public class MyMultySet<TVal> : BaseTree<TVal> where TVal : IComparable<TVal>
    {
        public MyMultySet() : base(false)
        {
        }
    }
}
