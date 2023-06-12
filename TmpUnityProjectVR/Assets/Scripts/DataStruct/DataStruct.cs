using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStruct
{

    public class Pair<X, Y>
    {
        public X x;
        public Y y;
        public Pair(X _x, Y _y)
        {
            x = _x;
            y = _y;
        }
    }

    public class ListNode<T>
    {
        public T val = default;

        public ListNode<T> next = null;

        public ListNode()
        {

        }

        public ListNode(T x) => val = x;

    }

    public class Dij<T>
    {

        public void build(T x, T y, int len)
        {
            ei++;
            e[ei].r = x;
            e[ei].ne = h[dic[x]];
            e[ei].len = len;
            h[dic[x]] = ei;
        }

        public void dij(T x)
        {
            List<int> d = new List<int>(M);
            bool[] st = new bool[N];
            d[dic[x]] = 0;
            Queue<Pair<int, T>> q = new Queue<Pair<int, T>>();
            q.Enqueue(new Pair<int, T>(0, x));
            while(q.Count > 0)
            {
                int dis = q.Peek().x, idx = dic[q.Peek().y];
                q.Dequeue();
                if (st[idx]) continue;
                st[idx] = true;
                for (int  i = h[idx]; i > 0; i = e[i].ne)
                {
                    T j = e[i].r;
                    if (d[dic[j]] > dis + e[i].len)
                    {
                        q.Enqueue(new Pair<int, T>(d[dic[j]], j));
                    }
                }
            }
        }

        private const int N = 200005, M = 1000005, INF = int.MaxValue;

        Dictionary<T, int> dic = new Dictionary<T, int>();

        private class edge
        {
            public T r;
            public int ne, len = INF;
            public edge(T _r, int _ne, int _len)
            {
                r = _r;
                ne = _ne;
                len = _len; 
            }
        }

        private int ei = 0;

        private List<int> h = new List<int>(N);

        private List<edge> e = new List<edge>(M);

    }

    public class TreeNode<T>
    {
        public T val;
        public TreeNode<T> left, right;

        public TreeNode(T x) => val = x;

        private Dictionary<T, int> predic, posdic = new Dictionary<T, int>();

        public TreeNode<T> pre_ino_build(int i, int l, int r, List<T> pre)
        {
            if (l >= r) return null;
            TreeNode<T> res = new TreeNode<T>(pre[i]);
            res.left = pre_ino_build(i + 1, l, predic[pre[i]], pre);
            res.right = pre_ino_build(i + 1 + predic[pre[i]] - l, predic[pre[i]] + 1, r, pre);
            return res;
        }

        public TreeNode<T> pos_ino_build(int i, int l, int r, List<T> pos)
        {
            if (l >= r) return null;
            TreeNode<T> res = new TreeNode<T>(pos[i]);
            res.left = pos_ino_build(i - r + posdic[pos[i]], l, posdic[pos[i]], pos);
            res.right = pos_ino_build(i - 1, posdic[pos[i]] + 1, r, pos);
            return res;
        }

        public void DicOnEnable(List<T> list, bool isv)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (isv) predic[list[i]] = i;
                else posdic[list[i]] = i;
            }
        }

        public void dfs(TreeNode<T> root, int type)
        {
            if (root == null) return;
            if (type == 1)
            {
                Debug.Log(root.val);
            }
            dfs(root.left, type);
            if (type == 2)
            {
                Debug.Log(root.val);
            }
            dfs(root.right, type);
            if (type == 3)
            {
                Debug.Log(root.val);
            }
        }
    }
}