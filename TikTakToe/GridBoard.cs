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
        public GridBoardState()
        {
        }
    }

    public class GridBoardSquare : IGridSquare<GridBoardState>
    {
        public GridBoardState State { get; set; }
        public Action<IGridSquare<GridBoardState>> WasActivated => throw new NotImplementedException();
        public GridBoardSquare()
        {
            State = new GridBoardState();
        }

        public void SetState(GridBoardState targetState)
        {
            State.Owner = targetState.Owner;
        }
    }

    public class GridBoard : IGridBoard<GridBoardState, GridBoardSquare>
    {
        public int WinSize { get; set; }
        public Players PreviousPlayer { get; set; }
        public bool IsWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsLose { get; private set; }

        public bool IsTerminal => IsWin || IsTie || IsLose;

        public Players NextPlayer => nextPlayerMap[PreviousPlayer](this);

        public GridBoardSquare[][] CurrentBoard { get; set; }

        public int YLength => CurrentBoard.Length;

        public int XLength => CurrentBoard[0].Length;

        public GridBoardSquare this[int y, int x] { get => CurrentBoard[y][x]; set => CurrentBoard[y][x] = value; }

        private readonly Dictionary<Players, Func<GridBoard, Players>> nextPlayerMap;


        public GridBoard(GridBoardSquare[][] currentBoard, Players previousPlayer, int winSize, Dictionary<Players, Func<GridBoard, Players>> nextPlayerMap)
        {
            PreviousPlayer = previousPlayer;
            WinSize = winSize;
            this.nextPlayerMap = nextPlayerMap;

            SetCurrentGame(currentBoard, previousPlayer);
        }

        public void UpdateStatus()
        {
            Players winner = GetWinner();
            IsWin = winner == NextPlayer;
            IsLose = winner != Players.None && winner != NextPlayer;
            IsTie = !IsPlayable() && !IsWin && !IsLose;
        }

        public List<IGridBoard<GridBoardState, GridBoardSquare>> GetChildren()
        {
            List<IGridBoard<GridBoardState, GridBoardSquare>> children = new List<IGridBoard<GridBoardState, GridBoardSquare>>();
            if (IsTerminal)
            {
                return children;
            }

            for (int y = 0; y < CurrentBoard.Length; y++)
            {
                for (int x = 0; x < CurrentBoard[y].Length; x++)
                {
                    if (CurrentBoard[y][x].State.Owner == Players.None)
                    {
                        GridBoard nextBoard = new GridBoard(CurrentBoard, NextPlayer, WinSize, nextPlayerMap);
                        nextBoard[y, x].State.Owner = NextPlayer;
                        nextBoard.UpdateStatus();
                        children.Add(nextBoard); 
                        if(nextBoard.PreviousPlayer == Players.None || nextBoard.PreviousPlayer == Players.Player1)
                        {

                        }
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
            for (int y = 0; y < CurrentBoard.Length; y++)
            {
                for (int x = 0; x < CurrentBoard[y].Length; x++)
                {
                    bool canMoveRight = x + WinSize - 1 < CurrentBoard[y].Length;
                    bool canMoveLeft = x - WinSize + 1 >= 0;
                    bool canMoveDown = y + WinSize - 1 < CurrentBoard.Length;
                    Players currentOwner = CurrentBoard[y][x].State.Owner;
                    if (currentOwner != Players.None)
                    {
                        if (canMoveRight)
                        {
                            for (int i = 1; i < WinSize; i++)
                            {
                                if (CurrentBoard[y][x + i].State.Owner != currentOwner)
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
                                if (CurrentBoard[y + i][x].State.Owner != currentOwner)
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
                                if (CurrentBoard[y + i][x + i].State.Owner != currentOwner)
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
                                if (CurrentBoard[y + i][x - i].State.Owner != currentOwner)
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
            for (int y = 0; y < CurrentBoard.Length; y++)
            {
                for (int x = 0; x < CurrentBoard[y].Length; x++)
                {
                    if (CurrentBoard[y][x].State.Owner != targetBoard[y, x].State.Owner)
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
            foreach (GridBoardSquare[] squares in CurrentBoard)
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

        public void SetCurrentGame(GridBoardSquare[][] targetBoard, Players previousPlayer)
        {
            PreviousPlayer = previousPlayer; 

            if (CurrentBoard == null)
            {
                CurrentBoard = new GridBoardSquare[targetBoard.Length][];
                for (int y = 0; y < targetBoard.Length; y++)
                {
                    CurrentBoard[y] = new GridBoardSquare[targetBoard[y].Length];
                    for (int x = 0; x < targetBoard.Length; x++)
                    {
                        CurrentBoard[y][x] = new GridBoardSquare();
                        CurrentBoard[y][x].SetState(targetBoard[y][x].State);
                    }
                }
            }
            else
            {
                for (int y = 0; y < targetBoard.Length; y++)
                {
                    for (int x = 0; x < targetBoard.Length; x++)
                    {
                        CurrentBoard[y][x].SetState(targetBoard[y][x].State);
                    }
                }
            }
            UpdateStatus();
        }

        public IGridBoard<GridBoardState, GridBoardSquare> Clone()
        {
            return new GridBoard(CurrentBoard, PreviousPlayer, WinSize, nextPlayerMap);
        }
    }
}
