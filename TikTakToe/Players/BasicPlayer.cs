using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.Players
{
    public class BasicPlayer : Player
    {
        private Random random;
        public BasicPlayer(Color playerColor, Random random)
            : base(playerColor)
        {
            this.random = random;
        }

        public override Sprite SelectTile(Sprite[][] currentGame)
        {
            for (int y = 0; y < currentGame.Length; y++)
            {
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < currentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < currentGame.Length;
                    if (currentGame[y][x].Tint == PlayerColor)
                    {
                        if (canMoveRight)
                        {
                            if (currentGame[y][x + 1].Tint == Color.White && currentGame[y][x + 2].Tint == PlayerColor)
                            {
                                return currentGame[y][x + 1];
                            }
                            else if (currentGame[y][x + 1].Tint == PlayerColor && currentGame[y][x + 2].Tint == Color.White)
                            {
                                return currentGame[y][x + 2];
                            }
                        }
                        if (canMoveDown)
                        {
                            if (currentGame[y + 1][x].Tint == Color.White && currentGame[y + 2][x].Tint == PlayerColor)
                            {
                                return currentGame[y + 1][x];
                            }
                            else if (currentGame[y + 1][x].Tint == PlayerColor && currentGame[y + 2][x].Tint == Color.White)
                            {
                                return currentGame[y + 2][x];
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (currentGame[y + 1][x + 1].Tint == Color.White && currentGame[y + 2][x + 2].Tint == PlayerColor)
                            {
                                return currentGame[y + 1][x + 1];
                            }
                            else if (currentGame[y + 1][x + 1].Tint == PlayerColor && currentGame[y + 2][x + 2].Tint == Color.White)
                            {
                                return currentGame[y + 2][x + 2];
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (currentGame[y + 1][x - 1].Tint == Color.White && currentGame[y + 2][x - 2].Tint == PlayerColor)
                            {
                                return currentGame[y + 1][x - 1];
                            }
                            else if (currentGame[y + 1][x - 1].Tint == PlayerColor && currentGame[y + 2][x - 2].Tint == Color.White)
                            {
                                return currentGame[y + 2][x - 2];
                            }
                        }
                    }
                    else if (currentGame[y][x].Tint == Color.White)
                    {
                        if (canMoveRight && currentGame[y][x + 1].Tint == PlayerColor && currentGame[y][x + 2].Tint == PlayerColor)
                        {
                            return currentGame[y][x];

                        }
                        if (canMoveDown && currentGame[y + 1][x].Tint == PlayerColor && currentGame[y + 2][x].Tint == PlayerColor)
                        {
                            return currentGame[y][x];

                        }
                        if (canMoveDown && canMoveRight && currentGame[y + 1][x + 1].Tint == PlayerColor && currentGame[y + 2][x + 2].Tint == PlayerColor)
                        {
                            return currentGame[y][x];

                        }
                        if (canMoveDown && canMoveLeft && currentGame[y + 1][x - 1].Tint == PlayerColor && currentGame[y + 2][x - 2].Tint == PlayerColor)
                        {
                            return currentGame[y][x];

                        }
                    }
                }
            }

            for (int y = 0; y < currentGame.Length; y++)
            {
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < currentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < currentGame.Length;
                    if (currentGame[y][x].Tint != PlayerColor && currentGame[y][x].Tint != Color.White)
                    {
                        Color OpponentColor = currentGame[y][x].Tint;
                        if (canMoveRight)
                        {
                            if (currentGame[y][x + 1].Tint == Color.White && currentGame[y][x + 2].Tint == OpponentColor)
                            {
                                return currentGame[y][x + 1];
                            }
                            else if (currentGame[y][x + 1].Tint == OpponentColor && currentGame[y][x + 2].Tint == Color.White)
                            {
                                return currentGame[y][x + 2];
                            }
                        }
                        if (canMoveDown)
                        {
                            if (currentGame[y + 1][x].Tint == Color.White && currentGame[y + 2][x].Tint == OpponentColor)
                            {
                                return currentGame[y + 1][x];
                            }
                            else if (currentGame[y + 1][x].Tint == OpponentColor && currentGame[y + 2][x].Tint == Color.White)
                            {
                                return currentGame[y + 2][x];
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (currentGame[y + 1][x + 1].Tint == Color.White && currentGame[y + 2][x + 2].Tint == OpponentColor)
                            {
                                return currentGame[y + 1][x + 1];
                            }
                            else if (currentGame[y + 1][x + 1].Tint == OpponentColor && currentGame[y + 2][x + 2].Tint == Color.White)
                            {
                                return currentGame[y + 2][x + 2];
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (currentGame[y + 1][x - 1].Tint == Color.White && currentGame[y + 2][x - 2].Tint == OpponentColor)
                            {
                                return currentGame[y + 1][x - 1];
                            }
                            else if (currentGame[y + 1][x - 1].Tint == OpponentColor && currentGame[y + 2][x - 2].Tint == Color.White)
                            {
                                return currentGame[y + 2][x - 2];
                            }
                        }
                    }
                    else if (currentGame[y][x].Tint == Color.White)
                    {
                        if (canMoveRight && currentGame[y][x + 1].Tint != Color.White && currentGame[y][x + 1].Tint == currentGame[y][x + 2].Tint)
                        {
                            return currentGame[y][x];
                        }
                        if (canMoveDown && currentGame[y + 1][x].Tint != Color.White && currentGame[y + 1][x].Tint == currentGame[y + 2][x].Tint)
                        {
                            return currentGame[y][x];
                        }
                        if (canMoveDown && canMoveRight && currentGame[y + 1][x + 1].Tint != Color.White && currentGame[y + 1][x + 1].Tint == currentGame[y + 2][x + 2].Tint)
                        {
                            return currentGame[y][x];
                        }
                        if (canMoveDown && canMoveLeft && currentGame[y + 1][x - 1].Tint != Color.White && currentGame[y + 1][x - 1].Tint == currentGame[y + 2][x - 2].Tint)
                        {
                            return currentGame[y][x];
                        }
                    }
                }
            }

            int start = random.Next(0, 9);
            for(int i = 0; i < 9; i ++)
            {
                if(start == 9)
                {
                    start = 0;
                }
                int y = start / 3;
                int x = start % 3;
                if(currentGame[y][x].Tint == Color.White)
                {
                    return currentGame[y][x];
                }
                start++;
            }
            
            throw new Exception("No Available Moves");
        }
    }
}
