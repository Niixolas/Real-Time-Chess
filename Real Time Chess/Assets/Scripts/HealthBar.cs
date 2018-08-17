using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float maxHealth;

    private BoardManager bm;
    
    private Image healthbar;
    private Text myText;

    //GameObject newGO;
    //Font ArialFont;

    public float MaxHealth
    {
        get
        {
            //Some other code
            return maxHealth;
        }
        set
        {
            //Some other code
            maxHealth = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        bm = FindObjectOfType<BoardManager>();

        //resets health to full on game load
        MaxHealth = maxHealth;        
        CurrentHealth = MaxHealth;

        // Get the instance of the health bar
        healthbar = GetComponent<Image>();
        healthbar.fillAmount = CalculateHealth();
        myText = GetComponentInChildren<Text>();

        //newGO = new GameObject("myTextGO");
        //myText = newGO.AddComponent<Text>();
        //ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        //myText.font = ArialFont;
        //myText.material = ArialFont.material;
        //myText.color = Color.yellow;
        //myText.fontSize = 11;
        //myText.fontStyle = FontStyle.Bold;

        //myText.transform.SetParent(this.transform, false);
        //myText.transform.Translate(1, -1, 0);

    }

    // Update is called once per frame
    void Update ()
    {        
        myText.text = (Mathf.Floor(CurrentHealth)).ToString();        
    }

    public void DealDamage(float damageValue)
    {
        myText.GetComponent<Animator>().SetTrigger("wasHit");

        //deduct the damage dealt from the character's health
        CurrentHealth -= damageValue;
        healthbar.fillAmount = CalculateHealth();
        //if the character is out of health, die!
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    void Die()
    {
        CurrentHealth = 0;
        GetComponentInParent<ChessPiece>().HidePossibleActions();

        GetComponentInParent<BoxCollider2D>().enabled = false;

        if (GetComponentInParent<ChessPiece>().isWhite)
        {
            if (bm.blueSelectedPiece == this.GetComponentInParent<ChessPiece>())
            {
                float x = 0.0f;
                Collider2D collider = null;
                LayerMask lm = LayerMask.GetMask("BluePieces");

                do
                {
                    x += 0.5f;
                    collider = Physics2D.OverlapCircle(GetComponentInParent<ChessPiece>().transform.position, x, lm);
                } while (collider == null);

                bm.blueSelection = new Vector2Int(collider.GetComponentInParent<ChessPiece>().CurrentX, collider.GetComponentInParent<ChessPiece>().CurrentY);
                collider.GetComponentInParent<ChessPiece>().glow.enabled = true;

                bm.blueSelectedPiece = null;
            }

            if (GetComponentInParent<ChessPiece>().glow.enabled)
            {
                float x = 0.0f;
                Collider2D collider = null;
                LayerMask lm = LayerMask.GetMask("BluePieces");

                do
                {
                    x += 0.5f;
                    collider = Physics2D.OverlapCircle(GetComponentInParent<ChessPiece>().transform.position, x, lm);
                } while (collider == null);

                bm.blueSelection = new Vector2Int(collider.GetComponentInParent<ChessPiece>().CurrentX, collider.GetComponentInParent<ChessPiece>().CurrentY);
                collider.GetComponentInParent<ChessPiece>().glow.enabled = true;
            }
            
        }
        if (!GetComponentInParent<ChessPiece>().isWhite)
        {
            if (bm.redSelectedPiece == this.GetComponentInParent<ChessPiece>())
            {
                float x = 0.0f;
                Collider2D collider = null;
                LayerMask lm = LayerMask.GetMask("RedPieces");

                do
                {
                    x += 0.5f;
                    collider = Physics2D.OverlapCircle(GetComponentInParent<ChessPiece>().transform.position, x, lm);
                } while (collider == null);

                bm.redSelection = new Vector2Int(collider.GetComponentInParent<ChessPiece>().CurrentX, collider.GetComponentInParent<ChessPiece>().CurrentY);
                collider.GetComponentInParent<ChessPiece>().glow.enabled = true;

                bm.redSelectedPiece = null;
            }

            if (GetComponentInParent<ChessPiece>().glow.enabled)
            {
                float x = 0.0f;
                Collider2D collider = null;
                LayerMask lm = LayerMask.GetMask("RedPieces");

                do
                {
                    x += 0.5f;
                    collider = Physics2D.OverlapCircle(GetComponentInParent<ChessPiece>().transform.position, x, lm);
                } while (collider == null);

                bm.redSelection = new Vector2Int(collider.GetComponentInParent<ChessPiece>().CurrentX, collider.GetComponentInParent<ChessPiece>().CurrentY);
                collider.GetComponentInParent<ChessPiece>().glow.enabled = true;
            }
            
        }

        if (transform.parent.gameObject.GetComponent<King>())
        {
            FindObjectOfType<Menu_Controller>().setWinner(!transform.parent.gameObject.GetComponent<ChessPiece>().isWhite);
        }

        Utilities.chessBoard[GetComponentInParent<ChessPiece>().CurrentX, GetComponentInParent<ChessPiece>().CurrentY] = null;

        bm.RefreshActions();

        Destroy(transform.parent.gameObject);
    }

}
