using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.GBVPlayerTypes
{
    public class GBVRandomPlayer : GBVPlayer
    {
        Random random;

        public GBVRandomPlayer(Players playerID, Random random)
            :base(playerID)
        {
            this.random = random;
        }

        public override (int y, int x) SelectTile(GridBoard currentTree)
        {
            return currentTree.RandomMove(random);
        }
    }
}
