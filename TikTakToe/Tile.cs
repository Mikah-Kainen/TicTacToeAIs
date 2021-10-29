using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe
{
    public class Tile : Sprite
    {
        public Tile(Texture2D tex, Color tint, Vector2 pos, Vector2 size, Vector2 scale, Vector2 origin)
            :base(tex, tint, pos, size, scale, origin)
        {

        }
    }
}
