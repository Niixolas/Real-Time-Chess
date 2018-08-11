using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            // Knight movement
            if ( ( (Mathf.Abs(CurrentX - x) == 2) && (Mathf.Abs(CurrentY - y) == 1) ) || ( (Mathf.Abs(CurrentX - x) == 1) && (Mathf.Abs(CurrentY - y) == 2) ) )
            {
                return true;
            }

        }
        return false;
    }

    public override void MovePiece()
    {
        Vector2 movement = isWhite ? inputController.p1KnightMove : inputController.p2KnightMove;
        targetSquare = Utilities.getBoardCoordinates(transform.position.x, transform.position.y);
        Vector2 destination = Utilities.getBoardCoordinates(transform.position.x + movement.x, transform.position.y + movement.y);
        if (destination.x <= 7 && destination.x >= 0 && destination.y <= 7 && destination.y >= 0)
        {
            if (isMovePossible((int)destination.x, (int)destination.y, Utilities.chessBoard[(int)destination.x, (int)destination.y]))
            {
                isMoving = true;
                targetSquare = destination;
                targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
                CurrentX = (int)targetPosition.x;
                CurrentY = (int)targetPosition.y;
                Utilities.chessBoard[(int)transform.position.x, (int)transform.position.y] = null;
                Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;
                bm.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                bm.GetComponent<AudioSource>().Play();
            }
        }
    }

    public override void fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            Vector2 aim = playerNumber == 1 ? inputController.p1KnightAim : inputController.p2KnightAim;
            if (aim != Vector2.zero)
            {
                nextFire = Time.time + fireRate;
                GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);

                thisShot.SendMessage("NewStart", playerNumber);
                thisShot.SendMessage("SetInstigator", this.gameObject);
                thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

                Vector2 targetSquare = new Vector2(CurrentX + aim.x, CurrentY + aim.y);
                thisShot.SendMessage("SetKnight", targetSquare);

                thisShot.layer = LayerMask.NameToLayer("Knight");
                Destroy(thisShot, 2);
                healthBar.DealDamage(selfDamagePerShot);
            }

        }
    }

}
