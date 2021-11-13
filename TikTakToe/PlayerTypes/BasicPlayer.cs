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
        }

        public override (int y, int x) SelectTile(Node<Board> CurrentTree)
        {
            Board currentGame = CurrentTree.Value;
            for (int y = 0; y < currentGame.Length; y++)
            {
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < currentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < currentGame.Length;
                    if (currentGame[y][x] == PlayerID)
                    {
                        if (canMoveRight)
                        {
                            if (currentGame[y][x + 1] == Players.None && currentGame[y][x + 2] == PlayerID)
                            {
                                return (y, x + 1);
                            }
                            else if (currentGame[y][x + 1] == PlayerID && currentGame[y][x + 2] == Players.None)
                            {
                                return (y, x + 2);
                            }
                        }
                        if (canMoveDown)
                        {
                            if (currentGame[y + 1][x] == Players.None && currentGame[y + 2][x] == PlayerID)
                            {
                                return (y + 1, x);
                            }
                            else if (currentGame[y + 1][x] == PlayerID && currentGame[y + 2][x] == Players.None)
                            {
                                return (y + 2, x);
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (currentGame[y + 1][x + 1] == Players.None && currentGame[y + 2][x + 2] == PlayerID)
                            {
                                return (y + 1, x + 1);
                            }
                            else if (currentGame[y + 1][x + 1] == PlayerID && currentGame[y + 2][x + 2] == Players.None)
                            {
                                return (y + 2, x + 2);
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (currentGame[y + 1][x - 1] == Players.None && currentGame[y + 2][x - 2] == PlayerID)
                            {
                                return (y + 1, x - 1);
                            }
                            else if (currentGame[y + 1][x - 1] == PlayerID && currentGame[y + 2][x - 2] == Players.None)
                            {
                                return (y + 2, x - 2);
                            }
                        }
                    }
                    else if (currentGame[y][x] == Players.None)
                    {
                        if (canMoveRight && currentGame[y][x + 1] == PlayerID && currentGame[y][x + 2] == PlayerID)
                        {
                            return (y, x);

                        }
                        if (canMoveDown && currentGame[y + 1][x] == PlayerID && currentGame[y + 2][x] == PlayerID)
                        {
                            return (y, x);

                        }
                        if (canMoveDown && canMoveRight && currentGame[y + 1][x + 1] == PlayerID && currentGame[y + 2][x + 2] == PlayerID)
                        {
                            return (y, x);

                        }
                        if (canMoveDown && canMoveLeft && currentGame[y + 1][x - 1] == PlayerID && currentGame[y + 2][x - 2] == PlayerID)
                        {
                            return (y, x);

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
                    if (currentGame[y][x] != PlayerID && currentGame[y][x] != Players.None)
                    {
                        Players OpponentID = currentGame[y][x];
                        if (canMoveRight)
                        {
                            if (currentGame[y][x + 1] == Players.None && currentGame[y][x + 2] == OpponentID)
                            {
                                return (y, x + 1);
                            }
                            else if (currentGame[y][x + 1] == OpponentID && currentGame[y][x + 2] == Players.None)
                            {
                                return (y, x + 2);
                            }
                        }
                        if (canMoveDown)
                        {
                            if (currentGame[y + 1][x] == Players.None && currentGame[y + 2][x] == OpponentID)
                            {
                                return (y + 1, x);
                            }
                            else if (currentGame[y + 1][x] == OpponentID && currentGame[y + 2][x] == Players.None)
                            {
                                return (y + 2, x);
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (currentGame[y + 1][x + 1] == Players.None && currentGame[y + 2][x + 2] == OpponentID)
                            {
                                return (y + 1, x + 1);
                            }
                            else if (currentGame[y + 1][x + 1] == OpponentID && currentGame[y + 2][x + 2] == Players.None)
                            {
                                return (y + 2, x + 2);
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (currentGame[y + 1][x - 1] == Players.None && currentGame[y + 2][x - 2] == OpponentID)
                            {
                                return (y + 1, x - 1);
                            }
                            else if (currentGame[y + 1][x - 1] == OpponentID && currentGame[y + 2][x - 2] == Players.None)
                            {
                                return (y + 2, x - 2);
                            }
                        }
                    }
                    else if (currentGame[y][x] == Players.None)
                    {
                        if (canMoveRight && currentGame[y][x + 1] != Players.None && currentGame[y][x + 1] == currentGame[y][x + 2])
                        {
                            return (y, x);
                        }
                        if (canMoveDown && currentGame[y + 1][x] != Players.None && currentGame[y + 1][x] == currentGame[y + 2][x])
                        {
                            return (y, x);
                        }
                        if (canMoveDown && canMoveRight && currentGame[y + 1][x + 1] != Players.None && currentGame[y + 1][x + 1] == currentGame[y + 2][x + 2])
                        {
                            return (y, x);
                        }
                        if (canMoveDown && canMoveLeft && currentGame[y + 1][x - 1] != Players.None && currentGame[y + 1][x - 1] == currentGame[y + 2][x - 2])
                        {
                            return (y, x);
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
