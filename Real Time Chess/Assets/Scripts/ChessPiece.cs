using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public float speed = 50.0f;
    public bool isWhite;

    [HideInInspector]
    public bool isMoving;

    protected Vector2 targetSquare;
    protected Vector2 targetPosition;

    protected HealthBar healthBar;
    
    public GameObject shot;
    public GameObject explosion;

    public BoardManager bm;

    protected float nextFire = 0.0F;
    public float fireRate = 0.5F;
    public float selfDamagePerShot = 0.5f;

    private AudioSource hitSound;
    public AudioClip hitClip;

    void Start()
    {
        isMoving = false;
        transform.position = Utilities.getTileCenter(currentX, currentY);
        targetSquare = new Vector2(0, 0);
        targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
        bm = FindObjectOfType<BoardManager>();
        hitSound = gameObject.AddComponent<AudioSource>();
        hitSound.clip = hitClip;
        hitSound.loop = false;
        hitSound.playOnAwake = false;
        hitSound.volume = 0.8f;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.05 && Mathf.Abs(targetPosition.y - transform.position.y) < 0.05)
            {
                isMoving = false;
                transform.position = targetPosition;
                Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;
                float thisHealth = healthBar.CurrentHealth;
                if (isWhite && (int)targetPosition.y == 7 && this.GetComponent<Pawn>() != null)
                {
                    FindObjectOfType<BoardManager>().spawnPiece(1, (int)targetPosition.x, 7, (int)thisHealth + 20);
                    FindObjectOfType<BoardManager>().greenSelectedPiece = null;
                    FindObjectOfType<BoardManager>().selectPiece((int)targetPosition.x, (int)targetPosition.y, 1);
                    Destroy(this.gameObject);
                }
                if (!isWhite && (int)targetPosition.y == 0 && this.GetComponent<Pawn>() != null)
                {
                    FindObjectOfType<BoardManager>().spawnPiece(7, (int)targetPosition.x, 0, (int)thisHealth + 20);
                    FindObjectOfType<BoardManager>().redSelectedPiece = null;
                    FindObjectOfType<BoardManager>().selectPiece((int)targetPosition.x, (int)targetPosition.y, 2);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                float newX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.x - targetPosition.x)));
                float newY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.y - targetPosition.y)));
                transform.position = new Vector2(newX, newY);
            }
        }
    }

    public virtual void movePiece()
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
                Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;
                bm.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                bm.GetComponent<AudioSource>().Play();
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

    public virtual void fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            if (isAimPossible((int)Controller.getAim(playerNumber).x, (int)Controller.getAim(playerNumber).y))
            {
                nextFire = Time.time + fireRate;
                GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);
                thisShot.SendMessage("NewStart", playerNumber);
                thisShot.SendMessage("SetInstigator", this.gameObject);
                thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                Destroy(thisShot, 2);
                healthBar.DealDamage(selfDamagePerShot);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet" && collision.gameObject.layer != LayerMask.NameToLayer("Knight"))
        {
            

            if (collision.gameObject.GetComponent<FireBullet>().playerNum == 1 && !isWhite ||
                collision.gameObject.GetComponent<FireBullet>().playerNum == 2 && isWhite)
            {
                hitSound.pitch = Random.Range(0.9f, 1.0f);
                hitSound.Play();
                GameObject newExplosion = Instantiate(explosion, this.transform);
                Destroy(newExplosion, 0.2f);
                healthBar.DealDamage(collision.gameObject.GetComponent<FireBullet>().damage);                
            }
            if (this.gameObject != collision.GetComponent<FireBullet>().instigator)
            {
                Destroy(collision.gameObject);
            }

            
        }
    }
}
