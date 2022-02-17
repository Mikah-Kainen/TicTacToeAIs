using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;
namespace TikTakToe.GBVPlayerTypes
{
    class GBVBasicPlayer : GBVPlayer
    {
        private Random random;
        public GBVBasicPlayer(Players playerID, Random random)
            : base(playerID)
        {
            this.random = random;
            GetPlayerValue = new Dictionary<int, Dictionary<Players, int>>();
        }

        public override (int y, int x) SelectTile(GridBoard currentTree)
        {
            GridBoard currentGame = currentTree;
            for (int y = 0; y < currentGame.YLength; y++)
            {
                for (int x = 0; x < currentGame.XLength; x++)
                {
                    bool canMoveRight = x + currentGame.WinSize - 1 < currentGame.XLength;
                    bool canMoveLeft = x - currentGame.WinSize - 1 >= 0;
                    bool canMoveDown = y + currentGame.WinSize - 1 < currentGame.YLength;
                    if (currentGame[y, x].State.Owner == PlayerID || currentGame[y, x].State.Owner == Players.None)
                    {
                        if (canMoveRight)
                        {
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y, x + i].State.Owner == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y, x + i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y, x + blankSquare);
                            }
                        }
                        if (canMoveDown)
                        {
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x].State.Owner == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x);
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x + i].State.Owner == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x + i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x + blankSquare);
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x - i].State.Owner == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x - i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x - blankSquare);
                            }
                        }
                    }
                }
            }


            for (int y = 0; y < currentGame.YLength; y++)
            {
                for (int x = 0; x < currentGame.XLength; x++)
                {
                    bool canMoveRight = x + currentGame.WinSize - 1 < currentGame.XLength;
                    bool canMoveLeft = x - currentGame.WinSize - 1 >= 0;
                    bool canMoveDown = y + currentGame.WinSize - 1 < currentGame.YLength;
                    if (currentGame[y, x].State.Owner != PlayerID)
                    {
                        Players currentPlayer = currentGame[y, x].State.Owner;
                        if (canMoveRight)
                        {
                            if (currentPlayer == Players.None)
                            {
                                currentPlayer = currentGame[y, x + 1].State.Owner;
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y, x + i].State.Owner == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y, x + i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y, x + blankSquare);
                            }
                        }
                        if (canMoveDown)
                        {
                            if (currentPlayer == Players.None)
                            {
                                currentPlayer = currentGame[y + 1, x].State.Owner;
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x].State.Owner == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x);
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (currentPlayer == Players.None)
                            {
                                currentPlayer = currentGame[y + 1, x + 1].State.Owner;
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x + i].State.Owner == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x + i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x + blankSquare);
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (currentPlayer == Players.None)
                            {
                                currentPlayer = currentGame[y + 1, x - 1].State.Owner;
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i, x - i].State.Owner == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i, x - i].State.Owner == Players.None)
                                {
                                    if (blankSquare != -1)
                                    {
                                        playerCount = 0;
                                        break;
                                    }
                                    blankSquare = i;
                                }
                                else
                                {
                                    playerCount = 0;
                                    break;
                                }
                            }
                            if (playerCount == currentGame.WinSize - 1)
                            {
                                return (y + blankSquare, x - blankSquare);
                            }
                        }
                    }
                }
            }

            return currentGame.RandomMove(random);

            throw new Exception("No Available Moves");
        }
    }
}
