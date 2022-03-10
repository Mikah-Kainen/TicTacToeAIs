using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

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

        //public static int Print<T> (this T board)
        //    where T : IGameState<T>
        //{
        //    int returnValue = 0;
        //    Board currentBoard = board as Board;
        //    if(currentBoard == null)
        //    {
        //        return returnValue;
        //    }

        //    for(int y = 0; y < currentBoard.Length; y ++)
        //    {
        //        for(int x = 0; x < currentBoard[y].Length; x ++)
        //        {
        //            switch(currentBoard[y][x])
        //            {
        //                case Players.None:
        //                    returnValue *= 100;
        //                    returnValue += 00;
        //                    break;

        //                case Players.Player1:
        //                    returnValue *= 100;
        //                    returnValue += 01;
        //                    break;

        //                case Players.Player2:
        //                    returnValue *= 100;
        //                    returnValue += 10;
        //                    break;

        //                case Players.Player3:
        //                    returnValue *= 100;
        //                    returnValue += 11;
        //                    break;
        //            }
        //        }
        //    }
        //    return returnValue;
        //}

        public static (int, int) RandomMove(this GridBoard currentGame, Random random)
        {
            var currentBoard = currentGame;

            int returnY = random.Next(0, currentBoard.YLength);
            int returnX = random.Next(0, currentBoard.XLength);

            while (currentBoard[returnY, returnX].State.Owner != Players.None)
            {
                returnX++;
                if (returnX >= currentBoard.XLength)
                {
                    returnX = 0;
                    returnY++;
                    if (returnY >= currentBoard.YLength)
                    {
                        returnY = 0;
                    }
                }
            }

            return (returnY, returnX);
        }

        public static GridBoard FindChild(this(int y, int x) selectedSquare, Players selectingPlayer, GridBoard currentBoard)
        {
            var children = currentBoard.GetChildren();
            for(int i = 0; i < children.Count; i ++)
            {
                if(children[i][selectedSquare.y, selectedSquare.x].State.Owner == selectingPlayer)
                {
                    return (GridBoard)children[i];
                }
            }
            throw new Exception("Child Not Found");
        }
    }

}
