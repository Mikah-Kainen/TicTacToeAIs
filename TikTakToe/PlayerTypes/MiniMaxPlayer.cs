using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikTakToe.PlayerTypes
{
    public class MiniMaxPlayer : Player
    {
        private Random random;
        public MiniMaxPlayer(Players playerID, Random random)
            : base(playerID)
        {
            this.random = random;
        }

        public override (int y, int x) SelectTile(Node<Board> currentTree)
        {
            return (1, 1);
        }

    }
}
