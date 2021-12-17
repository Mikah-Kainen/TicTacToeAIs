using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public class NeuralNetTrainer
    {

        public NeuralNetTrainer()
        {
            
        }

        public NeuralNet GetNet(Node<Board> completeGame, int numberOfSimulations, Random random)
        {
            int[] neuronsPerLayer = new int[]
            {
                completeGame.State.Length * completeGame.State[0].Length,
                4,
                3,
                4,
                completeGame.State.Length * completeGame.State[0].Length,
            };
            List<Board> boards = new List<Board>();
            List<NeuralNet> nets = new List<NeuralNet>();
            for (int i = 0; i < numberOfSimulations; i ++)
            {
                boards[i] = completeGame.State;
                nets[i] = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.Tanh, neuronsPerLayer);
            }
            return Train(completeGame, boards, nets);
        }

        private NeuralNet Train(Node<Board> completeGame, List<Board> boards, List<NeuralNet> nets)
        {
            return nets[0];
        }
    }
}
