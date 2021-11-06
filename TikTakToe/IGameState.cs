using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public interface IGameState<T> where T : IGameState<T>
    {
        public bool IsWin { get; }
        public bool IsTie { get; }
        public bool IsLose { get; }
        public bool IsTerminal { get; }

        public T[] GetChildren();

    }
}
