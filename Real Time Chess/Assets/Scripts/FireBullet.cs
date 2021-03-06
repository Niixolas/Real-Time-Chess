﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed;
    public float damage = 1;
    public int playerNum;

    private bool isKnight = false;
    private bool isPawnOrKing = false;
    private Vector2 targetSquare;

    [HideInInspector]
    public GameObject instigator;

    private void Awake()
    {
    }

    void NewStart(int playerNumber)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = (playerNumber == 1 ? InputController.Instance.p1Aim : InputController.Instance.p2Aim) * speed;
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
        //rb.transform.position += playerNum == 1 ? (Vector3)InputController.Instance.p1KnightAim * 0.3f : (Vector3)InputController.Instance.p2KnightAim * 0.3f;
        GetComponent<Animator>().enabled = true;
    }

    void SetPawnOrKing(Vector2 target)
    {
        isPawnOrKing = true;
        targetSquare = target;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 dir = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y) - Utilities.getTileCenter((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
        rb.velocity = dir * speed;
        //rb.transform.position += playerNum == 1 ? (Vector3)InputController.Instance.p1Aim * 0.5f : (Vector3)InputController.Instance.p2Aim * 0.5f;
    }

    private void Update()
    {
        if (isKnight || isPawnOrKing)
        {
            Vector2 currentTile = Utilities.getBoardCoordinates(gameObject.transform.position.x, gameObject.transform.position.y);
            if (currentTile == Utilities.getBoardCoordinates(targetSquare.x, targetSquare.y))
            {
                gameObject.layer = LayerMask.NameToLayer("Bullets");
            }

            Vector2 targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
            if (Mathf.Abs(gameObject.transform.position.x - targetPosition.x) <= 0.1f && Mathf.Abs(gameObject.transform.position.y - targetPosition.y) <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
