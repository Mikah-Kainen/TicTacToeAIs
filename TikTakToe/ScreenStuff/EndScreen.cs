using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.ScreenStuff
{
    public class EndScreen : IScreen
    {
        public Color Winner { get; set; }
        public List<GameObject> Objects { get; set; }

        public EndScreen(Color winner, Rectangle screen)
        {
            Vector2 windowSize = new Vector2(400, 300);
            Vector2 pos = new Vector2((screen.Width - windowSize.X) / 2f, (screen.Height - windowSize.Y) / 2f);
            Objects = new List<GameObject>();
            Objects.Add(new Sprite(Game1.WhitePixel, winner, pos, Vector2.One, windowSize, Vector2.Zero));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime);
            }
            Game1.InputManager.Update(gameTime);
            if (Objects[0].HitBox.Contains(Game1.InputManager.Mouse.Position) && Game1.InputManager.PreviousMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.InputManager.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                Game1.ScreenManager.Clear();
                Game1.ScreenManager.SetScreen(new PlayScreen());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Objects.Count; i ++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }

    }
}
