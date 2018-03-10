using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour {

    public float speed;

	void NewStart(int playerNumber)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Controller.getAim(playerNumber) * speed;
    }
}
