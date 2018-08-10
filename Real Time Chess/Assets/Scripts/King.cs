using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            return true;
        }
        return false;
    }

    public override bool isAimPossible(int x, int y)
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

    public override void fire(int playerNumber)
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
}
