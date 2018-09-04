using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override bool IsMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            if (x == this.CurrentX)
            {
                if (isWhite && y > this.CurrentY)
                {
                    return true;
                }
                if (!isWhite && y < this.CurrentY)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool IsAimPossible(int x, int y)
    {
        if (isWhite && x != 0 && y > 0)
        {
            return true;
        }
        if (!isWhite && x != 0 && y < 0)
        {
            return true;
        }
        return false;
    }

    public override void Fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            if (playerNumber == 1)
            {
                if (InputController.Instance.p1Aim != Vector2.zero && IsAimPossible((int)InputController.Instance.p1Aim.x, (int)InputController.Instance.p1Aim.y))
                {
                    nextFire = Time.time + fireRate;
                    GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);

                    thisShot.SendMessage("NewStart", playerNumber);
                    thisShot.SendMessage("SetInstigator", this.gameObject);
                    thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

                    Vector2 aim = InputController.Instance.p1Aim;
                    Vector2 targetSquare = new Vector2(CurrentX + aim.x, CurrentY + aim.y);
                    thisShot.SendMessage("SetPawnOrKing", targetSquare);

                    Destroy(thisShot, 2);
                    healthBar.DealDamage(selfDamagePerShot);
                }
            }

            if (playerNumber == 2)
            {
                if (InputController.Instance.p2Aim != Vector2.zero && IsAimPossible((int)InputController.Instance.p2Aim.x, (int)InputController.Instance.p2Aim.y))
                {
                    nextFire = Time.time + fireRate;
                    GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);

                    thisShot.SendMessage("NewStart", playerNumber);
                    thisShot.SendMessage("SetInstigator", this.gameObject);
                    thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

                    Vector2 aim = InputController.Instance.p2Aim;
                    Vector2 targetSquare = new Vector2(CurrentX + aim.x, CurrentY + aim.y);
                    thisShot.SendMessage("SetPawnOrKing", targetSquare);

                    Destroy(thisShot, 2);
                    healthBar.DealDamage(selfDamagePerShot);
                }
            }
        }
    }

    public override List<Vector2Int> PossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int moveX = CurrentX;
        int moveY = isWhite ? CurrentY + 1 : CurrentY - 1;

        if (Utilities.chessBoard[moveX, moveY] == null)
        {
            moves.Add(new Vector2Int(moveX, moveY));
        }

        return moves;
    }

    public override void ShowPossibleActions()
    {
        //int moveLookX = CurrentX;
        //int moveLookY = isWhite ? CurrentY + 1 : CurrentY - 1;

        //if (Utilities.chessBoard[moveLookX, moveLookY] == null)
        //{
        //    Vector3 newMoveSquarePosition = Utilities.getTileCenter(moveLookX, moveLookY);
        //    newMoveSquarePosition.z = 3.0f;
        //    GameObject newMoveSquare = Instantiate(targetMoveSquare, newMoveSquarePosition, Quaternion.identity);
        //    targetMoveAndAimSquares.Add(newMoveSquare);
        //}

        int aimLookY = isWhite ? CurrentY + 1 : CurrentY - 1;

        for (int dirX = -1; dirX <= 1; dirX += 2)
        {
            if (CurrentX + dirX < 0 || CurrentX + dirX > 7)
            {
                continue;
            }
            else if (Utilities.chessBoard[CurrentX + dirX, aimLookY] != null)
            {
                if (Utilities.chessBoard[CurrentX + dirX, aimLookY].isWhite != isWhite)
                {
                    Vector3 newAimSquarePosition = Utilities.getTileCenter(CurrentX + dirX, aimLookY);
                    newAimSquarePosition.z = 3.0f;
                    GameObject newAimSquare = Instantiate(targetAimSquare, newAimSquarePosition, Quaternion.identity);
                    targetMoveAndAimSquares.Add(newAimSquare);
                }
            }
        }
    }
}
