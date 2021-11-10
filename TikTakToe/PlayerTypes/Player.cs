using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.PlayerTypes
{
    public abstract class Player
    {
        public Players PlayerID { get; set; }
        public Player(Players playerID)
        {
            PlayerID = playerID;
        }

        public abstract (int y, int x) SelectTile(Board currentGame);
    }
}
