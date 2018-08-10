﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
//using InControl;

public class BoardManager : MonoBehaviour
{
    // Game sentinel value
    [HideInInspector]
    public bool gameOver;

    public InputController inputController;

    // Variable to hold the currently selected piece
    public ChessPiece blueSelectedPiece;
    public ChessPiece redSelectedPiece;

    //variable to hold the healthbar of a particular peice
    public HealthBar healthBar;

    public GameObject shot;

    // List containing all the chess pieces
    public List<GameObject> pieces;

    // List containing currently active pieces
    public List<GameObject> activePieces;

    // Holding which piece is glowing
    private Vector2 blueSelection;
    private Vector2 redSelection;

    // Selector Box (particles)
    public GameObject blueSelector;
    public GameObject redSelector;

    // Selection box
    public GameObject whiteSelectionBox;
    public GameObject blackSelectionBox;

    // Pulsing "Check!" text
    public Text redCheckText;
    public Text greenCheckText;

    // Currently selected tile. No selection provides -1.
    private int selectionX = -1;
    private int selectionY = -1;

    // Use this for initialization
    void Start ()
    {
        gameOver = false;
        SpawnAllPieces();
        Utilities.chessBoard[4, 0].glow.enabled = true;
        blueSelection = new Vector2(4, 0);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        if (!gameOver)
        {
            DrawChessBoard();
            CheckInputs();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FireBullet>())
        {
            Destroy(collision.gameObject);
        }
    }


    // Check controller inputs
    private void CheckInputs()
    {
        if (inputController.p1Pressed)
        {
            if (blueSelectedPiece == null)
            {
                if (Utilities.chessBoard[Controller.greenSelectionX, Controller.greenSelectionY] != null && Utilities.chessBoard[Controller.greenSelectionX, Controller.greenSelectionY].isWhite)
                {
                    SelectPiece(Controller.greenSelectionX, Controller.greenSelectionY, 1);
                    whiteSelectionBox.GetComponent<SpriteRenderer>().enabled = false;
                    whiteSelectionBox.GetComponent<selectionController>().isMoving = false;
                }
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("greenSelector"));
                whiteSelectionBox.GetComponent<SpriteRenderer>().enabled = true;
                whiteSelectionBox.transform.position = Utilities.getTileCenter(blueSelectedPiece.CurrentX, blueSelectedPiece.CurrentY);
                blueSelectedPiece = null;
            }
        }

        if (inputController.p2Pressed)
        {
            if (redSelectedPiece == null)
            {
                if (Utilities.chessBoard[Controller.redSelectionX, Controller.redSelectionY] != null && !Utilities.chessBoard[Controller.redSelectionX, Controller.redSelectionY].isWhite)
                {
                    SelectPiece(Controller.redSelectionX, Controller.redSelectionY, 2);
                    blackSelectionBox.GetComponent<SpriteRenderer>().enabled = false;
                    blackSelectionBox.GetComponent<selectionController>().isMoving = false;
                }
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("redSelector"));
                blackSelectionBox.GetComponent<SpriteRenderer>().enabled = true;
                blackSelectionBox.transform.position = Utilities.getTileCenter(redSelectedPiece.CurrentX, redSelectedPiece.CurrentY);
                redSelectedPiece = null;
            }
        }

        //if (Controller.getFire(1))
        //{
            if (inputController.p1Aim != Vector2.zero)
            {
                if (blueSelectedPiece != null)
                {
                    blueSelectedPiece.fire(1);
                }
            }
        //}

        //if (Controller.getFire(2))
        //{
            if (inputController.p2Aim != Vector2.zero)
            {
                if (redSelectedPiece != null)
                {
                    redSelectedPiece.fire(2);
                }
            }
        //}

        if (inputController.p1Move != Vector2.zero)
        {
            if (blueSelectedPiece != null && !blueSelectedPiece.isMoving)
            {
                blueSelectedPiece.movePiece();
            }
        }

        if (inputController.p2Move != Vector2.zero)
        {
            if (redSelectedPiece != null && !redSelectedPiece.isMoving)
            {
                redSelectedPiece.movePiece();
            }
        }

    }

    /// <summary>
    /// Selects a chess piece if a piece exists in the selected square
    /// </summary>
    public void SelectPiece(int x, int y, int player)
    {
        if (Utilities.chessBoard[x, y] == null)
        {
            return;
        }

        if (player == 1 && Utilities.chessBoard[x, y].isWhite)
        {
            blueSelectedPiece = Utilities.chessBoard[x, y];
            Instantiate(blueSelector, blueSelectedPiece.transform);
        }
        if (player == 2 && !Utilities.chessBoard[x, y].isWhite)
        {
            redSelectedPiece = Utilities.chessBoard[x, y];
            Instantiate(redSelector, redSelectedPiece.transform);
        }        
    }

    /// <summary>
    /// Draws the (debug line) chessboard
    /// </summary>
    private void DrawChessBoard ()
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
                if (Utilities.chessBoard[i, j] != null)
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
    public void SpawnPiece (int index, int x, int y, int maxHealthValue)
    {
        GameObject chessPiece = Instantiate(pieces[index], Utilities.getTileCenter(x, y), Quaternion.identity) as GameObject;
        ChessPiece aPiece = chessPiece.GetComponent<ChessPiece>();

        Utilities.chessBoard[x, y] = aPiece;
        Utilities.chessBoard[x, y].setPosition(x, y);

        HealthBar hb = chessPiece.GetComponentInChildren<HealthBar>();
        hb.MaxHealth = maxHealthValue;
        //hb.transform.SetParent(aPiece.transform, false);
        hb.CurrentHealth = maxHealthValue;
        aPiece.setHealthBar(hb);
        aPiece.setShot(shot);

        activePieces.Add(chessPiece);
    }

    /// <summary>
    /// Spawn a chess piece
    /// </summary>
    private void SpawnAllPieces()
    {
        activePieces = new List<GameObject>();
        Utilities.chessBoard = new ChessPiece[8, 8];

        // Spawn White pieces
        SpawnPiece(0, 4, 0, 200); // King
        SpawnPiece(1, 3, 0, 200); // Queen
        SpawnPiece(2, 0, 0, 100); // Rook 1
        SpawnPiece(2, 7, 0, 100); // Rook 2
        SpawnPiece(3, 1, 0, 100); // Knight 1
        SpawnPiece(3, 6, 0, 100); // Knight 2
        SpawnPiece(4, 2, 0, 100); // Bishop 1
        SpawnPiece(4, 5, 0, 100); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(5, i, 1, 20);
        }

        // Spawn Black pieces
        SpawnPiece(6, 4, 7, 200); // King
        SpawnPiece(7, 3, 7, 200); // Queen
        SpawnPiece(8, 0, 7, 100); // Rook 1
        SpawnPiece(8, 7, 7, 100); // Rook 2
        SpawnPiece(9, 1, 7, 100); // Knight 1
        SpawnPiece(9, 6, 7, 100); // Knight 2
        SpawnPiece(10, 2, 7, 100); // Bishop 1
        SpawnPiece(10, 5, 7, 100); // Bishop 2

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(11, i, 6, 20);
        }
    }

    public void SetCheck(bool isWhite)
    {
        if (isWhite)
        {
            greenCheckText.enabled = true;
        }
        else
        {
            redCheckText.enabled = true;
        }
    }

}
