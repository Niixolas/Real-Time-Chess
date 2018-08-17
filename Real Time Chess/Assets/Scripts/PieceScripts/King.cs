using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool IsMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            return true;
        }
        return false;
    }

    public override bool IsAimPossible(int x, int y)
    {
        return true;
    }

    private void Update()
    {
        if (healthBar.CurrentHealth <= 10)
        {
            FindObjectOfType<BoardManager>().SetCheck(isWhite);
        }
    }

    public override void Fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            if ( ( (playerNumber == 1) ? inputController.p1Aim : inputController.p2Aim ) != Vector2.zero)
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

    public override void ShowPossibleActions()
    {
        for (int xDir = -1; xDir <= 1; xDir += 1)
        {
            for (int yDir = -1; yDir <= 1; yDir += 1)
            {
                int x = CurrentX + xDir;
                int y = CurrentY + yDir;

                if (x < 0 || x > 7 || y < 0 || y > 7 || (xDir == 0 && yDir == 0) )
                {
                    continue;
                }

                if (Utilities.chessBoard[x, y] == null)
                {
                    Vector3 newMoveSquarePosition = Utilities.getTileCenter(x, y);
                    newMoveSquarePosition.z = 3.0f;
                    GameObject newMoveSquare = Instantiate(targetMoveSquare, newMoveSquarePosition, Quaternion.identity);
                    targetMoveAndAimSquares.Add(newMoveSquare);
                }
                else
                {
                    if (Utilities.chessBoard[x, y].isWhite != isWhite)
                    {
                        Vector3 newMoveSquarePosition = Utilities.getTileCenter(x, y);
                        newMoveSquarePosition.z = 3.0f;
                        GameObject newAimSquare = Instantiate(targetAimSquare, newMoveSquarePosition, Quaternion.identity);
                        targetMoveAndAimSquares.Add(newAimSquare);
                    }
                    continue;
                }
                
            }
        }
    }

}
