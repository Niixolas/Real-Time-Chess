using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{

    public float speed;
    public float damage = 1;
    public int playerNum;

    private bool isKnight = false;
    private Vector2 targetSquare;

    [HideInInspector]
    public GameObject instigator;

	void NewStart(int playerNumber)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Controller.getAim(playerNumber) * speed;
        playerNum = playerNumber;
    }

    void SetInstigator(GameObject pieceThatFired)
    {
        instigator = pieceThatFired;
    }

    void SetKnight(Vector2 target)
    {
        isKnight = true;
        targetSquare = target;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 dir = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y) - Utilities.getTileCenter((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
        rb.velocity = dir * speed;
    }

    private void Update()
    {
        if (isKnight)
        {
            Vector2 currentTile = Utilities.getBoardCoordinates(gameObject.transform.position.x, gameObject.transform.position.y);
            if (currentTile == Utilities.getBoardCoordinates(targetSquare.x, targetSquare.y))
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

            Vector2 targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
            if (Mathf.Abs(gameObject.transform.position.x - targetPosition.x) <= 0.2 && Mathf.Abs(gameObject.transform.position.y - targetPosition.y) <= 0.2)
            {
                Destroy(gameObject);
            }
        }
    }
}
