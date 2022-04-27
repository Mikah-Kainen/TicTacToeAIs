using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.GBVPlayerTypes
{
    public class GBVOverfitPlayer : GBVPlayer
    {
        public GBVOverfitPlayer(Players playerID)
            :base(playerID)
        {

        }

        public override (int y, int x) SelectTile(GridBoard currentTree)
        {
            for(int y = 0; y < currentTree.YLength; y ++)
            {
                for(int x = 0; x < currentTree.XLength; x ++)
                {
                    if(currentTree[y, x].State.Owner == Players.None)
                    {
                        return (y, x);
                    }
                }
            }
            throw new Exception("This should not happen exception");
            return (-1, -1);
        }
    }
}
