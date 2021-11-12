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
        public Board CurrentBoard { get; set; }
        public Random Random { get; set; }
        public Player RedPlayer { get; set; }
        public Player BluePlayer { get; set; }

        private Player previousPlayer;
        private Player nextPlayer;

        public Dictionary<Players, Color> PlayerColor = new Dictionary<Players, Color>()
        {
            [Players.None] = Color.White,
            [Players.Player1] = Color.Red,
            [Players.Player2] = Color.Red,
        };

        public Dictionary<Players, Func<IGameState<Board>, Players>> NextPlayer = new Dictionary<Players, Func<IGameState<Board>, Players>>()
        {
            [Players.Player1] = state => Players.Player2,
            [Players.Player2] = state => Players.Player1,
        };

        public PlayScreen()
        {
            Objects = new List<GameObject>();
            CurrentBoard = new Board(3, 3, NextPlayer);

            Random = new Random(); 

            RedPlayer = new BasicPlayer(Players.Player1, Random);
            BluePlayer = new BasicPlayer(Players.Player2, Random);
            previousPlayer = BluePlayer;
            nextPlayer = RedPlayer;

            CurrentBoard[1][1] = Players.Player1;
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
                (int y, int x) selected = nextPlayer.SelectTile(CurrentBoard);
                CurrentBoard[selected.y][selected.x] = nextPlayer.PlayerID;
                if(CurrentBoard.DidPlayerWin(nextPlayer.PlayerID))
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(PlayerColor[nextPlayer.PlayerID], Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
                else if(!CurrentBoard.IsPlayable())
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(PlayerColor[Players.None], Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
                Player temp = nextPlayer;
                nextPlayer = previousPlayer;
                previousPlayer = temp;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int y = 0; y < CurrentBoard.Length; y ++)
            {
                for(int x = 0; x < CurrentBoard[y].Length; x ++)
                {
                    Vector2 Position = new Vector2(25 + 104 * x, 25 + 104 * y);
                    spriteBatch.Draw(Game1.WhitePixel, Position, null, PlayerColor[CurrentBoard[y][x]], 0, Vector2.Zero, new Vector2(100, 100), SpriteEffects.None, 1);
                }
            }
            for(int i = 0; i < Objects.Count; i ++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }

    }
}
