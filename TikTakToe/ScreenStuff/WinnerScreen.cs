using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.ScreenStuff
{
    public class WinnerScreen : IScreen
    {
        public Color Winner { get; set; }
        public List<GameObject> Objects { get; set; }

        public WinnerScreen(Color winner, Rectangle screen)
        {
            Vector2 windowSize = new Vector2(400, 300);
            Vector2 pos = new Vector2((screen.Width - windowSize.X) / 2f, (screen.Height - windowSize.Y) / 2f);
            Objects.Add(new Sprite(Game1.WhitePixel, Color.White, pos, Vector2.One, windowSize, Vector2.Zero));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
