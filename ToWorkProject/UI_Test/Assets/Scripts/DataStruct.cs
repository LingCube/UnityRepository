using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStruct
{

    public class Swap<T>
    {
        public static void swap(ref T x, ref T y)
        {
            T tmp = x;
            x = y;
            y = tmp;
        }
    }

    public class Pair<X, Y>
    {

        public X x;
        public Y y;
        public Pair(X _x, Y _y)
        {
            x = _x;
            y = _y;
        }

        public override string ToString()
        {
            return '(' + x.ToString() + ',' + y.ToString() + ')';
        }
    }

    public class ListNode<T>
    {
        public T val = default;

        public ListNode<T> last = null;

        public ListNode<T> next = null;

        public ListNode() { }

        public ListNode(T x) => val = x;

    }

    public class Heap<T>
    {

        public Heap(Cmp<T> _cmp, bool type = true)
        {
            _v = type;
            cmp = _cmp;
        }

        public int size() => _size;

        public bool empty() => _size == 0;

        public T top() => v[0];

        public void push(T x)
        {
            v.Add(x);
            int idx = v.Count - 1, tmpidx = ((idx + 1) >> 1) - 1;
            while (tmpidx >= 0 && ((_v && cmp(v[tmpidx], v[idx]) < 0) || (!_v && cmp(v[tmpidx], v[idx]) > 0)))
            {
                vswap(tmpidx, idx);
                idx = tmpidx;
                tmpidx = ((idx + 1) >> 1) - 1;
            }
            _size++;
        }

        public void pop()
        {
            vswap(0, v.Count - 1);
            v.RemoveAt(v.Count - 1);
            int idx = 0, tmpidx = (idx << 1) + 1;
            while (tmpidx < v.Count)
            {
                int _tmpidx = tmpidx < v.Count - 1 && ((_v && cmp(v[tmpidx + 1], v[tmpidx]) > 0)
                    || (!_v && cmp(v[tmpidx + 1], v[tmpidx]) < 0)) ? tmpidx + 1 : tmpidx;
                if ((_v && cmp(v[_tmpidx], v[idx]) > 0) || (!_v && cmp(v[_tmpidx], v[idx]) < 0))
                {
                    vswap(_tmpidx, idx);
                    idx = _tmpidx;
                    tmpidx = (idx << 1) + 1;
                }
                else break;
            }
            _size--;
        }

        public bool _v;

        private int _size = 0;

        private List<T> v = new List<T>();

        private Cmp<T> cmp;

        private void vswap(int i, int j)
        {
            T tmp = v[i];
            v[i] = v[j];
            v[j] = tmp;
        }

    }

    public class Deque<T>
    {

        public Deque()
        {

        }

        public virtual int size() => _size;

        public virtual bool empty() => _size == 0;

        public virtual void clear()
        {
            l = r = null;
            _size = 0;
        }

        public virtual T front() => l.val;

        public virtual T back() => r.val;

        public virtual void push_front(T x)
        {
            if (_size == 0)
            {
                l = r = new ListNode<T>(x);
            }
            else
            {
                l.last = new ListNode<T>(x);
                l.last.next = l;
                l = l.last;
            }
            _size++;
        }

        public virtual void push_back(T x)
        {
            if (_size == 0)
            {
                l = r = new ListNode<T>(x);
            }
            else
            {
                r.next = new ListNode<T>(x);
                r.next.last = r;
                r = r.next;
            }
            _size++;
        }

        public virtual void pop_front()
        {
            l = l.next;
            if (l == r) r.last = l.last = null;
            _size--;
        }

        public virtual void pop_back()
        {
            r = r.last;
            if (l == r) l.next = r.next = null;
            _size--;
        }

        protected int _size = 0;

        protected ListNode<T> l = null;

        protected ListNode<T> r = null;

    }


    public delegate int Cmp<T>(T x, T y);


}