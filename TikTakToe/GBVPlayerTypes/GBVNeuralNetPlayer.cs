using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

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
            int yLength = currentGame.YLength;
            int xLength = currentGame.XLength;
            double[] inputs = new double[yLength * xLength];
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    switch (currentGame[y, x].State.Owner)
                    {
                        case Players.None:
                            inputs[y * yLength + x] = 0;
                            break;

                        case Players.Player1:
                            inputs[y * yLength + x] = 1;
                            break;

                        case Players.Player2:
                            inputs[y * yLength + x] = 2;
                            break;

                        case Players.Player3:
                            inputs[y * yLength + x] = 3;
                            break;
                    }
                }
            }
            double[] computedValues = Net.Compute(inputs);
            int target = -1;
            for (int a = 0; a < computedValues.Length; a++)
            {
                if (computedValues[a] == 1)
                {
                    if (target != -1)
                    {
                    }
                    target = a;
                }
            }
            if (target == -1)
            {
                throw new Exception("No Move Found");
            }
            int yVal = target / yLength;
            int xVal = target % xLength;
            if (currentGame[yVal, xVal].State.Owner != Players.None)
            {
            }
            return (yVal, xVal);
        }
    }
}
