using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    interface IMiniMaxPlayer
    {
        public Dictionary<Node<Board>, Dictionary<Players, int>> GetPlayerValue { get; set; }
        void SetValues(Node<Board> currentTree);
    }
}
