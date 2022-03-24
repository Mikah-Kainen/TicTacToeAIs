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

        public void SetValues(GridBoard currentBoard)
        {
            var board = currentBoard.CurrentBoard;
            int currentBoardValue = board.Print();
            if (!GetPlayerValue.ContainsKey(currentBoardValue))
            {
                GetPlayerValue.Add(currentBoardValue, new Dictionary<Players, int>());
                if (currentBoard.IsTerminal)
                {
                    foreach (Players player in activePlayers)
                    {
                        Players winner = currentBoard.GetWinner();
                        if (winner == player)
                        {
                            GetPlayerValue[currentBoardValue].Add(player, 1);
                        }
                        else if (winner != player && winner != Players.None)
                        {
                            GetPlayerValue[currentBoardValue].Add(player, -1);
                        }
                        else
                        {
                            GetPlayerValue[currentBoardValue].Add(player, 0);
                        }
                    }
                }
                else
                {
                    int largestValue = int.MinValue;
                    GridBoard largestValueMove = null;
                    for (int i = 0; i < currentBoard.GetChildren().Count; i++)
                    {
                        SetValues((GridBoard)currentBoard.GetChildren()[i]);
                        int temp = GetPlayerValue[currentBoard.GetChildren()[i].CurrentBoard.Print()][currentBoard.NextPlayer];
                        if (temp > largestValue)
                        {
                            largestValue = temp;
                            largestValueMove = (GridBoard)currentBoard.GetChildren()[i];
                        }
                    }
                    foreach (Players player in activePlayers)
                    {
                        if (player == currentBoard.NextPlayer)
                        {
                            GetPlayerValue[currentBoardValue].Add(player, largestValue);
                        }
                        else
                        {
                            GetPlayerValue[currentBoardValue].Add(player, GetPlayerValue[largestValueMove.CurrentBoard.Print()][player]);
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
                int superTemp = children[i].CurrentBoard.Print();
                bool isLose = children[i].GetWinner() != children[i].NextPlayer && children[i].GetWinner() != Players.None;
                if (!isLose)
                {
                    int temp = GetPlayerValue[superTemp][currentTree.NextPlayer];
                    if (temp > largestValue)
                    {
                        largestValue = temp;
                        maximizerMove = currentTree.CurrentBoard.FindDifference(children[i].CurrentBoard);
                    }
                }
            }

            return maximizerMove;
        }
    }
}
