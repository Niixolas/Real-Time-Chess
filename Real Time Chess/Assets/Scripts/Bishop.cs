using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
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

    public override bool isAimPossible(int x, int y)
    {
        if (x != 0 && y != 0)
        {
            return true;
        }
        return false;
    }
}


