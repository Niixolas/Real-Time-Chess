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

    protected override void Update()
    {
        base.Update();

        if (healthBar.CurrentHealth <= 20)
        {
            FindObjectOfType<BoardManager>().SetCheck(isWhite);
        }
    }

    public override void Fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            if ( ( (playerNumber == 1) ? InputController.Instance.p1Aim : InputController.Instance.p2Aim ) != Vector2.zero)
            {
                nextFire = Time.time + fireRate;
                GameObject thisShot = Instantiate(shot, (Vector2)this.transform.position, this.transform.rotation);

                thisShot.SendMessage("NewStart", playerNumber);
                thisShot.SendMessage("SetInstigator", this.gameObject);
                thisShot.GetComponent<AudioSource>().pitch = playerNumber == 1 ? Random.Range(1.2f, 1.6f) : Random.Range(0.6f, 1.0f);
                thisShot.GetComponent<AudioSource>().volume = 0.2f;
                thisShot.GetComponent<AudioSource>().Play();

                Vector2 aim = playerNumber == 1 ? InputController.Instance.p1Aim : InputController.Instance.p2Aim;
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

                //if (Utilities.chessBoard[x, y] == null)
                //{
                //    Vector3 newMoveSquarePosition = Utilities.getTileCenter(x, y);
                //    newMoveSquarePosition.z = 3.0f;
                //    GameObject newMoveSquare = Instantiate(targetMoveSquare, newMoveSquarePosition, Quaternion.identity);
                //    targetMoveAndAimSquares.Add(newMoveSquare);
                //}
                //else
                if (Utilities.chessBoard[x, y] != null)
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
