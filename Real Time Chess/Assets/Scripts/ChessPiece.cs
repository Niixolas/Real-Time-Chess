using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public bool isWhite;

    HealthBar healthBar;
    GameObject shot;
    private float nextFire = 0.0F;
    public float fireRate = 0.5F;

    public Transform lightSelect;

    public void Start()
    {
    }

    public void setPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual bool isMovePossible(int x, int y, ChessPiece target)
    {
        return true;
    }

    public virtual void showTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {

    }

    ///*
    public void setHealthBar(HealthBar aHealthBar)
    {
        healthBar = aHealthBar;
    }
    //*/

    public void setShot(GameObject aShot)
    {
        shot = aShot;
    }

    public void fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //transform shotTransform = new transform;
            Instantiate(shot, this.transform.position, this.transform.rotation);
        }
    }

    void Update()
    {

    }

}
