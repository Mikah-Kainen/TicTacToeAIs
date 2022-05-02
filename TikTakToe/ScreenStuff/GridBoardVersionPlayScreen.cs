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
            [Players.Player3] = Color.Yellow,
        };

        public Dictionary<Players, Func<GridBoard, Players>> GetNextPlayer = new Dictionary<Players, Func<GridBoard, Players>>()
        {
            [Players.None] = state => Players.Player1,
            [Players.Player1] = state => Players.Player2,
            [Players.Player2] = state => Players.Player1,
            //[Players.Player3] = state => Players.Player1,
        };

        public Dictionary<Players, GBVPlayer> GetPlayer { get; set; }

        static Dictionary<int, Dictionary<Players, int>> PlayerValueMap { get; set; }
        public GridBoardVersionPlayScreen(NeuralNet playerNet)
        {
            Objects = new List<GameObject>();

            Random = new Random();

            List<Players> activePlayers = new List<Players>();
            activePlayers.Add(Players.Player1);
            activePlayers.Add(Players.Player2);
            //activePlayers.Add(Players.Player3);

            GetPlayer = new Dictionary<Players, GBVPlayer>();

            GetPlayer.Add(Players.Player1, new GBVOverfitPlayer(Players.Player1));
            //GetPlayer.Add(Players.Player1, new GBVRandomPlayer(Players.Player1, Random));
            //GetPlayer.Add(Players.Player1, new GBVBasicPlayer(Players.Player1, Random));
            //GetPlayer.Add(Players.Player1, new GBVMaxiMaxPlayer(Players.Player1, activePlayers, Random));

            GetPlayer.Add(Players.Player2, new GBVNeuralNetPlayer(Players.Player2, playerNet, Random));

            //GetPlayer.Add(Players.Player3, new GBVRandomPlayer(Players.Player3, Random));
            GetPlayer.Add(Players.Player3, new GBVBasicPlayer(Players.Player3, Random));
            //GetPlayer.Add(Players.Player3, new GBVMaxiMaxPlayer(Players.Player3, activePlayers, Random));

            GameTree = new GridBoard(GridBoard.CreateNewGridSquares(3, 3), Players.None, 3, GetNextPlayer);
            //GameTree.CreateTree(GameTree);


            foreach (Players player in activePlayers)
            {
                if (GetPlayer[player] is GBVMaxiMaxPlayer currentPlayer)
                {
                    currentPlayer.GetPlayerValue = new Dictionary<int, Dictionary<Players, int>>();
                    currentPlayer.SetValues(GameTree);
                }
            }

                   
            Func<IGridBoard<GridBoardState, GridBoardSquare>, Random, bool>[] opponentMoves = new Func<IGridBoard<GridBoardState, GridBoardSquare>, Random, bool>[activePlayers.Count - 1];
            GBVPlayer trainingOpponent = GetPlayer[Players.Player1];
            if(trainingOpponent is GBVMaxiMaxPlayer maxiMaxOpponent && maxiMaxOpponent.GetPlayerValue == null)
            {
                maxiMaxOpponent.GetPlayerValue = new Dictionary<int, Dictionary<Players, int>>();
                maxiMaxOpponent.SetValues(GameTree);
            }
            //PlayerValueMap = GetPlayer[Players.Player1].GetPlayerValue;
            //opponentMoves[0] = (board, random) => trainingOpponent.SelectTile((GridBoard)board, PlayerValueMap).FindChild(Players.Player1, (GridBoard)board);
            opponentMoves[0] = (board, random) =>
            {
                (int y, int x) = trainingOpponent.SelectTile((GridBoard)board);

                if (board[y, x].State.Owner != Players.None)
                {
                    return false;
                }

                board[y, x].State.Owner = trainingOpponent.PlayerID;
                ((GridBoard)board).PreviousPlayer = trainingOpponent.PlayerID;
                if (board.GetWinner() != Players.None)
                {
                    return false;
                }
                return true;
            };




            //GameTree = new GridBoard(GridBoard.CreateNewGridSquares(3, 3), Players.None, 3, GetNextPlayer);
            TurnBasedBoardGameTrainer<GridBoardState, GridBoardSquare> trainer = new TurnBasedBoardGameTrainer<GridBoardState, GridBoardSquare>();
            trainer.BoardDied += (trainer, deadBoardIntTuple) =>
            {
                IGridBoard<GridBoardState, GridBoardSquare> deadBoard;
                int currentGeneration;
                
                (deadBoard, currentGeneration) = deadBoardIntTuple;

                Players winner = deadBoard.GetWinner();
                if(winner == Players.None)
                {
                    bool isUnownedSquare = false;
                    for (int y = 0; y < deadBoard.YLength; y++)
                    {
                        for (int x = 0; x < deadBoard.XLength; x++)
                        {
                            if (deadBoard[y, x].State.Owner == Players.None)
                            {
                                isUnownedSquare = true;
                            }
                        }
                    }
                    if (isUnownedSquare)
                    {
                        InvalidMoves++;
                        GenerationalInvalidMoves[currentGeneration]++;
                    }
                    else
                    {
                        TieMoves++;
                        GenerationalTieMoves[currentGeneration]++;
                    }
                }
                else if(winner == activePlayers[activePlayers.Count - 1])
                {
                    WinningMoves++;
                    GenerationalWinningMoves[currentGeneration]++;
                }
                else
                {
                    LosingMoves++;
                    GenerationalLosingMoves[currentGeneration]++;
                }
                GameLengthCounts[((GridBoard)deadBoard).MovesMade()]++;
            };

            int totalGenerations = 50;
            GenerationalInvalidMoves = new int[totalGenerations];
            GenerationalLosingMoves = new int[totalGenerations];
            GenerationalTieMoves = new int[totalGenerations];
            GenerationalWinningMoves = new int[totalGenerations];
            foreach (Players player in activePlayers)
            {
                if (GetPlayer[player] is GBVNeuralNetPlayer currentPlayer)
                {
                   currentPlayer.Net = trainer.GetNet(GameTree, activePlayers.ToArray(), MakeNeuralNetMove, opponentMoves, numberOfSimulations: 1000, totalGenerations, Random, playerNet);
                   GameTree = new GridBoard(GridBoard.CreateNewGridSquares(ylength: 3, xlength: 3), Players.None, winSize: 3, GetNextPlayer);
                }
            }
        }

        public int InvalidMoves;
        public int[] GenerationalInvalidMoves;

        public int LosingMoves;
        public int[] GenerationalLosingMoves;

        public int TieMoves;
        public int[] GenerationalTieMoves;

        public int WinningMoves;
        public int[] GenerationalWinningMoves;

        public int[] GameLengthCounts = new int[10];

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


        public bool MakeNeuralNetMove(BoardNetPair<GridBoardState, GridBoardSquare> currentPair, Random random)
        {
            Players neuralNetPlayer = currentPair.Board.NextPlayer;

            if (currentPair.Board.IsTerminal) 
            {
                return false;
            }

            //bool outputValid = false;
            //int selected = 0;
            //for(int i = 0; i < inputs.Length; i ++)
            //{
            //    if(output[i] == 0)
            //    {

            //    }
            //    else if(output[i] == 1)
            //    {
            //        if(!outputValid)
            //        {
            //            outputValid = true;
            //            selected = i;
            //        }
            //        else
            //        {
            //            outputValid = false;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("SOMETHING IS WRONG");
            //    }
            //}

            //if(!outputValid)
            //{
            //    return false;
            //}

            //int selectedX = selected % currentPair.Board.YLength;
            //int selectedY = selected / currentPair.Board.YLength;

            //if(currentPair.Board[selectedY, selectedX].State.Owner != Players.None)
            //{
            //    return false;
            //}

            (int selectedY, int selectedX) selected = currentPair.BoardNetPairToNeuralNetOutput();
            if(currentPair.Board[selected.selectedY, selected.selectedX].State.Owner != Players.None)
            {
                return false;
            }

            currentPair.Board[selected.selectedY, selected.selectedX].State.Owner = neuralNetPlayer;
            ((GridBoard)currentPair.Board).PreviousPlayer = neuralNetPlayer;
            currentPair.Success++;
            if(currentPair.Board.GetWinner() == neuralNetPlayer)
            {
                currentPair.Success = 100;
                return false;
            }
            if(currentPair.Board.GetWinner() != Players.None)
            {
                return false;
            }
            return true;
        }


        //public void MakeMove(BoardNetPair<GridBoardState, GridBoardSquare> currentPair, Random random)
        //{
        //    bool multipleMovesSelected = false;
        //    bool noMoveSelected = false;
        //    bool impossibleMoveSelected = false;
        //    bool correctMoveSelected = false;
        //    bool tieMoveSelected = false;
        //    bool winningMoveSelected = false;

        //    Players neuralNetPlayer = currentPair.Board.NextPlayer;
        //    List<IGridBoard<GridBoardState, GridBoardSquare>> children = currentPair.Board.GetChildren();
        //    if (!currentPair.Board.IsTerminal)
        //    {
        //        //currentPair.Success++;
        //        double[] inputs = new double[currentPair.Board.YLength * currentPair.Board.XLength];
        //        for (int y = 0; y < currentPair.Board.YLength; y++)
        //        {
        //            for (int x = 0; x < currentPair.Board.XLength; x++)
        //            {
        //                switch (currentPair.Board[y, x].State.Owner)
        //                {
        //                    case Players.None:
        //                        inputs[y * currentPair.Board.YLength + x] = 0;
        //                        break;

        //                    case Players.Player1:
        //                        inputs[y * currentPair.Board.YLength + x] = 1;
        //                        break;

        //                    case Players.Player2:
        //                        inputs[y * currentPair.Board.YLength + x] = 2;
        //                        break;

        //                    case Players.Player3:
        //                        inputs[y * currentPair.Board.YLength + x] = 3;
        //                        break;
        //                }
        //            }
        //        }
        //        double[] computedValues = currentPair.Net.Compute(inputs);
        //        int target = -1;
        //        for (int a = 0; a < computedValues.Length; a++)
        //        {
        //            if (computedValues[a] == 1)
        //            {
        //                if (target != -1)
        //                {
        //                    multipleMovesSelected = true;
        //                    goto deathZone;
        //                }
        //                target = a;
        //            }
        //        }
        //        if (target == -1)
        //        {
        //            noMoveSelected = true;
        //            goto deathZone;
        //        }
        //        int yVal = target / currentPair.Board.YLength;
        //        int xVal = target % currentPair.Board.XLength;
        //        if (currentPair.Board[yVal, xVal].State.Owner != Players.None)
        //        {
        //            if(currentPair.Board.IsTerminal)
        //            {

        //            }


        //            impossibleMoveSelected = true;
        //            goto deathZone;
        //        }
        //        for (int z = 0; z < children.Count; z++)
        //        {
        //            if (children[z][yVal, xVal].State.Owner == currentPair.Board.NextPlayer)
        //            {
        //                currentPair.Board = children[z];
        //                currentPair.Success++;
        //                break;
        //            }
        //        }
        //        if (currentPair.Board.IsTerminal == true)
        //        {
        //            Players winner = currentPair.Board.GetWinner();
        //            currentPair.Success = 500;
        //            if(winner == Players.None)
        //            {
        //                currentPair.Success = 5000;
        //                tieMoveSelected = true;
        //            }
        //            else if (winner == neuralNetPlayer)
        //            {
        //                currentPair.Success = 10000;
        //                winningMoveSelected = true;
        //            }
        //        }
        //        currentPair.Success++;
        //        correctMoveSelected = true;


        //        if(!((GridBoard)currentPair.Board).IsPlayable())
        //        {

        //        }

        //        return;
        //        deathZone:;
        //    }
        //    else
        //    {

        //    }
        //    currentPair.Board = children[random.Next(0, children.Count)];
        //}
    }
}
