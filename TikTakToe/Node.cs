using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe
{
    public class Node<T> where T : IGameState<T>
    {
        public T State { get; set; }
        public List<Node<T>> Children { get; set; }
        public Node()
        {
        }

        public void CreateTree(T startingState)
        {
            State = startingState; 
            if(startingState == null)
            {
                throw new Exception("Null startingState");
            }

            BuildTree();
            //SetValues(Players.Player1, Players.Player2);
        }

        private void BuildTree()
        {
            Children = State.GetChildren();
            if (!State.IsTerminal)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].BuildTree();
                }
            }
        }

    }
}
