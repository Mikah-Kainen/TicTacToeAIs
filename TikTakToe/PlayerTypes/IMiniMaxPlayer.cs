using System;
using System.Collections.Generic;
using System.Text;

using static NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

namespace TikTakToe.PlayerTypes
{
    interface IMiniMaxPlayer
    {
        public Dictionary<int, Dictionary<Players, int>> GetPlayerValue { get; set; }
        void SetValues(Node<Board> currentTree);
    }
}
