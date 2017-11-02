using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Array representing the board and locations of the pieces
    public ChessPiece[,] chessBoard { set; get; }

    // Variable to hold the currently selected piece
    private ChessPiece selectedPiece;

    // Width of each tile and offset to center of tile
    private const float tileWidth = 1.0f;
    private const float tileOffset = 0.5f;

    // List containing all the chess pieces
    public List<GameObject> pieces;

    // List containing currently active pieces
    public List<GameObject> activePieces;

    // Currently selected tile. No selection provides -1.
    private int selectionX = -1;
    private int selectionY = -1;

    // Use this for initialization
    void Start ()
    {
        spawnAllPieces();
    }
	
	/// Update is called once per frame
	void Update ()
    {
        updateSelection();
        drawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedPiece == null)
                {
                    selectPiece(selectionX, selectionY);
                } else {
                    movePiece(selectionX, selectionY);
                }
            }
        }
	}

    private void selectPiece(int x, int y)
    {
        if (chessBoard[x, y] == null)
        {
            return;
        }

        selectedPiece = chessBoard[x, y];
    }

    private void movePiece(int x, int y)
    {
        if (selectedPiece.isMovePossible(x, y))
        {
            chessBoard[selectedPiece.currentX, selectedPiece.currentY] = null;
            selectedPiece.transform.position = getTileCenter(x, y);
            chessBoard[x, y] = selectedPiece;
        }

        selectedPiece = null;
    }

    /// <summary>
    /// Updates the currently selected tile
    /// </summary>
    void updateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            selectionX = (int)(hit.point.x);
            selectionY = (int)(hit.point.y);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    /// <summary>
    /// Draws the chessboard
    /// </summary>
    private void drawChessBoard ()
    {
        for (int i = 0; i <= 8; i++)
        {
            Vector2 lineWidth = Vector2.right * 8;
            Vector2 lineHeight = Vector2.up * 8;
            Vector2 startH = Vector2.up * i;
            Vector2 startV = Vector2.right * i;
            Debug.DrawLine(startH, startH + lineWidth, Color.blue);
            Debug.DrawLine(startV, startV + lineHeight, Color.blue);
        }

        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * selectionY, Vector2.right * (selectionX + 1) + Vector2.up * (selectionY + 1), Color.red);
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * (selectionY + 1), Vector2.right * (selectionX + 1) + Vector2.up * selectionY, Color.red);
        }
    }

    /// <summary>
    /// Spawn a chess piece
    /// </summary>
    private void spawnPiece (int index, int x, int y)
    {
        GameObject chessPiece = Instantiate(pieces[index], getTileCenter(x, y), Quaternion.identity) as GameObject;
        chessBoard[x, y] = chessPiece.GetComponent<ChessPiece>();
        chessBoard[x, y].setPosition(x, y);
        activePieces.Add(chessPiece);
    }

    /// <summary>
    /// Spawn a chess piece
    /// </summary>
    private void spawnAllPieces()
    {
        activePieces = new List<GameObject>();
        chessBoard = new ChessPiece[8, 8];

        // Spawn White pieces
        spawnPiece(0, 4, 0); // King
        spawnPiece(1, 3, 0); // Queen
        spawnPiece(2, 0, 0); // Rook 1
        spawnPiece(2, 7, 0); // Rook 2
        spawnPiece(3, 1, 0); // Knight 1
        spawnPiece(3, 6, 0); // Knight 2
        spawnPiece(4, 2, 0); // Bishop 1
        spawnPiece(4, 5, 0); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            spawnPiece(5, i, 1);
        }

        // Spawn Black pieces
        spawnPiece(6, 3, 7); // King
        spawnPiece(7, 4, 7); // Queen
        spawnPiece(8, 0, 7); // Rook 1
        spawnPiece(8, 7, 7); // Rook 2
        spawnPiece(9, 1, 7); // Knight 1
        spawnPiece(9, 6, 7); // Knight 2
        spawnPiece(10, 2, 7); // Bishop 1
        spawnPiece(10, 5, 7); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            spawnPiece(11, i, 6);
        }
    }

    /// <summary>
    /// Get the coordinates for the center of a tile
    /// </summary>
    private Vector2 getTileCenter(int x, int y)
    {
        return new Vector2(tileWidth * x + tileOffset, tileWidth * y + tileOffset);
    }
}
