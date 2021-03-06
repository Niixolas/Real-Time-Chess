﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool IsMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            // Diagonal movement
            if (Mathf.Abs(this.CurrentX - x) == Mathf.Abs(this.CurrentY - y))
            {
                return true;
            }
        }
        return false;
    }

    public override bool IsAimPossible(int x, int y)
    {
        if (x != 0 && y != 0)
        {
            return true;
        }
        return false;
    }

    public override void ShowPossibleActions()
    {
        for (int xDir = -1; xDir <= 1; xDir += 2)
        {
            for (int yDir = -1; yDir <= 1; yDir += 2)
            {
                for (int x = CurrentX + xDir, y = CurrentY + yDir; x <= 7 && x >= 0; x += xDir, y += yDir)
                {
                    if (x < 0 || x > 7 || y < 0 || y > 7)
                    {
                        break;
                    }

                    //if (Utilities.chessBoard[x, y] == null)
                    //{
                    //    Vector3 newMoveSquarePosition = Utilities.getTileCenter(x, y);
                    //    newMoveSquarePosition.z = 3.0f;
                    //    GameObject newMoveSquare = Instantiate(targetMoveSquare, newMoveSquarePosition, Quaternion.identity);
                    //    targetMoveAndAimSquares.Add(newMoveSquare);
                    //}
                    //else
                    if (Utilities.chessBoard[x, y] != null)
                    {
                        if (Utilities.chessBoard[x, y].isWhite != isWhite)
                        {
                            Vector3 newMoveSquarePosition = Utilities.getTileCenter(x, y);
                            newMoveSquarePosition.z = 3.0f;
                            GameObject newAimSquare = Instantiate(targetAimSquare, newMoveSquarePosition, Quaternion.identity);
                            targetMoveAndAimSquares.Add(newAimSquare);
                        }
                        break;
                    }
                }
            }
        }
    }
}


