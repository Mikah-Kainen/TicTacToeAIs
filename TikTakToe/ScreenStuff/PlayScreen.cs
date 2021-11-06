using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;
using TikTakToe.PlayerTypes;

namespace TikTakToe.ScreenStuff
{
    public class PlayScreen : IScreen
    {
        public List<GameObject> Objects { get; set; }
        public Sprite[][] Tiles { get; set; }

        public Random Random { get; set; }
        public Player RedPlayer { get; set; }
        public Player BluePlayer { get; set; }

        private Player previousPlayer;
        private Player nextPlayer;
        
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

            Random = new Random(); 

            RedPlayer = new BasicPlayer(Color.Purple, Random);
            BluePlayer = new BasicPlayer(Color.Blue, Random);
            previousPlayer = BluePlayer;
            nextPlayer = RedPlayer;
          
            //Tiles[0][0].Tint = Color.Red;
            //Tiles[2][2].Tint = Color.Red;
            //Tiles[1][1].Tint = Color.Red;
        }

        public void Update(GameTime gameTime)
        {
            double[] simulatingOutputs = new double[]
            {
                0,
                0,
                0,

                1,
                0,
                0,

                0,
                0,
                0,
            };

            Game1.InputManager.Update(gameTime);
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime);
            }
            if (Game1.InputManager.WasKeyPressed(Keys.Space))
            {
                nextPlayer.SelectTile(Tiles).Tint = nextPlayer.PlayerColor;
                Player temp = nextPlayer;
                nextPlayer = previousPlayer;
                previousPlayer = temp;

                Color winner = DidPlayerWin();
                if(winner != Color.White)
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(winner, Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
                else if(!IsPlayable())
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(Color.White, Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
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
