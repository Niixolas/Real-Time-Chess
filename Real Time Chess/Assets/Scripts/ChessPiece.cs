using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public float speed = 5.0f;
    public bool isWhite;

    [HideInInspector]
    public bool isMoving;

    private Vector2 targetSquare;
    private Vector2 targetPosition;

    private HealthBar healthBar;
    
    public GameObject shot;

    private float nextFire = 0.0F;
    public float fireRate = 0.5F;
    public int selfDamagePerShot = 1;

    void Start()
    {
        isMoving = false;
        transform.position = Utilities.getTileCenter(currentX, currentY);
        targetSquare = new Vector2(0, 0);
        targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.2 && Mathf.Abs(targetPosition.y - transform.position.y) < 0.2)
            {
                isMoving = false;
                transform.position = targetPosition;
                Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;
            }
            else
            {
                float deltaX = targetPosition.x - transform.position.x;
                float deltaY = targetPosition.y - transform.position.y;
                float moveX = 0;
                float moveY = 0;
                if (deltaX != 0)
                {
                    moveX = (deltaX > 0) ? 1 : -1;
                }
                if (deltaY != 0)
                {
                    moveY = (deltaY > 0) ? 1 : -1;
                }
                float newX = transform.position.x + moveX * speed * Time.deltaTime;
                float newY = transform.position.y + moveY * speed * Time.deltaTime;
                transform.position = new Vector2(newX, newY);
            }
        }
    }

    public void movePiece()
    { 
        Vector2 movement = Controller.getMovement(isWhite ? 1 : 2);
        targetSquare = Utilities.getBoardCoordinates(transform.position.x, transform.position.y);
        Vector2 destination = Utilities.getBoardCoordinates(transform.position.x + movement.x, transform.position.y + movement.y);
        if (destination.x <= 7 && destination.x >= 0 && destination.y <= 7 && destination.y >= 0)
        {
            if (isMovePossible((int)destination.x, (int)destination.y, Utilities.chessBoard[(int)destination.x, (int)destination.y]))
            {
                isMoving = true;
                targetSquare = destination;
                targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
                currentX = (int)targetPosition.x;
                currentY = (int)targetPosition.y;
                Utilities.chessBoard[(int)transform.position.x, (int)transform.position.y] = null;
            }
        }
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

    public void fire(int playerNumber)
    {
        if (Time.time > nextFire)
        {
            if (isAimPossible((int)Controller.getAim(playerNumber).x, (int)Controller.getAim(playerNumber).y))
            {
                nextFire = Time.time + fireRate;
                GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);
                thisShot.SendMessage("NewStart", playerNumber);
                Destroy(thisShot, 2);
                healthBar.DealDamage(selfDamagePerShot);
            }

        }
    }
}
