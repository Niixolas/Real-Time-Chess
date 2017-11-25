using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            // Knight movement
            if ( ( (Mathf.Abs(this.currentX - x) == 2) && (Mathf.Abs(this.currentY - y) == 1) ) || ( (Mathf.Abs(this.currentX - x) == 1) && (Mathf.Abs(this.currentY - y) == 2) ) )
            {
                return true;
            }

        }
        return false;
    }

}
