using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    private BoardManager bm;
    
    private Image healthbar;
    private Text myText;

    // Use this for initialization
    void Start ()
    {
        bm = FindObjectOfType<BoardManager>();

        //resets health to full on game load
        CurrentHealth = MaxHealth;

        // Get the instance of the health bar
        healthbar = GetComponent<Image>();
        healthbar.fillAmount = CalculateHealth();
        myText = GetComponentInChildren<Text>();
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

        // Disable aim boxes and collider
        GetComponentInParent<ChessPiece>().HidePossibleActions();        
        GetComponentInParent<BoxCollider2D>().enabled = false;

        // Get Particle system for death explosion
        ParticleSystem ps;
        ParticleSystem.MainModule main;
        ParticleSystem.TrailModule trails;

        ps = GetComponentInParent<ChessPiece>().deathExplosion.GetComponent<ParticleSystem>();
        main = ps.main;
        trails = ps.trails;

        // Set death explosion colors based on customizer settings
        if (GetComponentInParent<ChessPiece>().isWhite)
        {
            main.startColor = bm.customizer.whitePiecesOutlineColor;
            trails.colorOverLifetime = bm.customizer.whitePieceGradient;
        }
        else
        {
            main.startColor = bm.customizer.blackPiecesOutlineColor;
            trails.colorOverLifetime = bm.customizer.blackPieceGradient;
        }

        // Create the death effect
        GameObject deathEffect = Instantiate(GetComponentInParent<ChessPiece>().deathExplosion, transform.parent.gameObject.transform.position, Quaternion.identity);
        Destroy(deathEffect, 1.0f);

        // If the piece was a king, set the winner for the game.
        if (transform.parent.gameObject.GetComponent<King>())
        {
            FindObjectOfType<Menu_Controller>().setWinner(!transform.parent.gameObject.GetComponent<ChessPiece>().isWhite);
        }
        else
        {
            // If the piece is selected, move the selector to a nearby piece
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
                    bm.SelectPiece(bm.blueSelection.x, bm.blueSelection.y, 1);
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
                    bm.SelectPiece(bm.redSelection.x, bm.redSelection.y, 2);
                }
            }

            Utilities.chessBoard[GetComponentInParent<ChessPiece>().CurrentX, GetComponentInParent<ChessPiece>().CurrentY] = null;

            bm.RefreshActions();

            Destroy(transform.parent.gameObject);
        }
    }
}
