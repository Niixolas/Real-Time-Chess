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

    public override void showTarget(ChessPiece[,] chessBoard)
    {
        for (int y = currentY; y <= 7; y++)
        {
            if (chessBoard[currentX, y] != null)
            {
                if (chessBoard[currentX, y].isWhite != this.isWhite)
                {
                    GameObject newselect = Instantiate(lightSelect, GetComponent<BoardManager>().getTileCenter(currentX, y), this.transform.rotation) as GameObject;
                    break;
                }
            }
        }
    }

}
