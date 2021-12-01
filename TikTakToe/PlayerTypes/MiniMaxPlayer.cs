using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    public class MiniMaxPlayer : Player
    {
        private Random random;
        private Players opponentID;
        public MiniMaxPlayer(Players playerID, Players opponentID, Random random)
            : base(playerID)
        {
            this.random = random;
            this.opponentID = opponentID;
        }

        private int SetValues(Node<Board> currentTree, Players maximizer, Players minimizer)
        {
            Board State = currentTree.State;
            GetPlayerValue.Add(currentTree, new Dictionary<Players, int>());
            if (State.IsTerminal)
            {
                Players winner = State.GetWinner();
                if (winner == maximizer)
                {
                    GetPlayerValue[currentTree].Add(maximizer, 1);
                    GetPlayerValue[currentTree].Add(minimizer, 1);
                    return 1;
                }
                else if (winner == minimizer)
                {
                    GetPlayerValue[currentTree].Add(maximizer, -1);
                    GetPlayerValue[currentTree].Add(minimizer, -1);
                    return -1;
                }
                else
                {
                    GetPlayerValue[currentTree].Add(maximizer, 0);
                    GetPlayerValue[currentTree].Add(minimizer, 0);
                    return 0;
                }
            }
            int smallestValue = int.MaxValue;
            int largestValue = int.MinValue;
            for (int i = 0; i < currentTree.Children.Count; i++)
            {
                int temp = SetValues(currentTree.Children[i], maximizer, minimizer);
                if (temp < smallestValue)
                {
                    smallestValue = temp;
                }
                if (temp > largestValue)
                {
                    largestValue = temp;
                }
            }
            if (State.NextPlayer == maximizer)
            {
                GetPlayerValue[currentTree].Add(maximizer, largestValue);
                GetPlayerValue[currentTree].Add(minimizer, largestValue);
                return largestValue;
            }
            else
            {
                GetPlayerValue[currentTree].Add(maximizer, smallestValue);
                GetPlayerValue[currentTree].Add(minimizer, smallestValue);
                return smallestValue;
            }
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            if(GetPlayerValue == null)
            {
                GetPlayerValue = new Dictionary<Node<Board>, Dictionary<Players, int>>();
                SetValues(currentTree, PlayerID, opponentID);
            }
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

            for (int i = 0; i < currentTree.Children.Count; i ++)
            {
                int temp = GetPlayerValue[currentTree.Children[i]][currentTree.Children[i].State.NextPlayer];
                if (temp < smallestValue)
                {
                    smallestValue = temp;
                    minimizerMove = currentTree.State.FindDifference(currentTree.Children[i].State);
                }
                if (temp > largestValue)
                {
                    largestValue = temp;
                    maximizerMove = currentTree.State.FindDifference(currentTree.Children[i].State);
                }
            }

            if(largestValue == GetPlayerValue[currentTree][currentTree.State.NextPlayer])
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
