using System;
using System.Collections.Generic;
using System.Text;

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
            GetPlayerValue.Add(currentTree, new Dictionary<Players, int>());
            if (State.IsTerminal)
            {
                foreach (Players player in activePlayers)
                {
                    Players winner = State.GetWinner();
                    if (winner == player)
                    {
                        GetPlayerValue[currentTree].Add(player, 1);
                    }
                    else if (winner != player && winner != Players.None)
                    {
                        GetPlayerValue[currentTree].Add(player, -1);
                    }
                    else
                    {
                        GetPlayerValue[currentTree].Add(player, 0);
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
                    int temp = GetPlayerValue[currentTree.Children[i]][currentTree.State.NextPlayer];
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
                        GetPlayerValue[currentTree].Add(player, largestValue);
                    }
                    else
                    {
                        GetPlayerValue[currentTree].Add(player, GetPlayerValue[largestValueMove][player]);
                    }
                }
            }
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            int largestValue = int.MinValue;
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
                    maximizerMove = (y, x);
                    break;
                }
                start++;
            }

            for (int i = 0; i < currentTree.Children.Count; i++)
            {
                int temp = GetPlayerValue[currentTree.Children[i]][currentTree.State.NextPlayer];
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
