using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    public class NeuralNetPlayer : Player
    {
        private Random random;
        NeuralNet Net;

        public NeuralNetPlayer(Players playerID, NeuralNet net, Random random)
        :base(playerID)
        {
            Net = net;
        }

        public override (int y, int x) SelectTile(Node<Board> CurrentTree)
        {
            return (0, 0);
        }
    }
}
