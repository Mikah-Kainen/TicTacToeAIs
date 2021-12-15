using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public static class Extensions
    {
        public static Texture2D CreatePixel(this Color color, GraphicsDeviceManager graphics)
        {
            Texture2D returnVal = new Texture2D(graphics.GraphicsDevice, 1, 1);
            returnVal.SetData(new Color[] { color}, 0, 1);
            return returnVal;
        }

        public static int Print<T> (this T board)
            where T : IGameState<T>
        {
            int returnValue = 0;
            Board currentBoard = board as Board;
            if(currentBoard == null)
            {
                return returnValue;
            }

            for(int y = 0; y < currentBoard.Length; y ++)
            {
                for(int x = 0; x < currentBoard[y].Length; x ++)
                {
                    switch(currentBoard[y][x])
                    {
                        case Players.None:
                            returnValue *= 100;
                            returnValue += 00;
                            break;

                        case Players.Player1:
                            returnValue *= 100;
                            returnValue += 01;
                            break;

                        case Players.Player2:
                            returnValue *= 100;
                            returnValue += 10;
                            break;

                        case Players.Player3:
                            returnValue *= 100;
                            returnValue += 11;
                            break;
                    }
                }
            }
            return returnValue;
        }

        public static (int, int) RandomMove(this Node<Board> currentGame, Random random)
        {
            Board currentBoard = currentGame.State;
            int y = currentBoard.Length;
            int x = currentBoard[0].Length;

            int returnY = random.Next(0, y);
            int returnX = random.Next(0, x);

            while(currentBoard[returnY][returnX] != Players.None)
            {
                returnX++;
                if(returnX >= x)
                {
                    returnX = 0;
                    returnY++;
                    if(returnY >= y)
                    {
                        returnY = 0;
                    }
                }
            }

            return (returnY, returnX);
        }
    }

}
