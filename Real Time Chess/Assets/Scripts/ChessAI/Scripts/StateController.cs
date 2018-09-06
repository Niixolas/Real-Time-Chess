using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Checker
{
    public Vector2Int location;
    public int distance;
}

public class StateController : MonoBehaviour
{
    public BoardManager bm;

    public ChessState currentState;
    public ChessState remainState;

    //[HideInInspector]
    public int pieceToControl;
    //[HideInInspector]
    public Vector2 squareToMoveControlPiece;
    //[HideInInspector]
    public int shortTermPieceToControl;
    //[HideInInspector]
    public int pieceToTarget;
    //[HideInInspector]
    public int longTermTarget;
    //[HideInInspector]
    public int shortTermTarget;
    //[HideInInspector]
    public int[] pieceWeight = new int[32];
    //[HideInInspector]
    public Vector2Int openingTarget;


    public float decisionTime = 1.0f;
    public float decisionTimer = 1.0f;

    public void StartChessAI()
    {
        SetStartWeights();
        longTermTarget = PieceWithHighestWeight();
        FindShortTermTarget(longTermTarget);
        FindDesiredPieceToControl();
    }

    void FindDesiredPieceToControl()
    {
        do
        {
            int controlPiece = Random.Range(1, 16) + 16;

            pieceToControl = controlPiece;

            //if (CanPieceMove(pieceToControl))
            //{
                shortTermPieceToControl = pieceToControl;
            //}

        } while (!CanPieceMove(shortTermPieceToControl));
        

        //squareToMoveControlPiece = FindDesiredMovement(AIUtilities.FindTargetLocation(shortTermPieceToControl), AIUtilities.FindTargetLocation(shortTermTarget));

    }


    public void ChooseNewPiece()
    {
        shortTermPieceToControl = Random.Range(1, 16) + 16;
    }

    public bool CanPieceReachTarget(Vector2Int mySquare, Vector2Int targetSquare)
    {
        if (bm.redSelectedPiece is Pawn)
        {
            if (mySquare.x != targetSquare.x)
            {
                return false;
            }
        }

        return true;
    }


    public Vector2Int FindDesiredMovement(Vector2Int mySquare, Vector2Int targetSquare)
    {
        List<Checker> possibleSquares = new List<Checker>();

        for (int y = Mathf.Max(0, mySquare.y - 1); y <= Mathf.Min(7, mySquare.y + 1); y++)
        {
            for (int x = Mathf.Max(0, mySquare.x - 1); x <= Mathf.Min(7, mySquare.x + 1); x++)
            {
                if (!(x == 0 && y == 0))
                {
                    ChessPiece p = Utilities.chessBoard[mySquare.x, mySquare.y].GetComponent<ChessPiece>();
                    int checkX = x;
                    int checkY = y;
                    if (p.IsMovePossible(checkX, checkY, Utilities.chessBoard[checkX, checkY]))
                    {
                        Checker c = new Checker
                        {
                            location = new Vector2Int(checkX, checkY),
                            distance = Mathf.Abs(checkX - targetSquare.x) + Mathf.Abs(checkY - targetSquare.y)
                        };
                        possibleSquares.Add(c);
                    }
                }
            }
        }


        Checker bestMove = possibleSquares.Count == 0 ? null : possibleSquares[0];
        if (bestMove != null)
        {
            foreach (Checker c in possibleSquares)
            {
                if (c.distance <= bestMove.distance)
                {
                    bestMove = c;
                }
            }
        }       

        return bestMove == null ? new Vector2Int(-1, -1) : bestMove.location;
    }


    bool CanPieceMove(int piece)
    {
        Vector2Int pieceLocation = AIUtilities.FindTargetLocation(piece);

        bool canMove = false;

        if (piece == 20 || piece == 21)
        {
            return true;
        }
        else
        {
            for (int y = Mathf.Max(0, pieceLocation.y - 1); y <= Mathf.Min(7, pieceLocation.y + 1); y++)
            {
                for (int x = Mathf.Max(0, pieceLocation.x - 1); x <= Mathf.Min(7, pieceLocation.x + 1); x++)
                {
                    if (!(x == pieceLocation.x && y == pieceLocation.y))
                    {
                        ChessPiece p = Utilities.chessBoard[pieceLocation.x, pieceLocation.y].GetComponent<ChessPiece>();

                        if (p.IsMovePossible(x, y, Utilities.chessBoard[x, y]))
                        {
                            canMove = true;
                        }
                    }
                }
            }
        }        

        return canMove;
    }


