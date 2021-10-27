using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public static class Extensions
    {
        public static Texture2D CreatePixel(this Color color, GraphicsDeviceManager graphics)
        {
            Texture2D returnVal = new Texture2D(graphics.GraphicsDevice, 1, 1);
            returnVal.SetData(new Color[] { color}, 0, 1);
            return returnVal;
        }
    }

}
