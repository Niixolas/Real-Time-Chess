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
                if (inputController.p1Aim != Vector2.zero && IsAimPossible((int)inputController.p1Aim.x, (int)inputController.p1Aim.y))
                {
                    nextFire = Time.time + fireRate;
                    GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);

                    thisShot.SendMessage("NewStart", playerNumber);
                    thisShot.SendMessage("SetInstigator", this.gameObject);
                    thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

                    Vector2 aim = playerNumber == 1 ? inputController.p1Aim : inputController.p2Aim;
                    Vector2 targetSquare = new Vector2(CurrentX + aim.x, CurrentY + aim.y);
                    thisShot.SendMessage("SetPawnOrKing", targetSquare);

                    Destroy(thisShot, 2);
                    healthBar.DealDamage(selfDamagePerShot);
                }
            }
        }
    }

    public override void ShowPossibleActions()
    {
        int moveLookX = CurrentX;
        int moveLookY = isWhite ? CurrentY + 1 : CurrentY - 1;

        if (Utilities.chessBoard[moveLookX, moveLookY] == null)
        {
            Vector3 newMoveSquarePosition = Utilities.getTileCenter(moveLookX, moveLookY);
            newMoveSquarePosition.z = -5.0f;
            GameObject newMoveSquare = Instantiate(targetMoveSquare, newMoveSquarePosition, Quaternion.identity);
            targetMoveAndAimSquares.Add(newMoveSquare);
        }

        List<Vector2Int> aimLook = new List<Vector2Int>();
        Vector2Int aimLook1 = new Vector2Int(CurrentX + 1, isWhite ? CurrentY + 1 : CurrentY - 1);
        Vector2Int aimLook2 = new Vector2Int(CurrentX - 1, isWhite ? CurrentY + 1 : CurrentY - 1);

        aimLook.Add(aimLook1);
        aimLook.Add(aimLook2);

        foreach (Vector2Int aimLookVector in aimLook)
        {
            if (aimLookVector.x >= 0 && aimLookVector.x <= 7 && aimLookVector.y >= 0 && aimLookVector.y <= 7)
            {
                Vector3 newAimSquarePosition = Utilities.getTileCenter(aimLookVector.x, aimLookVector.y);
                newAimSquarePosition.z = -5.0f;
                GameObject newAimSquare = Instantiate(targetAimSquare, newAimSquarePosition, Quaternion.identity);
                targetMoveAndAimSquares.Add(newAimSquare);
            }
            
        }
    }
}
