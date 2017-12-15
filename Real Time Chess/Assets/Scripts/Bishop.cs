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
        if (targetDirX != 0 && targetDirY != 0)
        {
            for (int i = currentX + targetDirX, j = currentY + targetDirY; i < 8; i += targetDirX, j += targetDirY)
            {
                if (i < 0 || i > 7 || j < 0 || j > 8)
                {
                    break;
                }

                if (chessBoard[i, j] != null)
                {
                    Instantiate(lightSelect, Utilities.getTileCenter(i, j), Quaternion.identity);
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


