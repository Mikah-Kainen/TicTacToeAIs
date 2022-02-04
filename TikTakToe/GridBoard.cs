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
        public GridBoardSquare[][] CurrentGame { get; set; }
        public int YLength => CurrentGame.Length;
        public Players PreviousPlayer { get; set; }
        //public Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        public bool IsWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsLose { get; private set; }

        public bool IsTerminal { get; private set; }

        public Enums.Players NextPlayer => throw new NotImplementedException();

        TSquare[][] IGridBoard<GridBoardState, TSquare>.CurrentGame => throw new NotImplementedException();

        TSquare IGridBoard<GridBoardState, TSquare>.this[int y, int x] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GridBoardSquare this[int y, int x] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        List<IGridBoard<GridBoardState, TSquare>> IGridBoard<GridBoardState, TSquare>.GetChildren()
        {
            List<IGridBoard<GridBoardState, TSquare>> children = new List<IGridBoard<GridBoardState, TSquare>>();
            if (IsTerminal) return children;

            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    //if (CurrentGame[y][x] == Players.None)
                    //{
                    //    Board nextBoard = new Board(CurrentGame, NextPlayer, WinSize, GetNextPlayer);
                    //    nextBoard[y][x] = NextPlayer;
                    //    Players winner = nextBoard.GetWinner();
                    //    nextBoard.IsWin = winner != Players.None && winner == nextBoard.PreviousPlayer;
                    //    nextBoard.IsLose = winner != Players.None && winner != nextBoard.PreviousPlayer;
                    //    nextBoard.IsTie = !nextBoard.IsPlayable() && !nextBoard.IsWin && !nextBoard.IsLose;
                    //    nextBoard.IsTerminal = nextBoard.IsWin || nextBoard.IsTie || nextBoard.IsLose;

                    //    Node<Board> currentChild = new Node<Board>();
                    //    currentChild.State = nextBoard;
                    //    children.Add(currentChild);
                    //}
                }
            }
            return children;
        }

        public List<GridBoard<TSquare>> GetChildren()
        {
            List<GridBoard<TSquare>> children = new List<GridBoard<TSquare>>();
            if (IsTerminal) return children;

            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    //if (CurrentGame[y][x] == Players.None)
                    //{
                    //    Board nextBoard = new Board(CurrentGame, NextPlayer, WinSize, GetNextPlayer);
                    //    nextBoard[y][x] = NextPlayer;
                    //    Players winner = nextBoard.GetWinner();
                    //    nextBoard.IsWin = winner != Players.None && winner == nextBoard.PreviousPlayer;
                    //    nextBoard.IsLose = winner != Players.None && winner != nextBoard.PreviousPlayer;
                    //    nextBoard.IsTie = !nextBoard.IsPlayable() && !nextBoard.IsWin && !nextBoard.IsLose;
                    //    nextBoard.IsTerminal = nextBoard.IsWin || nextBoard.IsTie || nextBoard.IsLose;

                    //    Node<Board> currentChild = new Node<Board>();
                    //    currentChild.State = nextBoard;
                    //    children.Add(currentChild);
                    //}
                }
            }
            return children;
        }

        public Dictionary<Players, Func<IGameState<Board>, Players>> GetNextPlayer;

        //public Board(Players[][] currentGame, Players previousPlayer, int winSize, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        //{
        //    WinSize = winSize;
        //    CurrentGame = new Players[currentGame.Length][];
        //    for (int y = 0; y < currentGame.Length; y++)
        //    {
        //        CurrentGame[y] = new Players[currentGame[y].Length];
        //        for (int x = 0; x < currentGame[y].Length; x++)
        //        {
        //            CurrentGame[y][x] = currentGame[y][x];
        //        }
        //    }
        //    PreviousPlayer = previousPlayer;
        //    GetNextPlayer = getNextPLayer;
        //}

        //public Board(int xSize, int ySize, int winSize, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        //{
        //    WinSize = winSize;
        //    GetNextPlayer = getNextPLayer;
        //    CurrentGame = new Players[ySize][];

        //    for (int y = 0; y < ySize; y++)
        //    {
        //        CurrentGame[y] = new Players[xSize];
        //        for (int x = 0; x < xSize; x++)
        //        {
        //            CurrentGame[y][x] = Players.None;
        //        }
        //    }

        //    PreviousPlayer = Players.None;
        //}

        //public Players[] this[int index]
        //{
        //    get
        //    {
        //        return CurrentGame[index];
        //    }
        //    set
        //    {
        //        CurrentGame[index] = value;
        //    }
        //}

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
            foreach (GridBoardSquare[] squares in CurrentGame)
            {
                foreach (GridBoardSquare square in squares)
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
