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
            if (this.currentX == x || this.currentY == y)
            {
                return true;
            }
            
        }
        return false;
    }

}
