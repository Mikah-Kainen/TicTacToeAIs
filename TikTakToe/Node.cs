using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe
{
    public class Node<T> where T : IGameState<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Children { get; set; }
        public Node()
        {
        }

        public void BuildTree()
        {
            if (Value == null) return;
            Children = new List<Node<T>>();
            T[] values = Value.GetChildren();
            for (int i = 0; i < values.Length; i++)
            {
                Node<T> Child = new Node<T>();
                Child.Value = values[i];
                Child.BuildTree();
                Children.Add(Child);
            }
        }

        //public void SetChildren(T[] values)
        //{
        //    Children = new List<Node<T>>();
        //    for (int i = 0; i < values.Length; i ++)
        //    {
        //        Node<T> child = new Node<T>();
        //        child.Value = values[i];
        //        Children.Add(child);
        //    }
        //}
    }
}
