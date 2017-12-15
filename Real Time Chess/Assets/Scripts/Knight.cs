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
            if ( ( (Mathf.Abs(currentX - x) == 2) && (Mathf.Abs(currentY - y) == 1) ) || ( (Mathf.Abs(currentX - x) == 1) && (Mathf.Abs(currentY - y) == 2) ) )
            {
                return true;
            }

        }
        return false;
    }

    public override void showTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {
        if (targetDirX == 0 && targetDirY == 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("targetSelector"));
        }


        if (targetDirX == 0 && targetDirY == 1)
        {
            Instantiate(lightSelect, Utilities.getTileCenter(currentX + 1, currentY + 2), Quaternion.identity);
        } else if (targetDirX == 1 && targetDirY == 1)
        {

        } else if (targetDirX == 1 && targetDirY == 0)
        {

        }


    }



}
