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
            if (targetDirX != 0 || targetDirY != 0)
            {
                if (currentX + targetDirX > 0 && currentY + targetDirY > 0)
                {
                    Instantiate(lightSelect, Utilities.getTileCenter(currentX + targetDirX, currentY + targetDirY), Quaternion.identity);
                }
            }
        }
    }

}
