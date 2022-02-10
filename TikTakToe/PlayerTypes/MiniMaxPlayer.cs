using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace TikTakToe.PlayerTypes
{
    public class MiniMaxPlayer : Player, IMiniMaxPlayer
    {
        private Random random;
        private Players opponentID;
        private Players maximizer => PlayerID;
        private Players minimizer => opponentID;
        public MiniMaxPlayer(Players playerID, Players opponentID, Random random)
            : base(playerID)
        {
            this.random = random;
            this.opponentID = opponentID;
        }

        public void SetValues(Node<Board> currentTree)
        {
            Board State = currentTree.State;
            int currentTreeValue = currentTree.State.Print();
            GetPlayerValue.Add(currentTree.State.Print(), new Dictionary<Players, int>());
            if (State.IsTerminal)
            {
                Players winner = State.GetWinner();
                if (winner == maximizer)
                {
                    GetPlayerValue[currentTreeValue].Add(maximizer, 1);
                    GetPlayerValue[currentTreeValue].Add(minimizer, 1);
                    return;
                }
                else if (winner == minimizer)
                {
                    GetPlayerValue[currentTreeValue].Add(maximizer, -1);
                    GetPlayerValue[currentTreeValue].Add(minimizer, -1);
                    return;
                }
                else
                {
                    GetPlayerValue[currentTreeValue].Add(maximizer, 0);
                    GetPlayerValue[currentTreeValue].Add(minimizer, 0);
                    return;
                }
            }
            int smallestValue = int.MaxValue;
            int largestValue = int.MinValue;
            for (int i = 0; i < currentTree.Children.Count; i++)
            {
                SetValues(currentTree.Children[i]);
                int temp = GetPlayerValue[currentTree.Children[i].State.Print()][currentTree.State.NextPlayer];
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
                GetPlayerValue[currentTreeValue].Add(maximizer, largestValue);
                GetPlayerValue[currentTreeValue].Add(minimizer, largestValue);
                return;
            }
            else
            {
                GetPlayerValue[currentTreeValue].Add(maximizer, smallestValue);
                GetPlayerValue[currentTreeValue].Add(minimizer, smallestValue);
                return;
            }
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            int smallestValue = int.MaxValue;
            int largestValue = int.MinValue;
            (int, int) minimizerMove = currentTree.RandomMove(random);
            (int, int) maximizerMove = currentTree.RandomMove(random);

            for (int i = 0; i < currentTree.Children.Count; i ++)
            {
                int temp = GetPlayerValue[currentTree.Children[i].State.Print()][currentTree.Children[i].State.NextPlayer];
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

            if(largestValue == GetPlayerValue[currentTree.State.Print()][currentTree.State.NextPlayer])
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
