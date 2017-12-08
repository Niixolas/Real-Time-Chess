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

    public override void showTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {
        if (targetDirX == 1 && targetDirY == 1)
        {
            for (int i = currentX + 1, j = currentY + 1; i < 8; i++, j++)
            {
                if (chessBoard[i, j] != null)
                {
                    Instantiate(lightSelect, Utilities.getTileCenter(i, j), Quaternion.identity);
                    break;
                }
                if (!(j < 8))
                {
                    break;
                }
            }
        }

        if (targetDirX == 0 && targetDirY == 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("targetSelector"));
        }
    }
}


