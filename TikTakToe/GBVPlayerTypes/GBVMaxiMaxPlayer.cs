using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.GBVPlayerTypes
{
    class GBVMaxiMaxPlayer : GBVPlayer
    {
        private Random random;
        private List<Players> activePlayers;
        public GBVMaxiMaxPlayer(Players playerID, List<Players> activePlayers, Random random)
            : base(playerID)
        {
            this.random = random;
            this.activePlayers = activePlayers;
        }

        public void SetValues(GridBoard currentTree)
        {
            var board = currentTree.CurrentBoard;
            int currentTreeValue = board.Print();
            if (!GetPlayerValue.ContainsKey(currentTreeValue))
            {
                GetPlayerValue.Add(currentTreeValue, new Dictionary<Players, int>());
                if (currentTree.IsTerminal)
                {
                    foreach (Players player in activePlayers)
                    {
                        Players winner = currentTree.GetWinner();
                        if (winner == player)
                        {
                            GetPlayerValue[currentTreeValue].Add(player, 1);
                        }
                        else if (winner != player && winner != Players.None)
                        {
                            GetPlayerValue[currentTreeValue].Add(player, -1);
                        }
                        else
                        {
                            GetPlayerValue[currentTreeValue].Add(player, 0);
                        }
                    }
                }
                else
                {
                    int largestValue = int.MinValue;
                    GridBoard largestValueMove = null;
                    for (int i = 0; i < currentTree.GetChildren().Count; i++)
                    {
                        SetValues((GridBoard)currentTree.GetChildren()[i]);
                        int temp = GetPlayerValue[currentTree.GetChildren()[i].CurrentBoard.Print()][currentTree.NextPlayer];
                        if (temp > largestValue)
                        {
                            largestValue = temp;
                            largestValueMove = (GridBoard)currentTree.GetChildren()[i];
                        }
                    }
                    foreach (Players player in activePlayers)
                    {
                        if (player == currentTree.NextPlayer)
                        {
                            GetPlayerValue[currentTreeValue].Add(player, largestValue);
                        }
                        else
                        {
                            GetPlayerValue[currentTreeValue].Add(player, GetPlayerValue[largestValueMove.CurrentBoard.Print()][player]);
                        }
                    }
                }
            }
        }

        public override (int y, int x) SelectTile(GridBoard currentTree)
        {
            (int, int) maximizerMove = currentTree.RandomMove(random);
            int largestValue = int.MinValue;

            var children = currentTree.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                //8790, 141396
                int temp = GetPlayerValue[children[i].CurrentBoard.Print()][currentTree.NextPlayer];
                if (temp > largestValue)
                {
                    largestValue = temp;
                    maximizerMove = currentTree.CurrentBoard.FindDifference(children[i].CurrentBoard);
                }
            }

            return maximizerMove;
        }
    }
}
