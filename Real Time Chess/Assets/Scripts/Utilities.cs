using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // Array representing the board and locations of the pieces
    public static ChessPiece[,] chessBoard { set; get; }

    // Width of each tile and offset to center of tile
    private const float tileWidth = 1.0f;
    private const float tileOffset = 0.5f;

    /// <summary>
    /// Get the screen coordinates for the center of a tile
    /// </summary>
    public static Vector3 getTileCenter(int x, int y)
    {
        return new Vector3(tileWidth * x + tileOffset, tileWidth * y + tileOffset, -5.0f);
    }

    public static Vector2 getBoardCoordinates(float x, float y)
    {
        return new Vector2((int)(x / tileWidth), (int)(y / tileWidth));
    }
        
}
