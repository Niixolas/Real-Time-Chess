using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Array representing the board and locations of the pieces
    public ChessPiece[,] chessBoard { set; get; }

    // Variable to hold the currently selected piece
    private ChessPiece greenSelectedPiece;
    private ChessPiece redSelectedPiece;

    //variable to hold the healthbar of a particluar peice
    public HealthBar healthBar;

    public GameObject shot;

    // List containing all the chess pieces
    public List<GameObject> pieces;

    // List containing currently active pieces
    public List<GameObject> activePieces;

    // Selector Box
    public GameObject greenSelector;
    public GameObject redSelector;

    // Currently selected tile. No selection provides -1.
    private int selectionX = -1;
    private int selectionY = -1;

    private int targetDirX = 0;
    private int targetDirY = 1;

    // Use this for initialization
    void Start ()
    {
        spawnAllPieces();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        updateSelection();
        drawChessBoard();

        if (Controller.getPressed(1))
        {
            if (greenSelectedPiece == null)
            {
                selectPiece(Controller.selectionX, Controller.selectionY, 1);
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("greenSelector"));
                greenSelectedPiece = null;
            }
        }

        if (Controller.getPressed(2))
        {
            if (redSelectedPiece == null)
            {
                selectPiece(Controller.selectionX, Controller.selectionY, 2);
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("redSelector"));
                redSelectedPiece = null;
            }
        }
        
        /*
        if (Input.GetKeyDown(KeyCode.Space) && selectedPiece != null)
        {
            HealthBar hb = selectedPiece.GetComponentInChildren<HealthBar>() as HealthBar;
            hb.DealDamage(1);
        }
        */
    }

    /// <summary>
    /// Selects a chess piece if a piece exists in the selected square
    /// </summary>
    private void selectPiece(int x, int y, int player)
    {
        if (chessBoard[x, y] == null)
        {
            return;
        }

        targetDirX = -1;
        targetDirY = 1;

        if (player == 1 && chessBoard[x, y].isWhite)
        {
            greenSelectedPiece = chessBoard[x, y];
            Instantiate(greenSelector, greenSelectedPiece.transform);
        }
        if (player == 2 && !chessBoard[x, y].isWhite)
        {
            redSelectedPiece = chessBoard[x, y];
            Instantiate(redSelector, redSelectedPiece.transform);
        }
        
    }

    /// <summary>
    /// Moves a chess piece if the movement is valid
    /// </summary>
    /*
    private void movePiece(int x, int y)
    {
        if (selectedPiece.isMovePossible(x, y, chessBoard[x,y]))
        {
            // Remove the piece from its original location in the array
            chessBoard[selectedPiece.currentX, selectedPiece.currentY] = null;

            // Move the visible piece on the board
            selectedPiece.transform.position = Utilities.getTileCenter(x, y);

            // Update the piece's position in the class
            selectedPiece.setPosition(x, y);

            // Place the piece in the array at its new location
            chessBoard[x, y] = selectedPiece;
        }

        targetDirX = 0;
        targetDirY = 0;
        selectedPiece.showTarget(chessBoard, targetDirX, targetDirY);

        // Have nothing selected
        Destroy(GameObject.FindGameObjectWithTag("selector"));
        selectedPiece = null;
    }
    */
    

    /// <summary>
    /// Updates the currently selected tile
    /// </summary>
    void updateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        // Set the current selection based on the mouse position
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, LayerMask.GetMask("Board"));
        if (hit.collider != null)
        {
            selectionX = (int)(hit.point.x);
            selectionY = (int)(hit.point.y);

            // Move the blue selection box to the currently selected tile
            //blueSelector.transform.position = Utilities.getTileCenter(selectionX, selectionY);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    /// <summary>
    /// Draws the (debug line) chessboard
    /// </summary>
    private void drawChessBoard ()
    {

        // For debugging - Mark on screen the chessboard grid
        for (int i = 0; i <= 8; i++)
        {
            Vector2 lineWidth = Vector2.right * 8;
            Vector2 lineHeight = Vector2.up * 8;
            Vector2 startH = Vector2.up * i;
            Vector2 startV = Vector2.right * i;
            Debug.DrawLine(startH, startH + lineWidth, Color.blue);
            Debug.DrawLine(startV, startV + lineHeight, Color.blue);
        }

        // For debugging - Mark on screen anything in the chessboard array that is not null
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[i, j] != null)
                {
                    Debug.DrawLine(Vector2.right * i + Vector2.up * j, Vector2.right * (i + 1) + Vector2.up * (j + 1), Color.green);
                    Debug.DrawLine(Vector2.right * i + Vector2.up * (j + 1), Vector2.right * (i + 1) + Vector2.up * j, Color.green);
                }
            }
        }

        // For debugging - Mark on screen the location of the mouse cursor
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * selectionY, Vector2.right * (selectionX + 1) + Vector2.up * (selectionY + 1), Color.red);
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * (selectionY + 1), Vector2.right * (selectionX + 1) + Vector2.up * selectionY, Color.red);
        }
    }

    /// <summary>
    /// Spawn a chess piece
    /// </summary>
    private void spawnPiece (int index, int x, int y, int maxHealthValue)
    {
        GameObject chessPiece = Instantiate(pieces[index], Utilities.getTileCenter(x, y), Quaternion.identity) as GameObject;
        ChessPiece aPiece = chessPiece.GetComponent<ChessPiece>();
        
        chessBoard[x, y] = aPiece;
        chessBoard[x, y].setPosition(x, y);

        HealthBar hb = Instantiate(healthBar , aPiece.transform);
        hb.MaxHealth = maxHealthValue;
        hb.transform.SetParent(aPiece.transform, false);
        hb.CurrentHealth = maxHealthValue;
        aPiece.setHealthBar(hb);
        aPiece.setShot(shot);

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
        spawnPiece(0, 4, 0, 200); // King
        spawnPiece(1, 3, 0, 200); // Queen
        spawnPiece(2, 0, 0, 100); // Rook 1
        spawnPiece(2, 7, 0, 100); // Rook 2
        spawnPiece(3, 1, 0, 100); // Knight 1
        spawnPiece(3, 6, 0, 100); // Knight 2
        spawnPiece(4, 2, 0, 100); // Bishop 1
        spawnPiece(4, 5, 0, 100); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            spawnPiece(5, i, 1, 20);
        }

        // Spawn Black pieces
        spawnPiece(6, 4, 7, 200); // King
        spawnPiece(7, 3, 7, 200); // Queen
        spawnPiece(8, 0, 7, 100); // Rook 1
        spawnPiece(8, 7, 7, 100); // Rook 2
        spawnPiece(9, 1, 7, 100); // Knight 1
        spawnPiece(9, 6, 7, 100); // Knight 2
        spawnPiece(10, 2, 7, 100); // Bishop 1
        spawnPiece(10, 5, 7, 100); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            spawnPiece(11, i, 6, 20);
        }
    }


}
