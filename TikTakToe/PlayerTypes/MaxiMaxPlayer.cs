using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    class MaxiMaxPlayer : Player
    {
        private Random random;
        public MaxiMaxPlayer(Players playerID, Random random)
            : base(playerID)
        {
            this.random = random;
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            int[] values = new int[currentTree.Children.Count];
            int smallestValue = int.MaxValue;
            int largestValue = int.MinValue;
            (int, int) minimizerMove = (0, 0);
            (int, int) maximizerMove = (0, 0);
            int start = random.Next(0, 9);
            for (int i = 0; i < 9; i++)
            {
                if (start == 9)
                {
                    start = 0;
                }
                int y = start / 3;
                int x = start % 3;
                if (currentTree.State[y][x] == Players.None)
                {
                    minimizerMove = (y, x);
                    maximizerMove = (y, x);
                    break;
                }
                start++;
            }

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = currentTree.Children[i].Value;
                if (values[i] < smallestValue)
                {
                    smallestValue = values[i];
                    minimizerMove = currentTree.State.FindDifference(currentTree.Children[i].State);
                }
                if (values[i] > largestValue)
                {
                    largestValue = values[i];
                    maximizerMove = currentTree.State.FindDifference(currentTree.Children[i].State);
                }
            }

            if (largestValue == currentTree.Value)
            {
                return maximizerMove;
            }
            else
            {
                return minimizerMove;
            }

        }
    }
}
