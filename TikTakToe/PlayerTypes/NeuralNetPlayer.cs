using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    public class NeuralNetPlayer : Player
    {
        private Random random;
        public NeuralNet Net { get; set; }

        public NeuralNetPlayer(Players playerID, NeuralNet net, Random random)
        : base(playerID)
        {
            Net = net;
        }

        public override (int y, int x) SelectTile(Node<Board> CurrentTree)
        {
            int yLength = CurrentTree.State.Length;
            int xLength = CurrentTree.State[0].Length;
            double[] inputs = new double[yLength * xLength];
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    switch (CurrentTree.State[y][x])
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
            }
            int yVal = target / yLength;
            int xVal = target % xLength;
            if (CurrentTree.State[yVal][xVal] != Players.None)
            {
            }
            return (yVal, xVal);
        }
    }
}
