using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using TikTakToe.DrawStuff;

namespace TikTakToe.GBVPlayerTypes
{

    public abstract class GBVPlayer
    {
        public Players PlayerID { get; set; }
        public GBVPlayer(Players playerID)
        {
            PlayerID = playerID;
        }

        public Dictionary<int, Dictionary<Players, int>> GetPlayerValue { get; set; }
        public abstract (int y, int x) SelectTile(Node<Board> CurrentTree);
    }
}
