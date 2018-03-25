using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionController : MonoBehaviour
{
    public float speed = 10.0f;
    public int startX = 4;
    public int startY = 3;

    [Range(1, 2)]
    public int playerNumber = 1;

    [HideInInspector]
    public bool isMoving;

    private Vector2 targetSquare;
    private Vector2 targetPosition;

	// Use this for initialization
	void Start ()
    {
        isMoving = false;
        transform.position = Utilities.getTileCenter(startX, startY);
        targetSquare = new Vector2(0, 0);
        targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().enabled == true)
        {
            if (playerNumber == 1)
            {
                Controller.greenSelectionX = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).x;
                Controller.greenSelectionY = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).y;
            }
            if (playerNumber == 2)
            {
                Controller.redSelectionX = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).x;
                Controller.redSelectionY = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).y;
            }

            if (isMoving)
            {
                if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.05 && Mathf.Abs(targetPosition.y - transform.position.y) < 0.05)
                {
                    isMoving = false;
                    transform.position = targetPosition;
                }
                else
                {
                    float newX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.x - targetPosition.x)));
                    float newY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.deltaTime * speed * (1 / Mathf.Abs(transform.position.y - targetPosition.y)));
                    transform.position = new Vector2(newX, newY);
                }
            }
            else
            {
                Vector2 movement = Controller.getMovement(playerNumber);
                targetSquare = Utilities.getBoardCoordinates(transform.position.x, transform.position.y);
                if (movement.x != 0 || movement.y != 0)
                {
                    Vector2 destination = Utilities.getBoardCoordinates(transform.position.x + movement.x, transform.position.y + movement.y);
                    if (destination.x <= 7 && destination.x >= 0 && destination.y <= 7 && destination.y >= 0)
                    {
                        isMoving = true;
                        targetSquare = destination;
                        targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
                    }
                }
                else
                {

                }
            }
        }
    }
}
