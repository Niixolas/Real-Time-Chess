using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            // Same row or Same column movement
            if (this.CurrentX == x || this.CurrentY == y)
            {
                return true;
            }
            
        }
        return false;
    }

    public override bool isAimPossible(int x, int y)
    {
        if (x != 0 && y == 0 || x == 0 && y !=0)
        {
            return true;
        }
        return false;
    }

}
