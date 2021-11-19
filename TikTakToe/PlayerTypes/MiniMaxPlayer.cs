using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    public class MiniMaxPlayer : Player
    {
        private Random random;
        public MiniMaxPlayer(Players playerID, Random random)
            : base(playerID)
        {
            this.random = random;
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            int[] values = new int[currentTree.Children.Count];
            int smallestValue = int.MaxValue;
            int largestValue = 0;
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
                if (currentTree.Value[y][x] == Players.None)
                {
                    minimizerMove = (y, x);
                    maximizerMove = (y, x);
                    break;
                }
                start++;
            }

            Players maximizer = Players.Player1;
            Players minimizer = Players.Player2;
            for (int i = 0; i < values.Length; i ++)
            {
                values[i] = GetValue(currentTree.Children[i], maximizer, minimizer);
                if (values[i] < smallestValue)
                {
                    smallestValue = values[i];
                    minimizerMove = currentTree.Value.FindDifference(currentTree.Children[i].Value);
                }
                if (values[i] > largestValue)
                {
                    largestValue = values[i];
                    maximizerMove = currentTree.Value.FindDifference(currentTree.Children[i].Value);
                }
            }

            if(currentTree.Value.NextPlayer == maximizer)
            {
                return maximizerMove;
            }
            else
            {
                return minimizerMove;
            }

        }

        public int GetValue(Node<Board> Current, Players maximizer, Players minimizer)
        {
            if(Current.Value.IsTerminal)
            {
                Players winner = Current.Value.GetWinner();
                if(winner == maximizer)
                {
                    return 1;
                }
                else if(winner == minimizer)
                {
                    return -1;
                }
                else
                {
                    return 0;
                } 
            }
            int[] values = new int[Current.Children.Count];
            int smallestValue = int.MaxValue;
            int largestValue = 0;
            for(int i = 0; i < Current.Children.Count; i ++)
            {
                values[i] = GetValue(Current.Children[i], maximizer, minimizer);
                if(values[i] < smallestValue)
                {
                    smallestValue = values[i];
                }
                if(values[i] > largestValue)
                {
                    largestValue = values[i];
                }
            }
            if(Current.Value.NextPlayer == maximizer)
            {
                return largestValue;
            }
            else
            {
                return smallestValue;
            }
        }
    }
}
