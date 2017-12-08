using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // Width of each tile and offset to center of tile
    private const float tileWidth = 1.0f;
    private const float tileOffset = 0.5f;

    /// <summary>
    /// Get the screen coordinates for the center of a tile
    /// </summary>
    public static Vector2 getTileCenter(int x, int y)
    {
        return new Vector2(tileWidth * x + tileOffset, tileWidth * y + tileOffset);
    }
}
