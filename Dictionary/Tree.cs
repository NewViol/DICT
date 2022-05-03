using System.Collections;
using System.Collections.Generic;

namespace Dictionary
{
    public class Tree<T, Comparer> : ICollection<T> where Comparer : IComparer<T>, new()
    {
        Comparer _comparer = new Comparer();
        protected class TreeItem
        {
            public T _value { get; set; }
            public TreeItem _parent { get; set; }
            public TreeItem _left { get; set; }
            public TreeItem _right { get; set; }
            public TreeItem(T value, TreeItem parent, TreeItem left = null, TreeItem right = null)
            {
                _value = value;
                _parent = parent;
                _left = left;
                _right = right;
            }
        }

        protected TreeItem _root = null;
        int _count = 0;
        protected bool _isUnique;

        public Tree(bool isUnique)
        {
            _isUnique = isUnique;
        }

        public bool IsReadOnly => false;
        public int Count => _count;
        public void Add(T value)
        {
            if (_root == null)
            {
                _root = new TreeItem(value, null);
                ++_count;
            }
            else
            {
                Add(value, _root);
            }
        }

        private void Add(T value, TreeItem item)
        {
            if (_isUnique && _comparer.Compare(value, item._value) == 0)
            {
                item._value = value;
            }
            else if (_comparer.Compare(value, item._value) < 0)
            {
                if (item._left == null)
                {
                    item._left = new TreeItem(value, item);
                    ++_count;
                }
                else
                {
                    Add(value, item._left);
                }
            }
            else
            {
                if (item._right == null)
                {
                    item._right = new TreeItem(value, item);
                    ++_count;
                }
                else
                {
                    Add(value, item._right);
                }
            }
        }

        protected TreeItem GetItem(T value)
        {
            return GetItem(value, _root);
        }

        public void Clear()
        {
            _root = null;
            _count = 0;
        }

        public bool Contains(T value)
        {
            using (IEnumerator<T> en = GetEnumerator())
            {
                while (en.MoveNext())
                {
                    if (_comparer.Compare(en.Current, value) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (IEnumerator<T> en = GetEnumerator())
            {
                while (en.MoveNext())
                {
                    array[arrayIndex++] = en.Current;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            Stack<TreeItem> stack = new Stack<TreeItem>();
            TreeItem item = _root;
            while (item != null || stack.Count != 0)
            {
                if (item != null)
                {
                    stack.Push(item);
                    item = item._left;
                }
                else
                {
                    item = stack.Pop();
                    yield return item._value;
                    item = item._right;
                }
            }
        }

        public bool Remove(T value)
        {
            TreeItem item = GetItem(value, _root);
            if (item == null)
            {
                return false;
            }
            RemoveItem(item);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected TreeItem GetItem(T value, TreeItem item)
        {
            if (item == null || _comparer.Compare(value, item._value) == 0)
            {
                return item;
            }
            else if (_comparer.Compare(value, item._value) < 0)
            {
                return GetItem(value, item._left);
            }
            else
            {
                return GetItem(value, item._right);
            }
        }
        private void RemoveItem(TreeItem item)
        {
            if (item._left == null && item._right == null)
            {
                RemoveItemWithoutChildren(item);
            }
            else if (item._left != null && item._right != null)
            {
                RemoveItemWithBothChildren(item);
            }
            else
            {
                RemoveItemWithOneChild(item);
            }
            --_count;
        }

        private void RemoveItemWithOneChild(TreeItem item)
        {
            TreeItem child = item._left == null ? item._right : item._left;

            if (item == _root)//special case: item is root
            {
                _root = child;
                _root._parent = null;
            }
            else
            {
                if (item._parent._left == item)
                {
                    item._parent._left = child;
                }
                else
                {
                    item._parent._right = child;
                }
                child._parent = item._parent;
            }
        }

        private void RemoveItemWithBothChildren(TreeItem item)
        {
            TreeItem candidate = item._right;
            while (candidate._left != null)
            {
                candidate = candidate._left;
            }
            item._value = candidate._value;

            if (candidate._parent._left == candidate)
            {
                candidate._parent._left = candidate._right;
            }
            else
            {
                candidate._parent._right = candidate._right;
            }
            if (candidate._right != null)
            {
                candidate._right._parent = candidate._parent;
            }
        }

        private void RemoveItemWithoutChildren(TreeItem item)
        {
            if (item == _root)//special case: item is root
            {
                _root = null;
            }
            else
            {
                if (item._parent._left == item)//item is left child
                {
                    item._parent._left = null;
                }
                else
                {
                    item._parent._right = null;
                }
            }
        }
    }
}