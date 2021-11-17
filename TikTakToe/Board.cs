using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe
{
    public class Board : IGameState<Board>
    {
        public Players[][] CurrentGame;
        public int Length => CurrentGame.Length;
        public Players PreviousPlayer { get; set; }
        public Players NextPlayer => GetNextPlayer[PreviousPlayer](this);
        public bool IsWin => GetWinner() != Players.None && GetWinner() == PreviousPlayer;

        public bool IsTie => !IsPlayable() && !IsWin && !IsLose;

        public bool IsLose => GetWinner() != Players.None && GetWinner() != PreviousPlayer;

        public bool IsTerminal => IsWin || IsTie || IsLose;

        public Board[] GetChildren()
        {
            List<Board> Children = new List<Board>();
            if (IsTerminal) return Children.ToArray();

            for(int y = 0; y < CurrentGame.Length; y ++)
            {
                for(int x = 0; x < CurrentGame[y].Length; x ++)
                {
                    if(CurrentGame[y][x] == Players.None)
                    {
                        Board nextBoard = new Board(CurrentGame, NextPlayer, GetNextPlayer);
                        nextBoard[y][x] = NextPlayer;
                        Children.Add(nextBoard);
                    }
                }
            }
            return Children.ToArray();
        }

        public Dictionary<Players, Func<IGameState<Board>, Players>> GetNextPlayer;

        public Board(Players[][] currentGame, Players previousPlayer, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        {
            CurrentGame = new Players[currentGame.Length][];
            for(int y = 0; y < currentGame.Length; y ++)
            {
                CurrentGame[y] = new Players[currentGame[y].Length];
                for(int x = 0; x < currentGame.Length; x ++)
                {
                    CurrentGame[y][x] = currentGame[y][x];
                }
            }
            PreviousPlayer = previousPlayer;
            GetNextPlayer = getNextPLayer;
        }

        public Board(int xSize, int ySize, Dictionary<Players, Func<IGameState<Board>, Players>> getNextPLayer)
        {
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
                    bool canMoveRight = x + 2 < CurrentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < CurrentGame.Length;
                    Players currentPlayer = CurrentGame[y][x];
                    if(currentPlayer != Players.None)
                    { 
                        if (canMoveRight)
                        {
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

        //public bool DidPlayerWin(Players currentPlayer)
        //{
        //    for (int y = 0; y < CurrentGame.Length; y++)
        //    {
        //        for (int x = 0; x < CurrentGame[y].Length; x++)
        //        {
        //            bool canMoveRight = x + 2 < CurrentGame[y].Length;
        //            bool canMoveLeft = x - 2 >= 0;
        //            bool canMoveDown = y + 2 < CurrentGame.Length;
        //            if (CurrentGame[y][x] == currentPlayer)
        //            {
        //                if (canMoveRight)
        //                {
        //                    if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown)
        //                {
        //                    if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown && canMoveRight)
        //                {
        //                    if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown && canMoveLeft)
        //                {
        //                    if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public bool DidPlayerLose(Players currentPlayer)
        //{
        //    for (int y = 0; y < CurrentGame.Length; y++)
        //    {
        //        for (int x = 0; x < CurrentGame[y].Length; x++)
        //        {
        //            bool canMoveRight = x + 2 < CurrentGame[y].Length;
        //            bool canMoveLeft = x - 2 >= 0;
        //            bool canMoveDown = y + 2 < CurrentGame.Length;
        //            if (CurrentGame[y][x] != currentPlayer && CurrentGame[y][x] != Players.None)
        //            {
        //                Players enemyPlayer = CurrentGame[y][x];
        //                if (canMoveRight)
        //                {
        //                    if (CurrentGame[y][x + 1] == enemyPlayer && CurrentGame[y][x + 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y][x + 1] == enemyPlayer && CurrentGame[y][x + 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown)
        //                {
        //                    if (CurrentGame[y + 1][x] == enemyPlayer && CurrentGame[y + 2][x] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x] == enemyPlayer && CurrentGame[y + 2][x] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown && canMoveRight)
        //                {
        //                    if (CurrentGame[y + 1][x + 1] == enemyPlayer && CurrentGame[y + 2][x + 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x + 1] == enemyPlayer && CurrentGame[y + 2][x + 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                if (canMoveDown && canMoveLeft)
        //                {
        //                    if (CurrentGame[y + 1][x - 1] == enemyPlayer && CurrentGame[y + 2][x - 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                    else if (CurrentGame[y + 1][x - 1] == enemyPlayer && CurrentGame[y + 2][x - 2] == enemyPlayer)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}


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