    void FindShortTermTarget(int longTermTarget)
    {
        Vector2Int longTermTargetLocation = AIUtilities.FindTargetLocation(longTermTarget);
        bool targetFound = false;

        if (longTermTargetLocation.x < 0 || longTermTargetLocation.y < 0)
        {
            // target not found on board
        }

        for (int distance = 1; distance < 6; distance++)
        {
            for (int y = Mathf.Max(0, longTermTargetLocation.y - distance); y <= Mathf.Min(7, longTermTargetLocation.y + distance); y++)
            {
                for (int x = Mathf.Max(0, longTermTargetLocation.x - distance); x <= Mathf.Min(7, longTermTargetLocation.x + distance); x++)
                {
                    if (Mathf.Abs(longTermTargetLocation.x - x) == distance || Mathf.Abs(longTermTargetLocation.y - y) == distance)
                    {
                        if (Utilities.chessBoard[x, y] == null)
                        {
                            openingTarget = new Vector2Int(x, y);

                            Vector2Int checkShortTerm = Vector2Int.zero;

                            if (openingTarget.x > longTermTargetLocation.x)
                            {
                                checkShortTerm.x = -1;
                            }
                            else if (openingTarget.x < longTermTargetLocation.x)
                            {
                                checkShortTerm.x = 1;
                            }
                            else
                            {
                                checkShortTerm.x = 0;
                            }

                            if (openingTarget.y > longTermTargetLocation.y)
                            {
                                checkShortTerm.y = -1;
                            }
                            else if (openingTarget.y < longTermTargetLocation.y)
                            {
                                checkShortTerm.y = 1;
                            }
                            else
                            {
                                checkShortTerm.y = 0;
                            }

                            shortTermTarget = Utilities.chessBoard[openingTarget.x + checkShortTerm.x,
                                                                   openingTarget.y + checkShortTerm.y]
                                                                   .GetComponent<ChessPiece>().id;

                            targetFound = true;
                        }
                    }
                }
            }

            if (targetFound)
            {
                break;                
            }
        }

        if (!targetFound)
        {
            shortTermTarget = longTermTarget;
        }
    }



    int PieceWithHighestWeight()
    {
        int highestPiece = 0;
        for (int i = 1; i < 32; i++)
        {
            if (pieceWeight[i] > pieceWeight[highestPiece])
            {
                highestPiece = i;
            }
        }
        return highestPiece;
    }

    void Update()
    {
        //AIUtilities.AIPressed = false;
        if (bm.gameStarted)
        {
            decisionTimer -= Time.deltaTime;

            currentState.UpdateState(this);
        }
        
    }

    void OnDrawGizmos()
    {
    }

    public void TransitionToState(ChessState nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    private void OnExitState()
    {
    }

    void SetStartWeights()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Utilities.chessBoard[i, j] != null && Utilities.chessBoard[i, j].GetComponent<ChessPiece>().isWhite)
                {
                    int weight = 0;

                    if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is Pawn)
                    {
                        weight = 10;
                    }
                    else if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is Rook)
                    {
                        weight = 15;
                    }
                    else if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is Knight)
                    {
                        weight = 15;
                    }
                    else if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is Bishop)
                    {
                        weight = 15;
                    }
                    else if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is Queen)
                    {
                        weight = 25;
                    }
                    else if (Utilities.chessBoard[i, j].GetComponent<ChessPiece>() is King)
                    {
                        weight = 50;
                    }

                    pieceWeight[Utilities.chessBoard[i, j].GetComponent<ChessPiece>().id] = weight;
                }
            }
        }
    }
}