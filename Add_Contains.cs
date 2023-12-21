using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class TreeNode<T> where T : IComparable
    {
        public T Value;
        public TreeNode<T> Left, Right;
        public int quantity = 1;
        public IEnumerable<T> FindVS()
        {
            if (Left != null)
                foreach (var value in Left.FindVS())
                    yield return value;
            yield return Value;
            if (Right != null)
                foreach (var value in Right.FindVS())
                    yield return value;
        }

        public static TreeNode<T> Scan(TreeNode<T> root, T key)
        {
            if (root == null) 
                return null;
            if (key.Equals(root.Value)) 
                return root;
            if (key.CompareTo(root.Value) > 0)
            {
                return (Scan(root.Right, key));
            }
            else
            {
                return (Scan(root.Left, key));
            }
        }
    }
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private TreeNode<T> root;

        public T this[int x]
        {
            get
            {
                if (root == null) throw new Exception();
                if (root.quantity < x) throw new IndexOutOfRangeException();
                return FindVal(root, x);
            }
        }
        public void Add(T y)
        {
            if (root == null)
            {
                root = new TreeNode<T>
                {
                    Value = y
                };
                return;
            }
            Help(y);
        }
        private void Help(T value)
        {
            bool added = false;
            while (!added)
            {
                if (root != null)
                {
                    var now = root;
                    while (true)
                    {
                        now.quantity++;
                        if (now.Value.CompareTo(value) <= 0)
                        {
                            if (now.Right != null)
                            {
                                now = now.Right;

                            }
                            else
                            {
                                added = true;
                                now.Right = new TreeNode<T>
                                {
                                    Value = value
                                };
                                break;
                            }
                        }
                        else
                        {
                            if (now.Left != null)
                            {
                                now = now.Left;
                            }
                            else
                            {
                                now.Left = new TreeNode<T>
                                {
                                    Value = value
                                };
                                added = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public bool Contains(T y)
        {
            return TreeNode<T>.Scan(root, y) != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public virtual IEnumerator<T> GetEnumerator()
        {
            if (root == null)
            {
                LinkedList<T> now = new LinkedList<T>();
                return now.GetEnumerator();
            }
            return root.FindVS().GetEnumerator();
        }

        public T FindVal(TreeNode<T> root, int x)
        {
            if (root.Left == null)
            {
                if (x == 0) 
                    return root.Value;
                else 
                    return FindVal(root.Right, x - 1);
            }
            else if (root.Left.quantity > x) 
                return FindVal(root.Left, x);
            else if (root.Left.quantity - x == 0) 
                return root.Value;
            else return FindVal(root.Right, x - root.Left.quantity - 1);
        }
    }
}


