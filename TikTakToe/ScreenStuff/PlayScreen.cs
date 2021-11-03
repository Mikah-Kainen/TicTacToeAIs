using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;
using TikTakToe.Players;

namespace TikTakToe.ScreenStuff
{
    public class PlayScreen : IScreen
    {
        public InputManager InputManager { get; set; }
        public List<GameObject> Objects { get; set; }
        public Sprite[][] Tiles { get; set; }

        public Random Random { get; set; }
        public Player RedPlayer { get; set; }
        public Player BluePlayer { get; set; }

        private Player previousPlayer;
        private Player nextPlayer;
        
        public PlayScreen(InputManager inputManager)
        {
            InputManager = inputManager;
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

            RedPlayer = new BasicPlayer(Color.Red, Random);
            BluePlayer = new BasicPlayer(Color.Blue, Random);
            previousPlayer = BluePlayer;
            nextPlayer = RedPlayer;
          
            //Tiles[0][0].Tint = Color.Red;
            //Tiles[2][2].Tint = Color.Red;
            //Tiles[1][2].Tint = Color.Red;
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
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

            //UpdateTile(GetTile(simulatingOutputs));
            if (InputManager.WasKeyPressed(Keys.Space))
            {
                nextPlayer.SelectTile(Tiles).Tint = nextPlayer.PlayerColor;
                Player temp = nextPlayer;
                nextPlayer = previousPlayer;
                previousPlayer = temp;

                Color winner = DidPlayerWin();
                if(winner != Color.White)
                {
                    Game1.ScreenManager.SetScreen(new WinnerScreen(winner, Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
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

        public Color DidPlayerWin()
        {
            for (int y = 0; y < Tiles.Length; y++)
            {
                for (int x = 0; x < Tiles[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < Tiles[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < Tiles.Length;
                    if (Tiles[y][x].Tint != Color.White)
                    {
                        Color OpponentColor = Tiles[y][x].Tint;
                        if (canMoveRight)
                        {
                            if (Tiles[y][x + 1].Tint == Color.White && Tiles[y][x + 2].Tint == OpponentColor)
                            {
                                return Tiles[y][x + 1].Tint;
                            }
                            else if (Tiles[y][x + 1].Tint == OpponentColor && Tiles[y][x + 2].Tint == Color.White)
                            {
                                return Tiles[y][x + 2].Tint;
                            }
                        }
                        if (canMoveDown)
                        {
                            if (Tiles[y + 1][x].Tint == Color.White && Tiles[y + 2][x].Tint == OpponentColor)
                            {
                                return Tiles[y + 1][x].Tint;
                            }
                            else if (Tiles[y + 1][x].Tint == OpponentColor && Tiles[y + 2][x].Tint == Color.White)
                            {
                                return Tiles[y + 2][x].Tint;
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (Tiles[y + 1][x + 1].Tint == Color.White && Tiles[y + 2][x + 2].Tint == OpponentColor)
                            {
                                return Tiles[y + 1][x + 1].Tint;
                            }
                            else if (Tiles[y + 1][x + 1].Tint == OpponentColor && Tiles[y + 2][x + 2].Tint == Color.White)
                            {
                                return Tiles[y + 2][x + 2].Tint;
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (Tiles[y + 1][x - 1].Tint == Color.White && Tiles[y + 2][x - 2].Tint == OpponentColor)
                            {
                                return Tiles[y + 1][x - 1].Tint;
                            }
                            else if (Tiles[y + 1][x - 1].Tint == OpponentColor && Tiles[y + 2][x - 2].Tint == Color.White)
                            {
                                return Tiles[y + 2][x - 2].Tint;
                            }
                        }
                    }
                }
            }
            return Color.White;
        }

    }
}
