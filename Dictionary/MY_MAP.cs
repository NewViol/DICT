using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public class MyMap<TKey, TVal> : BaseDictionary<TKey, TVal> where TKey : IComparable<TKey>
    {
        public MyMap() : base(true)
        {
        }
    }

    public class MyMultyMap<TKey, TVal> : BaseDictionary<TKey, TVal> where TKey : IComparable<TKey>
    {
        public MyMultyMap() : base(false)
        {
        }
    }
}
