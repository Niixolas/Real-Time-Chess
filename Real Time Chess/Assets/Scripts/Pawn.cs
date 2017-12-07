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

}
