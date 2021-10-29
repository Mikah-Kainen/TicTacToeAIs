using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.Players
{
    public class BasicPlayer : Player
    {

        public BasicPlayer(Color playerColor)
            :base(playerColor)
        {

        }

        public override Sprite SelectTile(Sprite[][] currentGame)
        {
            for(int y = 0; y < currentGame.Length; y ++)
            {
                for(int x = 0; x < currentGame[y].Length; x ++)
                {
                    if(currentGame[y][x].Tint == PlayerColor)
                    {
                        bool canMoveRight = currentGame[y].Length < x + 2;
                        bool canMoveLeft = x - 2 >= 0;
                        bool canMoveDown = y + 2 < currentGame.Length;
                        if (canMoveRight)
                        {
                            if(currentGame[y][x+1].Tint == Color.White && currentGame[y][x+2].Tint == PlayerColor)
                            {
                                return currentGame[y][x + 1];
                            }
                            else if(currentGame[y][x+1].Tint == PlayerColor && currentGame[y][x+2].Tint == Color.White)
                            {
                                return currentGame[y][x + 2];
                            }
                        }
                        if(canMoveDown)
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
                        if(canMoveDown && canMoveRight)
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
                        if(canMoveDown && canMoveLeft)
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
                }
            }

            throw new Exception("Found Nothing");
            return null;
        }
    }
}
