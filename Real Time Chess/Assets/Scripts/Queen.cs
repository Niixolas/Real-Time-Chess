using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            return true;
        }
        return false;
    }

    public override bool isAimPossible(int x, int y)
    {
        return true;
    }
}
