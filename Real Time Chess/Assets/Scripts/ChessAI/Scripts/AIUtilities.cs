using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIUtilities
{
    public static Vector2 AIMove;
    public static bool AIPressed;


    public static Vector2Int FindTargetLocation(int target)
    {
        Vector2Int targetLocation = new Vector2Int(-1, -1);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Utilities.chessBoard[i, j] != null && Utilities.chessBoard[i, j].GetComponent<ChessPiece>().id == target)
                {
                    targetLocation.x = Utilities.chessBoard[i, j].GetComponent<ChessPiece>().CurrentX;
                    targetLocation.y = Utilities.chessBoard[i, j].GetComponent<ChessPiece>().CurrentY;
                }
            }
        }

        return targetLocation;
    }
}
