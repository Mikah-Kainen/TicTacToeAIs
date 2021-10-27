using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.ScreenStuff
{
    public class PlayScreen : IScreen
    {
        public List<GameObject> Objects { get; set; }
        public Sprite[][] Tiles { get; set; }

        public PlayScreen()
        {
            Objects = new List<GameObject>();
            Tiles = new Sprite[3][];

            for (int y = 0; y < 3; y ++)
            {
                Tiles[y] = new Sprite[3];
                for (int x = 0; x < 3; x ++)
                {
                    Tiles[y][x] = new Sprite(Game1.WhitePixel, Color.White, new Vector2(50 + 105 * x, 50 + 105 * y), Vector2.One, new Vector2(100, 100), Vector2.Zero);
                    Objects.Add(Tiles[y][x]);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            //for(int i = 0; i < Objects.Count; i ++)
            //{
            //    Objects[i].Update(gameTime);
            //}
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
