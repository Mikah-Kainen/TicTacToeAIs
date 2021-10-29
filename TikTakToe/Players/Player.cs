using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.Players
{
    public abstract class Player
    {
        public Color PlayerColor { get; private set; }
        public Player(Color playerColor)
        {
            PlayerColor = playerColor;
        }

        public abstract Sprite SelectTile(Sprite[][] currentGame);
    }
}
