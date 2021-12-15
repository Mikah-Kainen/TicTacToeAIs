using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TikTakToe.DrawStuff;
using TikTakToe.PlayerTypes;

namespace TikTakToe.ScreenStuff
{
    public class PlayScreen : IScreen
    {
        public List<GameObject> Objects { get; set; }
        public Random Random { get; set; }
        public Node<Board> GameTree { get; set; }

        public Dictionary<Players, Color> PlayerColor = new Dictionary<Players, Color>()
        {
            [Players.None] = Color.White,
            [Players.Player1] = Color.Red,
            [Players.Player2] = Color.Blue,
            [Players.Player3] = Color.Yellow,
        };

        public Dictionary<Players, Func<IGameState<Board>, Players>> GetNextPlayer = new Dictionary<Players, Func<IGameState<Board>, Players>>()
        {
            [Players.None] = state => Players.Player1,
            [Players.Player1] = state => Players.Player2,
            [Players.Player2] = state => Players.Player3,
            [Players.Player3] = state => Players.Player1,
        };

        public Dictionary<Players, Player> GetPlayer { get; set; }

        public PlayScreen()
        {
            Objects = new List<GameObject>();

            Random = new Random();

            List<Players> activePlayers = new List<Players>();
            activePlayers.Add(Players.Player1);                
            activePlayers.Add(Players.Player2);
            activePlayers.Add(Players.Player3);

            GetPlayer = new Dictionary<Players, Player>();
            //GetPlayer.Add(Players.Player1, new MiniMaxPlayer(Players.Player1, Players.Player2, Random));
            GetPlayer.Add(Players.Player1, new MaxiMaxPlayer(Players.Player1, activePlayers, Random));
            GetPlayer.Add(Players.Player2, new MaxiMaxPlayer(Players.Player2, activePlayers, Random));
            GetPlayer.Add(Players.Player3, new MaxiMaxPlayer(Players.Player3, activePlayers, Random));

            GameTree = new Node<Board>();

            GameTree.State = new Board(4, 4, 3, GetNextPlayer);
            GameTree.State[0][0] = Players.Player3;
            GameTree.State[3][0] = Players.Player2;
            GameTree.State[0][3] = Players.Player1;
            GameTree.State[3][3] = Players.Player3;

            GameTree.CreateTree(GameTree.State);
            foreach (Players player in activePlayers)
            {
                if (GetPlayer[player] is IMiniMaxPlayer currentPlayer)
                {
                    currentPlayer.GetPlayerValue = new Dictionary<int, Dictionary<Players, int>>();
                    currentPlayer.SetValues(GameTree);
                }
            }
        }

        public void Update(GameTime gameTime)
        {

            Game1.InputManager.Update(gameTime);
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime);
            }
            if (Game1.InputManager.WasKeyPressed(Keys.Space))
            {
                (int y, int x) selected = GetPlayer[GameTree.State.NextPlayer].SelectTile(GameTree);
                for(int i = 0; i < GameTree.Children.Count; i ++)
                {
                    if(GameTree.Children[i].State[selected.y][selected.x] == GameTree.State.NextPlayer)
                    {
                        GameTree = GameTree.Children[i];
                        break;
                    }
                }
                if(GameTree.State.IsWin)
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(PlayerColor[GameTree.State.PreviousPlayer], Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
                else if(!GameTree.State.IsPlayable())
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(PlayerColor[Players.None], Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int y = 0; y < GameTree.State.Length; y ++)
            {
                for(int x = 0; x < GameTree.State[y].Length; x ++)
                {
                    Vector2 Position = new Vector2(25 + 104 * x, 25 + 104 * y);
                    spriteBatch.Draw(Game1.WhitePixel, Position, null, PlayerColor[GameTree.State[y][x]], 0, Vector2.Zero, new Vector2(100, 100), SpriteEffects.None, 1);
                }
            }
            for(int i = 0; i < Objects.Count; i ++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }

    }
}
