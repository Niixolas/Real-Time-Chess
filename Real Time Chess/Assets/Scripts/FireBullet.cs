using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {

    public float speed;
    public float damage = 1;
    public int playerNum;

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
}
