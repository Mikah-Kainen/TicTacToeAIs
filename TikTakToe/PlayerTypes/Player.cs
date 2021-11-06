using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.PlayerTypes
{
    public abstract class Player
    {
        public Color PlayerColor { get; set; }
        public Player(Color playerColor)
        {
            PlayerColor = playerColor;
        }

        public abstract Sprite SelectTile(Sprite[][] currentGame);
    }
}
