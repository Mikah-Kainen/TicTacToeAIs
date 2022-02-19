using NeuralNetwork.TurnBasedBoardGameTrainerStuff;
using NeuralNetwork.TurnBasedBoardGameTrainerStuff.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public class GridBoardState : INetInputer
    {
        public double[] NetInputs { get; set; }
        public Players Owner { get; set; }
    }

    public class GridBoardSquare : IGridSquare<GridBoardState>
    {
        public GridBoardState State { get; set; }
        public Action<IGridSquare<GridBoardState>> WasActivated => throw new NotImplementedException();
        public GridBoardSquare()
        {
            State = new GridBoardState();
        }
    }

    public class GridBoard : IGridBoard<GridBoardState, GridBoardSquare>
    {
        public int WinSize { get; set; }
        public Players PreviousPlayer { get; set; }
        public bool IsWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsLose { get; private set; }

        public bool IsTerminal { get; private set; }

        public Players NextPlayer => nextPlayerMap[PreviousPlayer](this);

        public GridBoardSquare[][] CurrentGame { get; set; }

        public int YLength => CurrentGame.Length;

        public int XLength => CurrentGame[0].Length;

        public GridBoardSquare this[int y, int x] { get => CurrentGame[y][x]; set => CurrentGame[y][x] = value; }

        private readonly Dictionary<Players, Func<GridBoard, Players>> nextPlayerMap;


        public GridBoard(GridBoardSquare[][] currentGame, Players previousPlayer, int winSize, Dictionary<Players, Func<GridBoard, Players>> nextPlayerMap)
        {
            PreviousPlayer = previousPlayer;
            WinSize = winSize;
            CurrentGame = currentGame;
            this.nextPlayerMap = nextPlayerMap;

            Players winner = GetWinner();
            IsWin = winner == NextPlayer;
            IsLose = winner != Players.None && winner != NextPlayer;
            IsTie = !IsPlayable() && !IsWin && !IsLose;
            IsTerminal = IsWin || IsLose || IsTie;
        }

        public List<IGridBoard<GridBoardState, GridBoardSquare>> GetChildren()
        {
            List<IGridBoard<GridBoardState, GridBoardSquare>> children = new List<IGridBoard<GridBoardState, GridBoardSquare>>();
            if (IsTerminal) return children;

            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x].State.Owner == Players.None)
                    {
                        GridBoard nextBoard = new GridBoard(CurrentGame, NextPlayer, WinSize, nextPlayerMap);
                        nextBoard[y, x].State.Owner = NextPlayer;
                        children.Add(nextBoard);
                    }
                }
            }
            return children;
        }

        public static GridBoardSquare[][] CreateNewGridSquares(int ylength, int xlength)
        {
            GridBoardSquare[][] returnValue = new GridBoardSquare[ylength][];
            for (int y = 0; y < ylength; y++)
            {
                returnValue[y] = new GridBoardSquare[xlength];
                for (int x = 0; x < xlength; x++)
                {
                    returnValue[y][x] = new GridBoardSquare();
                    returnValue[y][x].State.Owner = Players.None;
                }
            }
            return returnValue;
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
                    Players currentOwner = CurrentGame[y][x].State.Owner;
                    if (currentOwner != Players.None)
                    {
                        if (canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentGame[y][x + i].State.Owner != currentOwner)
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
                                if (CurrentGame[y + i][x].State.Owner != currentOwner)
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
                                if (CurrentGame[y + i][x + i].State.Owner != currentOwner)
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
                                if (CurrentGame[y + i][x - i].State.Owner != currentOwner)
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
            return Players.None;
        }

        public (int y, int x) FindDifference(GridBoard targetBoard)
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    if (CurrentGame[y][x].State.Owner != targetBoard[y, x].State.Owner)
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
                    if (square.State.Owner == Players.None)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
