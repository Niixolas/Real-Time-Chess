using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            if (x == this.currentX)
            {
                if (y > this.currentY)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool isAimPossible(int x, int y)
    {
        if (isWhite && x != 0 && y > 0)
        {
            return true;
        }
        return false;
    }
}
