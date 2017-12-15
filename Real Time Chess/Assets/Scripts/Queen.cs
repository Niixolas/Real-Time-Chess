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

    public override void showTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {
        // Destroy the target selector if not targeting anything
        if (targetDirX == 0 && targetDirY == 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("targetSelector"));
        }
        else
        {
            // Determine the first piece in the direction targeting and create the target selector
            for (int i = currentX + targetDirX, j = currentY + targetDirY; i >= 0 && j >= 0 && Mathf.Abs(j) < 8 && Mathf.Abs(i) < 8; i += targetDirX, j += targetDirY)
            {
                if (chessBoard[i, j] != null)
                {
                    Instantiate(lightSelect, Utilities.getTileCenter(i, j), Quaternion.identity);
                    break;
                }
            }
        }
    }
}
