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

        public override (int y, int x) SelectTile(Tree currentTree)
        {
            currentTree.Value;
        }

        public void CreateTree(Board CurrentState)
        {
            Tree.Value = CurrentState;
            var buildTree = Tree.Children;
        }
    }
}
