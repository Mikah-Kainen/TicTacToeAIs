using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace TikTakToe
{
    public class Pair
    {
        public Board Board { get; set; }
        public NeuralNet Net { get; set; }
        public int Success { get; set; }
        public bool IsAlive { get; set; }
        public Pair(Board board, NeuralNet net)
        {
            Board = board;
            Net = net;
            Success = 0;
            IsAlive = true;
        }
    }

    public class NeuralNetTrainer
    {

        public NeuralNetTrainer()
        {

        }

        public NeuralNet GetNet(Node<Board> completeGame, int numberOfSimulations, int numberOfGenerations, Random random)
        {
            int[] neuronsPerLayer = new int[]
            {
                completeGame.State.Length * completeGame.State[0].Length,
                4,
                3,
                4,
                completeGame.State.Length * completeGame.State[0].Length,
            };
            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                NeuralNet pairNet = new NeuralNet(ErrorFunctions.MeanSquared, ActivationFunctions.BinaryStep, neuronsPerLayer);
                pairNet.Randomize(random, -1, 1);
                pairs.Add(new Pair(completeGame.State, pairNet));
            }
            NeuralNet best = null;
            for(int i = 0; i < numberOfGenerations; i ++)
            {
                best = Train(pairs, random, 10, 10, 0.5f, 1.5f, -1, 1);
            }
            return best;
        }
        int correctCount = 0;

        private NeuralNet Train(List<Pair> pairs, Random random, double preservePercent, double randomizePercent, double mutationMin, double mutationMax, double randomizeMin, double randomizeMax)
            //preservePercent => percent of population to save, randomizePercent => percent of population to randomize, mutationRange => multiply mutations by a random value between positive and negative mutationRange
        {
            bool IsThereBoardAlive = true;
            while (IsThereBoardAlive)
            {
                for (int i = 0; i < pairs.Count; i++)
                {
                    IsThereBoardAlive = MakeMove(pairs[i]);
                }
            }
            pairs = pairs.OrderByDescending<Pair, int>((Pair current) => current.Success).ToList();

            int preserveCutoff = (int)(pairs.Count * preservePercent / 100);
            int randomizeCutoff = (int)(pairs.Count * (100 - randomizePercent) / 100);
            for(int i = 0; i < preserveCutoff; i ++)
            {
                pairs[i].IsAlive = true;
                //pairs[i].Success = 0;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }
            for(int i = preserveCutoff; i < randomizeCutoff; i ++)
            {
                int parent = random.Next(0, preserveCutoff);
                pairs[i].Net.Cross(pairs[parent].Net, random);
                pairs[i].Net.Mutate(random, mutationMin, mutationMax);
                pairs[i].IsAlive = true;
                //pairs[i].Success = 0;
                if(pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }
            for(int i = randomizeCutoff; i < pairs.Count; i ++)
            {
                pairs[i].Net.Randomize(random, randomizeMin, randomizeMax);
                pairs[i].IsAlive = true;
                //pairs[i].Success = 0;
                if (pairs[i].Net.Layers[3].Neurons[0].Bias != 0)
                {

                }
            }

            return pairs[0].Net;
        }

        private bool MakeMove(Pair currentPair)
        {
            bool returnValue = false;
            if (currentPair.IsAlive)
            {
                //currentPair.Success++;
                int yLength = currentPair.Board.Length;
                int xLength = currentPair.Board[0].Length;
                double[] inputs = new double[yLength * xLength];
                for (int y = 0; y < yLength; y++)
                {
                    for (int x = 0; x < xLength; x++)
                    {
                        switch (currentPair.Board[y][x])
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
                double[] computedValues = currentPair.Net.Compute(inputs);
                int target = -1;
                for (int a = 0; a < computedValues.Length; a++)
                {
                    if (computedValues[a] == 1)
                    {
                        if (target != -1)
                        {
                            currentPair.IsAlive = false;
                            goto deathZone;
                        }
                        target = a;
                    }
                }
                if (target == -1)
                {
                    currentPair.IsAlive = false;
                    goto deathZone;
                }
                int yVal = target / yLength;
                int xVal = target % xLength;
                if (currentPair.Board[yVal][xVal] != Players.None)
                {
                    currentPair.IsAlive = false;
                    goto deathZone;
                }
                List<Node<Board>> children = currentPair.Board.GetChildren();
                for (int z = 0; z < children.Count; z++)
                {
                    if (children[z].State[yVal][xVal] == currentPair.Board.NextPlayer)
                    {
                        currentPair.Board = children[z].State;
                        currentPair.Success++;
                    }
                }
                if (currentPair.Board.IsTerminal == true)
                {
                    currentPair.Success = 10000;
                    currentPair.IsAlive = false;
                }
                else
                {
                    returnValue = true;
                }
                currentPair.Success++;
                correctCount++;
                deathZone:;
            }
            return returnValue;
        }
    }
}
