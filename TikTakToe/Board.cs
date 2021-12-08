using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe
{
    public class Board : IGameState<Board>
    {
        public int WinSize { get; set; }
        public Players[][] CurrentGame;
        public int Length => CurrentGame.Length;
        public Players PreviousPlayer { get; set; }
        public Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        public bool IsWin { get; private set; }

        public bool IsTie { get; private set; }

        public bool IsLose { get; private set; }

        public bool IsTerminal { get; private set; }

        public List<Node<Board>> GetChildren()
        {
            List<Node<Board>> children = new List<Node<Board>>();
            if (IsTerminal) return children;

            for(int y = 0; y < CurrentGame.Length; y ++)
            {
                for(int x = 0; x < CurrentGame[y].Length; x ++)
                {
                    if(CurrentGame[y][x] == Players.None)
                    {
                        Board nextBoard = new Board(CurrentGame, NextPlayer, GetNextPlayer);
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

        public Board(Players[][] currentGame, Players previousPlayer, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        {
            CurrentGame = new Players[currentGame.Length][];
            for(int y = 0; y < currentGame.Length; y ++)
            {
                CurrentGame[y] = new Players[currentGame[y].Length];
                for(int x = 0; x < currentGame[y].Length; x ++)
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
                    int loopFactor = WinSize - 1;
                    bool canMoveRight = x + loopFactor < CurrentGame[y].Length;
                    bool canMoveLeft = x - loopFactor >= 0;
                    bool canMoveDown = y + loopFactor < CurrentGame.Length;
                    Players currentPlayer = CurrentGame[y][x];
                    if(currentPlayer != Players.None)
                    { 
                        if (canMoveRight)
                        {
                            bool looping = true;
                            for (int i = 1; i < loopFactor; i ++)
                            {
                                looping = CurrentGame[y][x + i] == currentPlayer;
                                //This doesn't work
                            }
                            if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                            else if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                        }
                        if (canMoveDown)
                        {
                            if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                            else if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                            else if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                            else if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
                            {
                                return currentPlayer;
                            }
                        }
                    }
                }
            }
            return Players.None;
        }

        public (int y, int x) FindDifference(Board targetBoard)
        {
            for(int y = 0; y < CurrentGame.Length; y ++)
            {
                for(int x = 0; x < CurrentGame[y].Length; x ++)
                {
                    if(CurrentGame[y][x] != targetBoard[y][x])
                    {
                        return (y, x);
                    }
                }
            }
            return (0, 0);
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
    }
}
