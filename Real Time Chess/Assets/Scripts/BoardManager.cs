using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Game Prefabs")]
    [Tooltip("The prefab for the shots that are fired")]
    public GameObject shot;

    [Tooltip("Prefabs for the pawn promotion effects")]
    public GameObject bluePromotion;
    [Tooltip("Prefabs for the pawn promotion effects")]
    public GameObject redPromotion;

    [Tooltip("Prefab for the checkers")]
    public GameObject checkers;

    [Tooltip("Prefabs for all the chess pieces")]
    public List<GameObject> pieces;    

    [Header("UI Prefabs")]
    [Tooltip("Prefabs for the pulsing 'Check!' text")]
    public Text blueCheckText;
    [Tooltip("Prefabs for the pulsing 'Check!' text")]
    public Text redCheckText;    

    [Tooltip("Prefab for the starting text")]
    public Text startText;

    [Tooltip("The fade animation")]
    public Animator fadeIn;

    [Header("References")]
    [Tooltip("Reference to the moving sound")]
    public AudioClip moveClip;

    [Tooltip("Reference to the firing sound")]
    public AudioClip hitClip;

    [Tooltip("Reference to the blue selector movement sound")]
    public AudioClip blueSelectorClip;

    [Tooltip("Reference to the red selector movement sound")]
    public AudioClip redSelectorClip;

    [Header("Variables")]
    [Tooltip("Time between movement input checks")]
    [SerializeField]
    private float SelectionMoveDelay = 0.25f;

    // Game sentinel value
    [HideInInspector]
    public bool gameOver;

    [HideInInspector]
    public bool gameStarted;

    // Variables to hold the currently selected pieces
    [HideInInspector]
    public ChessPiece blueSelectedPiece, redSelectedPiece;

    // List containing currently active pieces
    [HideInInspector]
    public List<GameObject> activePieces;

    /// <summary>
    /// Vector holding position of currently selected glowing piece
    /// </summary>
    [HideInInspector]
    public Vector2Int blueSelection, redSelection;
    
    // Variables to keep track of the delay between movement
    private float BlueSelectionMoveTime = 0.0f;
    private float RedSelectionMoveTime = 0.0f;

    public Customizer customizer;

    private int idCounter = 0;

    // Use this for initialization
    void Start ()
    {
        // Initialize gameOver sentinel value
        gameOver = false;

        // Grab the color customizer
        customizer = FindObjectOfType<Customizer>();

        // Set the board color
        foreach (Transform child in checkers.transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = customizer.checkerColor;
        }

        // Spawn the chess pieces and customize their properties
        SpawnAllPieces();

        // Set first blue selection to king
        Utilities.chessBoard[4, 1].glow.enabled = true;
        blueSelection = new Vector2Int(4, 1);
        SelectPiece(blueSelection.x, blueSelection.y, 1);

        // Set first red selection to king
        Utilities.chessBoard[4, 6].glow.enabled = true;
        redSelection = new Vector2Int(4, 6);
        SelectPiece(redSelection.x, redSelection.y, 2);

        // Enable start text
        startText.enabled = true;

        // Start the AI if it exists
        if (FindObjectOfType<StateController>() != null)
        {
            FindObjectOfType<StateController>().StartChessAI();
        }

        fadeIn.SetTrigger("fadeIn");

        // Trigger the start game after a short delay
        Invoke("StartGame", 1.5f);
    }

    private void StartGame()
    {
        gameStarted = true;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        if (!gameOver && gameStarted)
        {
            DrawChessBoard();
            CheckInputs();
        }

        if (BlueSelectionMoveTime > 0)
        {
            BlueSelectionMoveTime -= Time.deltaTime;
            BlueSelectionMoveTime = BlueSelectionMoveTime < 0 ? 0 : BlueSelectionMoveTime;
        }

        if (RedSelectionMoveTime > 0)
        {
            RedSelectionMoveTime -= Time.deltaTime;
            RedSelectionMoveTime = RedSelectionMoveTime < 0 ? 0 : RedSelectionMoveTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy bullets when they leave the playfield
        if (collision.gameObject.GetComponent<FireBullet>())
        {
            Destroy(collision.gameObject);
        }
    }

    // Refresh highlight squares
    public void RefreshActions()
    {
        if (blueSelectedPiece != null)
        {
            blueSelectedPiece.HidePossibleActions();
            blueSelectedPiece.ShowPossibleActions();
        }

        if (redSelectedPiece != null)
        {
            redSelectedPiece.HidePossibleActions();
            redSelectedPiece.ShowPossibleActions();
        }
    }
    /// <summary>
    /// Check if a piece is eligible for Pawn promotion to Queen
    /// </summary>
    /// <param name="sendingPiece">The piece to check if eligible for pawn promotion</param>
    /// <param name="targetPosition">The square the piece has moved to</param>
    public void CheckPawnPromotion(ChessPiece sendingPiece, Vector2 targetPosition)
    {
        // Grab the current piece health
        float thisHealth = sendingPiece.healthBar.CurrentHealth;

        // If the piece is a white piece, it is at the end of the board, and it is a pawn
        if (sendingPiece.isWhite && (int)targetPosition.y == 7 && sendingPiece.GetComponent<Pawn>() != null)
        {
            // Clear the piece from the board array
            sendingPiece.HidePossibleActions();
            Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = null;

            // Create pawn promotion effect
            GameObject pawnPromotionEffect = Instantiate(bluePromotion, sendingPiece.transform.position, Quaternion.identity);
            pawnPromotionEffect.transform.position = new Vector3(sendingPiece.transform.position.x,
                                                                 sendingPiece.transform.position.y - 0.5f,
                                                                 -3.0f);
            Destroy(pawnPromotionEffect, 1.0f);

            // Destroy the pawn
            Destroy(sendingPiece.gameObject);

            // Spawn the Queen
            SpawnPiece(1, (int)targetPosition.x, 7, (int)thisHealth + 20);

            // Select the new piece
            blueSelectedPiece = null;
            SelectPiece((int)targetPosition.x, (int)targetPosition.y, 1);

            // Show squares for possible targets
            blueSelectedPiece.ShowPossibleActions();
        }

        // If the piece is a black piece, it is at the end of the board, and it is a pawn
        if (!sendingPiece.isWhite && (int)targetPosition.y == 0 && sendingPiece.GetComponent<Pawn>() != null)
        {
            // Clear the piece from the board array
            sendingPiece.HidePossibleActions();
            Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = null;

            // Create pawn promotion effect
            GameObject pawnPromotionEffect = Instantiate(redPromotion, sendingPiece.transform.position, Quaternion.identity);
            pawnPromotionEffect.transform.position = new Vector3(sendingPiece.transform.position.x, 
                                                                 sendingPiece.transform.position.y - 0.5f,
                                                                 -3.0f);
            Destroy(pawnPromotionEffect, 1.0f);

            // Destroy the pawn
            Destroy(sendingPiece.gameObject);

            // Spawn the Queen
            SpawnPiece(7, (int)targetPosition.x, 0, (int)thisHealth + 20);

            // Select the new piece
            redSelectedPiece = null;
            SelectPiece((int)targetPosition.x, (int)targetPosition.y, 2);

            // Show squares for possible targets
            redSelectedPiece.ShowPossibleActions();
        }
    }

    // Check controller inputs
    private void CheckInputs()
    {

        // Check for player 1 moving glowing selection
        if (InputController.Instance.p1Move != Vector2.zero && InputController.Instance.p1Pressed && blueSelectedPiece == null && BlueSelectionMoveTime == 0.0f)
        {
            Vector2 rayStart = Utilities.getTileCenter(blueSelection.x, blueSelection.y);
            RaycastHit2D hit = Physics2D.CircleCast(rayStart, 0.3f, InputController.Instance.p1MoveFloat, Mathf.Infinity, LayerMask.GetMask("BluePieces"));

            Debug.DrawLine(rayStart, rayStart + InputController.Instance.p1MoveFloat * 100.0f);

            if (hit.collider != null && hit.collider.GetComponent<ChessPiece>().isWhite && Utilities.chessBoard[blueSelection.x, blueSelection.y] != null)
            {
                Utilities.chessBoard[blueSelection.x, blueSelection.y].glow.enabled = false;

                int targetX = hit.collider.GetComponent<ChessPiece>().CurrentX;
                int targetY = hit.collider.GetComponent<ChessPiece>().CurrentY;

                blueSelection = new Vector2Int(targetX, targetY);
                Utilities.chessBoard[blueSelection.x, blueSelection.y].glow.enabled = true;

                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.Play();

                BlueSelectionMoveTime = SelectionMoveDelay;
            }
        }

        if (InputController.Instance.p1DPad != Vector2.zero && BlueSelectionMoveTime == 0.0f)
        {
            Vector2 rayStart = Utilities.getTileCenter(blueSelection.x, blueSelection.y);
            RaycastHit2D hit = Physics2D.CircleCast(rayStart, 0.3f, InputController.Instance.p1DPad, Mathf.Infinity, LayerMask.GetMask("BluePieces"));

            if (hit.collider != null && hit.collider.GetComponent<ChessPiece>().isWhite && Utilities.chessBoard[blueSelection.x, blueSelection.y] != null)
            {
                Utilities.chessBoard[blueSelection.x, blueSelection.y].glow.enabled = false;
                blueSelectedPiece.selectedOutline.enabled = false;
                blueSelectedPiece = null;

                int targetX = hit.collider.GetComponent<ChessPiece>().CurrentX;
                int targetY = hit.collider.GetComponent<ChessPiece>().CurrentY;

                blueSelection = new Vector2Int(targetX, targetY);
                Utilities.chessBoard[blueSelection.x, blueSelection.y].glow.enabled = true;

                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.Play();

                SelectPiece(blueSelection.x, blueSelection.y, 1);
                Utilities.chessBoard[blueSelection.x, blueSelection.y].ShowPossibleActions();

                BlueSelectionMoveTime = SelectionMoveDelay;
            }
        }

        // Check for player 2 moving glowing selection
        if (InputController.Instance.p2Move != Vector2.zero && redSelectedPiece == null && RedSelectionMoveTime == 0.0f)
        {
            Vector2 rayStart = Utilities.getTileCenter(redSelection.x, redSelection.y);
            RaycastHit2D hit = Physics2D.CircleCast(rayStart, 0.2f, InputController.Instance.p2MoveFloat, 10.0f, LayerMask.GetMask("RedPieces"));

            if (hit.collider != null && !hit.collider.GetComponent<ChessPiece>().isWhite)
            {
                Utilities.chessBoard[redSelection.x, redSelection.y].glow.enabled = false;

                int targetX = hit.collider.GetComponent<ChessPiece>().CurrentX;
                int targetY = hit.collider.GetComponent<ChessPiece>().CurrentY;

                redSelection = new Vector2Int(targetX, targetY);
                Utilities.chessBoard[redSelection.x, redSelection.y].glow.enabled = true;

                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.Play();

                RedSelectionMoveTime = SelectionMoveDelay;
            }
        }

        if (InputController.Instance.p2DPad != Vector2.zero && RedSelectionMoveTime == 0.0f)
        {
            Vector2 rayStart = Utilities.getTileCenter(redSelection.x, redSelection.y);
            RaycastHit2D hit = Physics2D.CircleCast(rayStart, 0.3f, InputController.Instance.p2DPad, Mathf.Infinity, LayerMask.GetMask("RedPieces"));

            if (hit.collider != null && hit.collider.GetComponent<ChessPiece>().isWhite && Utilities.chessBoard[redSelection.x, redSelection.y] != null)
            {
                Utilities.chessBoard[redSelection.x, redSelection.y].glow.enabled = false;
                redSelectedPiece.selectedOutline.enabled = false;
                redSelectedPiece = null;

                int targetX = hit.collider.GetComponent<ChessPiece>().CurrentX;
                int targetY = hit.collider.GetComponent<ChessPiece>().CurrentY;

                redSelection = new Vector2Int(targetX, targetY);
                Utilities.chessBoard[redSelection.x, redSelection.y].glow.enabled = true;

                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.Play();

                SelectPiece(redSelection.x, redSelection.y, 1);
                Utilities.chessBoard[redSelection.x, redSelection.y].ShowPossibleActions();

                RedSelectionMoveTime = SelectionMoveDelay;
            }
        }

        // Check if player 1 pressed action button
        if (InputController.Instance.p1Pressed)
        {
            //if (blueSelectedPiece == null)
            //{
            //    // If blue player is not already selecting a piece, and the current selection is not null and is also a blue piece
            //    if (Utilities.chessBoard[blueSelection.x, blueSelection.y] != null && Utilities.chessBoard[blueSelection.x, blueSelection.y].isWhite)
            //    {
            //        SelectPiece(blueSelection.x, blueSelection.y, 1);
            //        Utilities.chessBoard[blueSelection.x, blueSelection.y].ShowPossibleActions();
            //    }
            //}
            //else
            //{
            if (blueSelectedPiece != null)
            {
                // Player 1 unselects their piece
                blueSelectedPiece.glow.enabled = true;
                blueSelectedPiece.selectedOutline.enabled = false;
                blueSelectedPiece = null;
                Utilities.chessBoard[blueSelection.x, blueSelection.y].HidePossibleActions();

                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.Play();
            }
                
            //}
        }
        else
        {
            if (blueSelectedPiece == null)
            {
                //blueSelectedPiece.glow.enabled = false;
                //blueSelectedPiece.selectedOutline.enabled = true;
                SelectPiece(blueSelection.x, blueSelection.y, 1);
                Utilities.chessBoard[blueSelection.x, blueSelection.y].ShowPossibleActions();

                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.pitch = 0.7f;
                Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.Play();
                //Utilities.chessBoard[blueSelection.x, blueSelection.y].selectorAudio.pitch = 1.0f;
            }
            
        }

        // Check if player 2 pressed action button
        if (InputController.Instance.p2Pressed)
        {
            if (redSelectedPiece != null)
            {
                redSelectedPiece.glow.enabled = true;
                redSelectedPiece.selectedOutline.enabled = false;
                redSelectedPiece = null;
                Utilities.chessBoard[redSelection.x, redSelection.y].HidePossibleActions();

                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.pitch = 1.0f;
                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.Play();
                //if (Utilities.chessBoard[redSelection.x, redSelection.y] != null && !Utilities.chessBoard[redSelection.x, redSelection.y].isWhite)
                //{
                //    SelectPiece(redSelection.x, redSelection.y, 2);
                //    Utilities.chessBoard[redSelection.x, redSelection.y].ShowPossibleActions();
                //}
            }
        }
        else
        {
            if (redSelectedPiece == null)
            {
                SelectPiece(redSelection.x, redSelection.y, 2);
                Utilities.chessBoard[redSelection.x, redSelection.y].ShowPossibleActions();

                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.pitch = 0.7f;
                Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.Play();
                //Utilities.chessBoard[redSelection.x, redSelection.y].selectorAudio.pitch = 1.0f;
            }
            // Player 2 unselects their piece
            //redSelectedPiece.glow.enabled = true;
            //redSelectedPiece.selectedOutline.enabled = false;
            //redSelectedPiece = null;

        }

        // If player 1 is firing
        if (InputController.Instance.p1Aim != Vector2.zero)
        {
            if (blueSelectedPiece != null)
            {
                blueSelectedPiece.Fire(1);
            }
        }

        // If player 2 is firing
        if (InputController.Instance.p2Aim != Vector2.zero)
        {
            if (redSelectedPiece != null)
            {
                redSelectedPiece.Fire(2);
            }
        }

        // If player 1 is moving a piece
        if (InputController.Instance.p1Move != Vector2.zero)
        {
            if (blueSelectedPiece != null && !blueSelectedPiece.isMoving)
            {
                blueSelectedPiece.MovePiece();
            }
        }

        // If player 2 is moving a piece
        if (InputController.Instance.p2Move != Vector2.zero)
        {
            if (redSelectedPiece != null && !redSelectedPiece.isMoving)
            {
                redSelectedPiece.MovePiece();
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
            blueSelectedPiece.glow.enabled = false;
            blueSelectedPiece.selectedOutline.enabled = true;
        }
        if (player == 2 && !Utilities.chessBoard[x, y].isWhite)
        {
            redSelectedPiece = Utilities.chessBoard[x, y];
            redSelectedPiece.glow.enabled = false;
            redSelectedPiece.selectedOutline.enabled = true;
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
    }

    /// <summary>
    /// Spawn a chess piece
    /// </summary>
    public void SpawnPiece (int index, int x, int y, int maxHealthValue)
    {
        GameObject chessPiece = Instantiate(pieces[index], Utilities.getTileCenter(x, y), Quaternion.identity) as GameObject;
        chessPiece.transform.position = new Vector3(chessPiece.transform.position.x, chessPiece.transform.position.y, 0.0f);
        ChessPiece aPiece = chessPiece.GetComponent<ChessPiece>();

        Utilities.chessBoard[x, y] = aPiece;
        Utilities.chessBoard[x, y].SetPosition(x, y);

        aPiece.healthBar.MaxHealth = maxHealthValue;
        aPiece.healthBar.CurrentHealth = maxHealthValue;
        aPiece.SetShot(shot);

        activePieces.Add(chessPiece);

        Color outlineColor;
        Color fillColor;
        Gradient pieceGradient;
        if (aPiece.isWhite)
        {
            outlineColor = customizer.whitePiecesOutlineColor;
            fillColor = customizer.whitePiecesFillColor;
            pieceGradient = customizer.whitePieceGradient;

            aPiece.selectorAudio.clip = blueSelectorClip;
        }
        else
        {
            outlineColor = customizer.blackPiecesOutlineColor;
            fillColor = customizer.blackPiecesFillColor;
            pieceGradient = customizer.blackPieceGradient;

            aPiece.selectorAudio.clip = redSelectorClip;            
        }

        chessPiece.GetComponent<SpriteRenderer>().color = outlineColor;
        aPiece.healthBar.gameObject.GetComponent<Image>().color = fillColor;
        aPiece.GetComponent<TrailRenderer>().colorGradient = pieceGradient;
        aPiece.targetAimSquare.GetComponent<SpriteRenderer>().color = outlineColor;

        aPiece.selectorAudio.volume = 0.2f;
        aPiece.GetComponent<AudioSource>().clip = moveClip;

        aPiece.id = idCounter;
        idCounter++;
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
            blueCheckText.enabled = true;
        }
        else
        {
            redCheckText.enabled = true;
        }
    }

}
