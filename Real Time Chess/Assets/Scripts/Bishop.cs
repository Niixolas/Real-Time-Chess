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
            if (Mathf.Abs(this.currentX - x) == Mathf.Abs(this.currentY - y))
            {
                return true;
            }

        }
        return false;
    }
}


