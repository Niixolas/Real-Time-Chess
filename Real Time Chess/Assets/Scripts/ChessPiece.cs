using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    protected BoardManager bm;
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    [Header("Variables")]
    public bool isWhite;

    [Tooltip("Piece speed")]
    public float speed = 50.0f;

    [Tooltip("Delay between movements")]
    public float moveDelay = 0.2f;

    [Tooltip("Delay between shots")]
    public float fireRate = 0.5F;

    [Tooltip("How much damage the piece takes per shot")]
    public float selfDamagePerShot = 0.5f;

    [Header("References")]
    [Tooltip("Reference to filling healthbar")]
    public HealthBar healthBar;

    [Tooltip("Reference to the pieces hand selection sprite")]
    public SpriteRenderer handSelector;

    [Tooltip("Reference to the pieces selected outline sprite")]
    public SpriteRenderer selectedOutline;

    [Tooltip("Reference to the target move square prefab")]
    public GameObject targetMoveSquare;

    [Tooltip("Reference to the target aim square prefab")]
    public GameObject targetAimSquare;

    [Tooltip("Reference to the explosion prefab")]
    public GameObject explosion;

    [Tooltip("Reference to death explosion prefab")]
    public GameObject deathExplosion;

    [Tooltip("Reference to selector movement audio source")]
    public AudioSource selectorAudio;

    [Tooltip("Reference to the sound when hit")]
    private AudioClip hitClip;

    [HideInInspector]
    // Boolean to trigger whether a piece is moving, or is able to move
    public bool isMoving;

    [HideInInspector]
    public GameObject shot;

    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public float nextMove;

    //[HideInInspector]
    public int id;

    protected Vector2 targetSquare;
    protected Vector2 targetPosition;
    protected float nextFire;

    protected List<GameObject> targetMoveAndAimSquares;

    private AudioSource hitSound;

    private void Awake()
    {
        targetMoveAndAimSquares = new List<GameObject>();
    }

    void Start()
    {
        // Initialize variables
        isMoving = false;
        canMove = true;
        transform.position = Utilities.getTileCenter(CurrentX, CurrentY);
        targetSquare = new Vector2(0, 0);
        targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
        

        // Initialize references
        bm = FindObjectOfType<BoardManager>();
        InputController.Instance = FindObjectOfType<InputController>();

        // Create the audio source for the hit explosion
        hitSound = gameObject.AddComponent<AudioSource>();
        hitClip = FindObjectOfType<BoardManager>().hitClip;
        hitSound.clip = hitClip;
        hitSound.loop = false;
        hitSound.playOnAwake = false;
        hitSound.volume = 0.1f;
    }

    protected virtual void Update()
    {
        if (!canMove)
        {
            nextMove -= Time.deltaTime;
            if (nextMove <= 0)
            {
                canMove = true;
                nextMove = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // If the piece is moving, check if it has reached the destination
            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.05 && Mathf.Abs(targetPosition.y - transform.position.y) < 0.05)
            {
                // Stop the piece and position it at the exact correct position (to correct for minor drift)
                isMoving = false;
                transform.position = targetPosition;

                // Place the piece in the chessboard array so it knows the piece is there
                Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;

                // If the piece is a pawn, check if it reached the other side of the board
                CheckPawnPromotion();

                // Refresh move and aim boxes
                bm.RefreshActions();  

                if (moveDelay != 0)
                {
                    canMove = false;
                    nextMove = moveDelay;
                }
                
            }
            else
            {
                // Continue moving the piece
                float newX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.x - targetPosition.x)));
                float newY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.y - targetPosition.y)));
                transform.position = new Vector2(newX, newY);
            }
        }
    }

    private void CheckPawnPromotion()
    {
        bm.CheckPawnPromotion(this.GetComponent<ChessPiece>(), targetPosition);
    }

    public virtual void MovePiece()
    {
        if (canMove)
        {
            Vector2 movement = isWhite ? InputController.Instance.p1Move : InputController.Instance.p2Move;

            targetSquare = Utilities.getBoardCoordinates(transform.position.x, transform.position.y);

            Vector2 destination = Utilities.getBoardCoordinates(transform.position.x + movement.x, transform.position.y + movement.y);

            if (destination.x <= 7 && destination.x >= 0 && destination.y <= 7 && destination.y >= 0)
            {
                if (IsMovePossible((int)destination.x, (int)destination.y, Utilities.chessBoard[(int)destination.x, (int)destination.y]))
                {
                    isMoving = true;
                    bm.RefreshActions();
                    HidePossibleActions();


                    targetSquare = destination;
                    targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
                    CurrentX = (int)targetPosition.x;
                    CurrentY = (int)targetPosition.y;
                    Utilities.chessBoard[(int)transform.position.x, (int)transform.position.y] = null;
                    Utilities.chessBoard[(int)targetPosition.x, (int)targetPosition.y] = this;
                    //GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                    GetComponent<AudioSource>().Play();

                    if (isWhite)
                    {
                        bm.blueSelection = new Vector2Int((int)targetSquare.x, (int)targetSquare.y);
                    }
                    else
                    {
                        bm.redSelection = new Vector2Int((int)targetSquare.x, (int)targetSquare.y);
                    }
                }
            }
        }
        
    }


    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool IsMovePossible(int x, int y, ChessPiece target)
    {
        return true;
    }

    public virtual bool IsAimPossible(int x, int y)
    {
        return true;
    }

    public virtual void ShowPossibleActions()
    {
        return;
    }

    public virtual List<Vector2Int> PossibleMoves()
    {
        return null;
    }

    public void HidePossibleActions()
    {
        foreach(GameObject g in targetMoveAndAimSquares)
        {
            Destroy(g);
        }
        targetMoveAndAimSquares.Clear();
    }

    public virtual void ShowTarget(ChessPiece[,] chessBoard, int targetDirX, int targetDirY)
    {

    }

    public void SetShot(GameObject aShot)
    {
        shot = aShot;
    }

    public virtual void Fire(int playerNumber)
    {
        if (Time.time > nextFire && !isMoving)
        {
            if (playerNumber == 1)
            {
                if (IsAimPossible((int)InputController.Instance.p1Aim.x, (int)InputController.Instance.p1Aim.y))
                {
                    nextFire = Time.time + fireRate;
                    GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);
                    thisShot.SendMessage("NewStart", playerNumber);
                    thisShot.SendMessage("SetInstigator", this.gameObject);
                    thisShot.GetComponent<AudioSource>().pitch = Random.Range(1.2f, 1.6f);
                    thisShot.GetComponent<AudioSource>().volume = 0.2f;
                    thisShot.GetComponent<AudioSource>().Play();
                    Destroy(thisShot, 2);
                    healthBar.DealDamage(selfDamagePerShot);
                }
            }
            else if (playerNumber == 2)
            {
                if (IsAimPossible((int)InputController.Instance.p2Aim.x, (int)InputController.Instance.p2Aim.y))
                {
                    nextFire = Time.time + fireRate;
                    GameObject thisShot = Instantiate(shot, this.transform.position, this.transform.rotation);
                    thisShot.SendMessage("NewStart", playerNumber);
                    thisShot.SendMessage("SetInstigator", this.gameObject);
                    thisShot.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.0f);
                    thisShot.GetComponent<AudioSource>().volume = 0.2f;
                    thisShot.GetComponent<AudioSource>().Play();
                    Destroy(thisShot, 2);
                    healthBar.DealDamage(selfDamagePerShot);
                }
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
                newExplosion.transform.position = new Vector3(transform.position.x, transform.position.y, -3.0f);
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
