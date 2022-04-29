using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff;

namespace TikTakToe.GBVPlayerTypes
{
    class GBVNeuralNetPlayer : GBVPlayer
    {
        private Random random;
        public NeuralNet Net { get; set; }

        public GBVNeuralNetPlayer(Players playerID, NeuralNet net, Random random)
        : base(playerID)
        {
            Net = net;
        }

        public override (int y, int x) SelectTile(GridBoard currentGame)
        {
            BoardNetPair<GridBoardState, GridBoardSquare> currentPair = new BoardNetPair<GridBoardState, GridBoardSquare>(currentGame, Net);
            return currentPair.BoardNetPairToNeuralNetOutput();
        }
    }
}
