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

        public Func<IGridSquare<GridBoardState>, GridBoardState> WasActivated => throw new NotImplementedException();
    }

    public class GridBoard : IGridBoard<INetInputer>
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

        Enums.Players IGridBoard<INetInputer>.NextPlayer => throw new NotImplementedException();

        public IGridSquare<INetInputer> this[int y, int x] { get => (IGridSquare<INetInputer>)CurrentGame[y][x]; set => (IGridSquare<INetInputer>)CurrentGame[y][x] = value; }

        public List<GridBoard> GetChildren()
        {
            List<GridBoard> children = new List<GridBoard>();
            if (IsTerminal) return children;

            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x] == Players.None)
                    {
                        Board nextBoard = new Board(CurrentGame, NextPlayer, WinSize, GetNextPlayer);
                        nextBoard[y][x] = NextPlayer;
                        Players winner = nextBoard.GetWinner();
                        nextBoard.IsWin = winner != Players.None && winner == nextBoard.PreviousPlayer;
                        nextBoard.IsLose = winner != Players.None && winner != nextBoard.PreviousPlayer;
                        nextBoard.IsTie = !nextBoard.IsPlayable() && !nextBoard.IsWin && !nextBoard.IsLose;
                        nextBoard.IsTerminal = nextBoard.IsWin || nextBoard.IsTie || nextBoard.IsLose;

                        Node<Board> currentChild = new Node<Board>();
                        currentChild.State = nextBoard;
                        children.Add(currentChild);
                    }
                }
            }
            return children;
        }

        public Dictionary<Players, Func<IGameState<Board>, Players>> GetNextPlayer;

        public Board(Players[][] currentGame, Players previousPlayer, int winSize, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        {
            WinSize = winSize;
            CurrentGame = new Players[currentGame.Length][];
            for (int y = 0; y < currentGame.Length; y++)
            {
                CurrentGame[y] = new Players[currentGame[y].Length];
                for (int x = 0; x < currentGame[y].Length; x++)
                {
                    CurrentGame[y][x] = currentGame[y][x];
                }
            }
            PreviousPlayer = previousPlayer;
            GetNextPlayer = getNextPLayer;
        }

        public Board(int xSize, int ySize, int winSize, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        {
            WinSize = winSize;
            GetNextPlayer = getNextPLayer;
            CurrentGame = new Players[ySize][];

            for (int y = 0; y < ySize; y++)
            {
                CurrentGame[y] = new Players[xSize];
                for (int x = 0; x < xSize; x++)
                {
                    CurrentGame[y][x] = Players.None;
                }
            }

            PreviousPlayer = Players.None;
        }

        public Players[] this[int index]
        {
            get
            {
                return CurrentGame[index];
            }
            set
            {
                CurrentGame[index] = value;
            }
        }

        public Players GetWinner()
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    bool canMoveRight = x + WinSize - 1 < CurrentGame[y].Length;
                    bool canMoveLeft = x - WinSize + 1 >= 0;
                    bool canMoveDown = y + WinSize - 1 < CurrentGame.Length;
                    Players currentPlayer = CurrentGame[y][x];
                    if (currentPlayer != Players.None)
                    {
                        if (canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y][x + i] != currentPlayer)
                                {
                                    goto end1;
                                }
                            }
                            return currentPlayer;
                        }
                    end1:
                        if (canMoveDown)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x] != currentPlayer)
                                {
                                    goto end2;
                                }
                            }
                            return currentPlayer;
                        }
                    end2:
                        if (canMoveDown && canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x + i] != currentPlayer)
                                {
                                    goto end3;
                                }
                            }
                            return currentPlayer;
                        }
                    end3:
                        if (canMoveDown && canMoveLeft)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y + i][x - i] != currentPlayer)
                                {
                                    goto end4;
                                }
                            }
                            return currentPlayer;
                        }
                    end4:
                        if (true) { }
                    }
                }
            }
            return Players.None;
        }

        public (int y, int x) FindDifference(Board targetBoard)
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x] != targetBoard[y][x])
                    {
                        return (y, x);
                    }
                }
            }
            throw new Exception("does this ever happen");
            //return (0, 0);
        }


        public bool IsPlayable()
        {
            foreach (Players[] players in CurrentGame)
            {
                foreach (Players player in players)
                {
                    if (player == Players.None)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        List<IGridBoard<INetInputer>> IGridBoard<INetInputer>.GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}

    }
}
