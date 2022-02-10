using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace TikTakToe.PlayerTypes
{
    class MaxiMaxPlayer : Player, IMiniMaxPlayer
    {
        private Random random;
        private List<Players> activePlayers;
        public MaxiMaxPlayer(Players playerID, List<Players> activePlayers, Random random)
            : base(playerID)
        {
            this.random = random;
            this.activePlayers = activePlayers;
        }

        public void SetValues(Node<Board> currentTree)
        {
            Board State = currentTree.State;
            int currentTreeValue = currentTree.State.Print();
            if (!GetPlayerValue.ContainsKey(currentTreeValue))
            {
                GetPlayerValue.Add(currentTreeValue, new Dictionary<Players, int>());
                if (State.IsTerminal)
                {
                    foreach (Players player in activePlayers)
                    {
                        Players winner = State.GetWinner();
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
                    Node<Board> largestValueMove = null;
                    for (int i = 0; i < currentTree.Children.Count; i++)
                    {
                        SetValues(currentTree.Children[i]);
                        int temp = GetPlayerValue[currentTree.Children[i].State.Print()][currentTree.State.NextPlayer];
                        if (temp > largestValue)
                        {
                            largestValue = temp;
                            largestValueMove = currentTree.Children[i];
                        }
                    }
                    foreach (Players player in activePlayers)
                    {
                        if (player == currentTree.State.NextPlayer)
                        {
                            GetPlayerValue[currentTreeValue].Add(player, largestValue);
                        }
                        else
                        {
                            GetPlayerValue[currentTreeValue].Add(player, GetPlayerValue[largestValueMove.State.Print()][player]);
                        }
                    }
                }
            }
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            (int, int) maximizerMove = currentTree.RandomMove(random);
            int largestValue = int.MinValue;

            for (int i = 0; i < currentTree.Children.Count; i++)
            {
                int temp = GetPlayerValue[currentTree.Children[i].State.Print()][currentTree.State.NextPlayer];
                if (temp > largestValue)
                {
                    largestValue = temp;
                    maximizerMove = currentTree.State.FindDifference(currentTree.Children[i].State);
                }
            }

            return maximizerMove;
        }
    }
}
