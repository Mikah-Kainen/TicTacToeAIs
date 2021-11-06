﻿using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe
{
    public class Board : IGameState<Board>
    {
        public Players[][] CurrentGame;
        public Players CurrentPlayer;
        public bool IsWin => DidPlayerWin(CurrentPlayer);

        public bool IsTie => !IsPlayable();

        public bool IsLose => throw new NotImplementedException();

        public bool IsTerminal => IsWin || IsTie || IsLose;

        public Board[] GetChildren()
        {
            throw new NotImplementedException();
        }

        public Board(Players[][] currentGame, Players currentPlayer)
        {
            CurrentGame = currentGame;
            CurrentPlayer = currentPlayer;
        }

        public bool DidPlayerWin(Players currentPlayer)
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < CurrentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < CurrentGame.Length;
                    if (CurrentGame[y][x] == currentPlayer)
                    {
                        if (canMoveRight)
                        {
                            if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y][x + 1] == currentPlayer && CurrentGame[y][x + 2] == currentPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown)
                        {
                            if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x] == currentPlayer && CurrentGame[y + 2][x] == currentPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x + 1] == currentPlayer && CurrentGame[y + 2][x + 2] == currentPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x - 1] == currentPlayer && CurrentGame[y + 2][x - 2] == currentPlayer)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false ;
        }

        public bool DidPlayerLose(Players currentPlayer)
        {
            for (int y = 0; y < CurrentGame.Length; y++)
            {
                for (int x = 0; x < CurrentGame[y].Length; x++)
                {
                    bool canMoveRight = x + 2 < CurrentGame[y].Length;
                    bool canMoveLeft = x - 2 >= 0;
                    bool canMoveDown = y + 2 < CurrentGame.Length;
                    if (CurrentGame[y][x] != currentPlayer && CurrentGame[y][x] != Players.None)
                    {
                        Players enemyPlayer = CurrentGame[y][x];
                        if (canMoveRight)
                        {
                            if (CurrentGame[y][x + 1] == enemyPlayer && CurrentGame[y][x + 2] == enemyPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y][x + 1] == enemyPlayer && CurrentGame[y][x + 2] == enemyPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown)
                        {
                            if (CurrentGame[y + 1][x] == enemyPlayer && CurrentGame[y + 2][x] == enemyPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x] == enemyPlayer && CurrentGame[y + 2][x] == enemyPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown && canMoveRight)
                        {
                            if (CurrentGame[y + 1][x + 1] == enemyPlayer && CurrentGame[y + 2][x + 2] == enemyPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x + 1] == enemyPlayer && CurrentGame[y + 2][x + 2] == enemyPlayer)
                            {
                                return true;
                            }
                        }
                        if (canMoveDown && canMoveLeft)
                        {
                            if (CurrentGame[y + 1][x - 1] == enemyPlayer && CurrentGame[y + 2][x - 2] == enemyPlayer)
                            {
                                return true;
                            }
                            else if (CurrentGame[y + 1][x - 1] == enemyPlayer && CurrentGame[y + 2][x - 2] == enemyPlayer)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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