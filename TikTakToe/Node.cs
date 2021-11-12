using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe
{
    public class Node<T> where T : IGameState<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Children 
        { 
            get
            {
                if(Value != null && children == null)
                {
                    children = new List<Node<T>>();
                    T[] values = Value.GetChildren();
                    for (int i = 0; i < values.Length; i++)
                    {
                        Node<T> child = new Node<T>();
                        child.Value = values[i];
                        var buildChildren = child.Children;
                        children.Add(child);
                    }
                }
                return children;
            }
        }
        
        private List<Node<T>> children;

        public Node()
        {
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
