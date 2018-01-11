using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public bool isWhite;

    private HealthBar healthBar;
    
    public GameObject shot;

    private float nextFire = 0.0F;
    public float fireRate = 0.5F;
    public int selfDamagePerShot = 1;

    public void setPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual bool isMovePossible(int x, int y, ChessPiece target)
    {
        return true;
    }

    public virtual bool isAimPossible(int x, int y)
    {
        return true;
    }

    public virtual void showTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {

    }

    public void setHealthBar(HealthBar aHealthBar)
    {
        healthBar = aHealthBar;
    }

    public void setShot(GameObject aShot)
    {
        shot = aShot;
    }

    public void fire()
    {
        if (Time.time > nextFire)
        {
            if (isAimPossible((int)Controller.getAim(1).x, (int)Controller.getAim(1).y))
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, this.transform.position, this.transform.rotation);
                healthBar.DealDamage(selfDamagePerShot);
            }

        }
    }
}
