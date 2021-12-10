using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.PlayerTypes
{
    public class BasicPlayer : Player
    {
        private Random random;
        public BasicPlayer(Players playerID, Random random)
            : base(playerID)
        {
            this.random = random;
            GetPlayerValue = new Dictionary<Node<Board>, Dictionary<Players, int>>();
        }

        public override (int y, int x) SelectTile(Node<Board> CurrentTree)
        {
            Board currentGame = CurrentTree.State;
            for (int y = 0; y < currentGame.Length; y++)
            {
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    bool canMoveRight = x + currentGame.WinSize - 1 < currentGame[y].Length;
                    bool canMoveLeft = x - currentGame.WinSize - 1 >= 0;
                    bool canMoveDown = y + currentGame.WinSize - 1 < currentGame.Length;
                    if (currentGame[y][x] == PlayerID || currentGame[y][x] == Players.None)
                    {
                        if (canMoveRight)
                        {
                            int playerCount = 0;
                            int blankSquare = -1;
                            for(int i = 0; i < currentGame.WinSize; i ++)
                            {
                                if(currentGame[y][x + i] == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if(currentGame[y][x + i] == Players.None)
                                {
                                    if(blankSquare != -1)
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
                            if(playerCount == currentGame.WinSize - 1)
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
                                if (currentGame[y + i][x] == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x] == Players.None)
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
                                if (currentGame[y + i][x + i] == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x + i] == Players.None)
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
                                if (currentGame[y + i][x - i] == PlayerID)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x - i] == Players.None)
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


            for (int y = 0; y < currentGame.Length; y++)
            {
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    bool canMoveRight = x + currentGame.WinSize - 1 < currentGame[y].Length;
                    bool canMoveLeft = x - currentGame.WinSize - 1 >= 0;
                    bool canMoveDown = y + currentGame.WinSize - 1 < currentGame.Length;
                    if (currentGame[y][x] != PlayerID)
                    {
                        Players currentPlayer = currentGame[y][x];
                        if (canMoveRight)
                        {
                            if(currentPlayer == Players.None)
                            {
                                currentPlayer = currentGame[y][x + 1];
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y][x + i] == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y][x + i] == Players.None)
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
                                currentPlayer = currentGame[y + 1][x];
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i][x] == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x] == Players.None)
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
                                currentPlayer = currentGame[y + 1][x + 1];
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i][x + i] == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x + i] == Players.None)
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
                                currentPlayer = currentGame[y + 1][x - 1];
                            }
                            int playerCount = 0;
                            int blankSquare = -1;
                            for (int i = 0; i < currentGame.WinSize; i++)
                            {
                                if (currentGame[y + i][x - i] == currentPlayer)
                                {
                                    playerCount++;
                                }
                                else if (currentGame[y + i][x - i] == Players.None)
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

            int max = currentGame.Length * currentGame[0].Length;
            int start = random.Next(0, max);
            for(int i = 0; i < max; i ++)
            {
                if(start == max)
                {
                    start = 0;
                }
                int y = start / currentGame.Length;
                int x = start % currentGame[0].Length;
                if(currentGame[y][x] == Players.None)
                {
                    return (y, x);
                }
                start++;
            }
            
            throw new Exception("No Available Moves");
        }
    }
}
