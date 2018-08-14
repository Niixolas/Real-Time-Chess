using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool IsMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            return true;
        }
        return false;
    }

    public override bool IsAimPossible(int x, int y)
    {
        return true;
    }
}
