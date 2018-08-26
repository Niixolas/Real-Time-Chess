using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public ChessState currentState;
    public ChessState remainState;

    [HideInInspector]
    public int pieceToControl;
    [HideInInspector]
    public int pieceToTarget;
    //[HideInInspector]
    public int longTermTarget;
    //[HideInInspector]
    public int shortTermTarget;
    //[HideInInspector]
    public int[] pieceWeight = new int[32];
    //[HideInInspector]
    public Vector2Int openingTarget;


    public void StartChessAI()
    {
        SetStartWeights();
        longTermTarget = PieceWithHighestWeight();
        FindShortTermTarget(longTermTarget);
    }

    int FindShortTermTarget(int longTermTarget)
    {
        Vector2Int longTermTargetLocation = FindTargetLocation(longTermTarget);
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

        return shortTermTarget;
    }

    Vector2Int FindTargetLocation(int target)
    {
        Vector2Int targetLocation = new Vector2Int(-1, -1);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Utilities.chessBoard[i,j] != null && Utilities.chessBoard[i, j].GetComponent<ChessPiece>().id == target)
                {
                    targetLocation.x = Utilities.chessBoard[i, j].GetComponent<ChessPiece>().CurrentX;
                    targetLocation.y = Utilities.chessBoard[i, j].GetComponent<ChessPiece>().CurrentY;
                }
            }
        }

        return targetLocation;
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
        currentState.UpdateState(this);
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