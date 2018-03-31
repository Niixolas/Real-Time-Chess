using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {


    public float CurrentHealth { get; set; }
    public float maxHealth;
    Image healthbar;
    GameObject newGO;
    Text myText;
    Font ArialFont;


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
        MaxHealth = maxHealth;
        //resets health to full on game load
        CurrentHealth = MaxHealth;

        // Get the instance of the health bar
        healthbar = GetComponentInChildren<Image>();

        healthbar.fillAmount = CalculateHealth();

        newGO = new GameObject("myTextGO");
        myText = newGO.AddComponent<Text>();
        ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        myText.font = ArialFont;
        myText.material = ArialFont.material;
        myText.color = Color.yellow;
        myText.fontSize = 11;
        myText.fontStyle = FontStyle.Bold;

        myText.transform.SetParent(this.transform, false);
        myText.transform.Translate(1, -1, 0);


    }

    // Update is called once per frame
    void Update ()
    {        
        myText.text = (Mathf.Floor(CurrentHealth)).ToString();        
    }

    public void DealDamage(float damageValue)
    {
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
        BoardManager bm = FindObjectOfType<BoardManager>();
        if (GetComponentInParent<ChessPiece>().isWhite && bm.greenSelectedPiece == this.GetComponentInParent<ChessPiece>())
        {
            Destroy(GameObject.FindGameObjectWithTag("greenSelector"));
            bm.whiteSelectionBox.GetComponent<SpriteRenderer>().enabled = true;
            bm.whiteSelectionBox.transform.position = Utilities.getTileCenter(bm.greenSelectedPiece.currentX, bm.greenSelectedPiece.currentY);
            bm.greenSelectedPiece = null;
        }
        if (!GetComponentInParent<ChessPiece>().isWhite && bm.redSelectedPiece == this.GetComponentInParent<ChessPiece>())
        {
            Destroy(GameObject.FindGameObjectWithTag("redSelector"));
            bm.blackSelectionBox.GetComponent<SpriteRenderer>().enabled = true;
            bm.blackSelectionBox.transform.position = Utilities.getTileCenter(bm.redSelectedPiece.currentX, bm.redSelectedPiece.currentY);
            bm.redSelectedPiece = null;
        }

        Destroy(transform.parent.gameObject);
    }

}
