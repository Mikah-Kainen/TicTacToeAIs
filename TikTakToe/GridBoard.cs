using NeuralNetwork.TurnBasedBoardGameTrainerStuff;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public class GridBoardState : INetInputer
    {
        public double[] NetInputs { get; set; }
    }

    public class GridBoardSquare : IGridSquare<GridBoardState>
    {
        public GridBoardState State { get; set; }
        public Enums.Players Owner { get; set; }
        public Action<IGridSquare<GridBoardState>> WasActivated => throw new NotImplementedException();
    }

    public class GridBoard<TSquare> : IGridBoard<GridBoardState, TSquare>
        where TSquare : IGridSquare<GridBoardState>
    {
        public int WinSize { get; set; }
        public Enums.Players PreviousPlayer { get; set; }
        public bool IsWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsLose { get; private set; }

        public bool IsTerminal { get; private set; }

        public Enums.Players NextPlayer => nextPlayerMap[PreviousPlayer](this);

        public TSquare[][] CurrentGame { get; set; }
        public int YLength => CurrentGame.Length;

        public int XLength => CurrentGame[0].Length;

        public TSquare this[int y, int x] { get => CurrentGame[y][x]; set => CurrentGame[y][x] = value; }

        private readonly Dictionary<Enums.Players, Func<IGridBoard<GridBoardState, TSquare>, Enums.Players>> nextPlayerMap;


        public GridBoard(TSquare[][] currentGame, int winSize, Dictionary<Enums.Players, Func<IGridBoard<GridBoardState, TSquare>, Enums.Players>> getNextPLayer)
        {
            WinSize = winSize;
            CurrentGame = new TSquare[currentGame.Length][];
            for (int y = 0; y < currentGame.Length; y++)
            {
                CurrentGame[y] = new TSquare[currentGame[y].Length];
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    CurrentGame[y][x] = currentGame[y][x];
                }
            }
            nextPlayerMap = getNextPLayer;
        }

        List<IGridBoard<GridBoardState, TSquare>> IGridBoard<GridBoardState, TSquare>.GetChildren()
        {
            List<IGridBoard<GridBoardState, TSquare>> children = new List<IGridBoard<GridBoardState, TSquare>>();
            if (IsTerminal) return children;

            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x].Owner == Enums.Players.None)
                    {
                        GridBoard<TSquare> nextBoard = new GridBoard<TSquare>(CurrentGame, WinSize, nextPlayerMap);
                        nextBoard[y, x].Owner = NextPlayer;
                        Enums.Players winner = nextBoard.GetWinner();
                        nextBoard.IsWin = winner == ((IGridBoard<GridBoardState, TSquare>)this).NextPlayer;
                        nextBoard.IsLose = winner != Enums.Players.None && winner != NextPlayer;
                        nextBoard.IsTie = !nextBoard.IsPlayable() && !nextBoard.IsWin && !nextBoard.IsLose;
                        nextBoard.IsTerminal = nextBoard.IsWin || nextBoard.IsTie || nextBoard.IsLose;
                        children.Add(nextBoard);
                    }
                }
            }
            return children;
        }


        public Enums.Players GetWinner()
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    bool canMoveRight = x + WinSize - 1 < CurrentGame[y].Length;
                    bool canMoveLeft = x - WinSize + 1 >= 0;
                    bool canMoveDown = y + WinSize - 1 < CurrentGame.Length;
                    Enums.Players currentOwner = CurrentGame[y][x].Owner;
                    if (currentOwner != Enums.Players.None)
                    {
                        if (canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y][x + i].Owner != currentOwner)
                                {
                                    goto end1;
                                }
                            }
                            return currentOwner;
                        }
                    end1:
                        if (canMoveDown)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x].Owner != currentOwner)
                                {
                                    goto end2;
                                }
                            }
                            return currentOwner;
                        }
                    end2:
                        if (canMoveDown && canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x + i].Owner != currentOwner)
                                {
                                    goto end3;
                                }
                            }
                            return currentOwner;
                        }
                    end3:
                        if (canMoveDown && canMoveLeft)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x - i].Owner != currentOwner)
                                {
                                    goto end4;
                                }
                            }
                            return currentOwner;
                        }
                    end4:
                        if (true) { }
                    }
                }
            }
            return Enums.Players.None;
        }

        public (int y, int x) FindDifference(GridBoard<TSquare> targetBoard)
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x].Owner != targetBoard[y, x].Owner)
                    {
                        return (y, x);
                    }
                }
            }
            throw new Exception("This shouldn't happen");
            //return (0, 0);
        }


        public bool IsPlayable()
        {
            foreach (TSquare[] squares in CurrentGame)
            {
                foreach (TSquare square in squares)
                {
                    if (square.Owner == Enums.Players.None)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
