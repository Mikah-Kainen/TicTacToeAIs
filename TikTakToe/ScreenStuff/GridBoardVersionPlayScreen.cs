using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NeuralNetwork;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TikTakToe.DrawStuff;
using TikTakToe.GBVPlayerTypes;

namespace TikTakToe.ScreenStuff
{
    class GridBoardVersionPlayScreen : IScreen
    {
        public List<GameObject> Objects { get; set; }
        public Random Random { get; set; }
        public GridBoard GameTree { get; set; }

        public Dictionary<Players, Color> PlayerColor = new Dictionary<Players, Color>()
        {
            [Players.None] = Color.White,
            [Players.Player1] = Color.Red,
            [Players.Player2] = Color.Blue,
            //[Enums.Players.Player3] = Color.Yellow,
        };

        public Dictionary<Players, Func<GridBoard, Players>> GetNextPlayer = new Dictionary<Players, Func<GridBoard, Players>>()
        {
            [Players.None] = state => Players.Player1,
            [Players.Player1] = state => Players.Player2,
            [Players.Player2] = state => Players.Player1,
            //[Players.Player3] = state => Players.Player1,
        };

        public Dictionary<Players, GBVPlayer> GetPlayer { get; set; }

        public GridBoardVersionPlayScreen(NeuralNet playerNet)
        {
            Objects = new List<GameObject>();

            Random = new Random();

            List<Players> activePlayers = new List<Players>();
            activePlayers.Add(Players.Player1);
            activePlayers.Add(Players.Player2);
            //activePlayers.Add(Players.Player3);

            GetPlayer = new Dictionary<Players, GBVPlayer>();
            //GetPlayer.Add(Players.Player1, new MaxiMaxPlayer(Players.Player1, activePlayers, Random));
            GetPlayer.Add(Players.Player1, new GBVBasicPlayer(Players.Player1, Random));
            GetPlayer.Add(Players.Player2, new GBVNeuralNetPlayer(Players.Player2, playerNet, Random));
            //GetPlayer.Add(Players.Player3, new MaxiMaxPlayer(Players.Player3, activePlayers, Random));

            GameTree = new GridBoard(GridBoard.CreateNewGridSquares(3, 3), Players.None, 3, GetNextPlayer);
            //GameTree.State = new GridBoard(3, 3, 3, GetNextPlayer).CurrentGame;


            //GameTree.CreateTree(GameTree);


            //foreach (Players player in activePlayers)
            //{
            //    if (GetPlayer[player] is IMiniMaxPlayer currentPlayer)
            //    {
            //        currentPlayer.GetPlayerValue = new Dictionary<int, Dictionary<Players, int>>();
            //        currentPlayer.SetValues(GameTree);
            //    }
            //}

            if (playerNet == null)
            {
                foreach (Players player in activePlayers)
                {
                    if (GetPlayer[player] is GBVNeuralNetPlayer currentPlayer)
                    {
                        currentPlayer.Net = NeuralNetwork.TurnBasedBoardGameTrainerStuff.TurnBasedBoardGameTrainer<GridBoardState, GridBoardSquare>.GetNet(GameTree, MakeMove, 1000, 1000, Random);
                        //currentPlayer.Net = NeuralNetTrainer.GetNet(GameTree, 1000, 1000, Random);
                    }
                }
            }

            GameTree = new GridBoard(GridBoard.CreateNewGridSquares(3, 3), Players.None, 3, GetNextPlayer);
        }


        public void Update(GameTime gameTime)
        {
            ////////////////
            ///////////////////
            //Lets make a UI that will let you build your own Net at the start. There should be a dropdown for which Player to train against and a save button and maybe a display for how many times the Net won
            //////////////////
            //////////
            ///

            Game1.InputManager.Update(gameTime);
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime);
            }
            if (Game1.InputManager.WasKeyPressed(Keys.Space))
            {
                if (!GameTree.IsTerminal)
                {
                    (int y, int x) selected = GetPlayer[GameTree.NextPlayer].SelectTile(GameTree);
                    var children = GameTree.GetChildren();
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i][selected.y, selected.x].State.Owner == GameTree.NextPlayer)
                        {
                            GameTree = (GridBoard)children[i];
                        }
                    }
                }

                NeuralNet currentNet = null;
                foreach (KeyValuePair<Players, Color> kvp in PlayerColor)
                {
                    if (kvp.Key != Players.None && GetPlayer[kvp.Key] is GBVNeuralNetPlayer currentPlayer)
                    {
                        currentNet = ((GBVNeuralNetPlayer)GetPlayer[kvp.Key]).Net;
                    }
                }
                if (GameTree.IsTerminal)
                {
                    Game1.ScreenManager.SetScreen(new EndScreen(PlayerColor[GameTree.GetWinner()], currentNet, Game1.WhitePixel.GraphicsDevice.Viewport.Bounds));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < GameTree.YLength; y++)
            {
                for (int x = 0; x < GameTree.XLength; x++)
                {
                    Vector2 Position = new Vector2(25 + 104 * x, 25 + 104 * y);
                    spriteBatch.Draw(Game1.WhitePixel, Position, null, PlayerColor[GameTree[y, x].State.Owner], 0, Vector2.Zero, new Vector2(100, 100), SpriteEffects.None, 1);
                }
            }
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }


        public static bool MakeMove(Pair<GridBoardState, GridBoardSquare> currentPair)
        {
            bool returnValue = false;
            if (currentPair.IsAlive)
            {
                //currentPair.Success++;
                double[] inputs = new double[currentPair.Board.YLength * currentPair.Board.XLength];
                for (int y = 0; y < currentPair.Board.YLength; y++)
                {
                    for (int x = 0; x < currentPair.Board.XLength; x++)
                    {
                        switch (currentPair.Board[y, x].State.Owner)
                        {
                            case Players.None:
                                inputs[y * currentPair.Board.YLength + x] = 0;
                                break;

                            case Players.Player1:
                                inputs[y * currentPair.Board.YLength + x] = 1;
                                break;

                            case Players.Player2:
                                inputs[y * currentPair.Board.YLength + x] = 2;
                                break;

                            case Players.Player3:
                                inputs[y * currentPair.Board.YLength + x] = 3;
                                break;
                        }
                    }
                }
                double[] computedValues = currentPair.Net.Compute(inputs);
                int target = -1;
                for (int a = 0; a < computedValues.Length; a++)
                {
                    if (computedValues[a] == 1)
                    {
                        if (target != -1)
                        {
                            currentPair.IsAlive = false;
                            goto deathZone;
                        }
                        target = a;
                    }
                }
                if (target == -1)
                {
                    currentPair.IsAlive = false;
                    goto deathZone;
                }
                int yVal = target / currentPair.Board.YLength;
                int xVal = target % currentPair.Board.XLength;
                if (currentPair.Board[yVal, xVal].State.Owner != Players.None)
                {
                    currentPair.IsAlive = false;
                    goto deathZone;
                }
                List<IGridBoard<GridBoardState, GridBoardSquare>> children = currentPair.Board.GetChildren();
                for (int z = 0; z < children.Count; z++)
                {
                    if (children[z][yVal, xVal].State.Owner == currentPair.Board.NextPlayer)
                    {
                        currentPair.Board = children[z];
                        currentPair.Success++;
                    }
                }
                if (currentPair.Board.IsTerminal == true)
                {
                    currentPair.Success = 10000;
                    currentPair.IsAlive = false;
                }
                else
                {
                    returnValue = true;
                }
                currentPair.Success++;
            //                correctCount++;
            deathZone:;
            }
            return returnValue;
        }
    }
}
