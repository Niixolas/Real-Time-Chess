using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
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

    private void Update()
    {
        if (healthBar.CurrentHealth <= 10)
        {
            FindObjectOfType<BoardManager>().setCheck(isWhite);
        }
    }
}
