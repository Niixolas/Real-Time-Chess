using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionController : MonoBehaviour
{
    public float speed = 2.5f;
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
	void Update ()
    {
        Controller.selectionX = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).x;
        Controller.selectionY = (int)Utilities.getBoardCoordinates(transform.position.x, transform.position.y).y;


        if (isMoving)
        {
            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.2 && Mathf.Abs(targetPosition.y - transform.position.y) < 0.2)
            {
                isMoving = false;
                transform.position = targetPosition;
            }
            else
            {
                float deltaX = targetPosition.x - transform.position.x;
                float deltaY = targetPosition.y - transform.position.y;
                float moveX = 0;
                float moveY = 0;
                if (deltaX != 0)
                {
                    moveX = ( deltaX > 0) ? 1 : -1;
                }
                if (deltaY != 0)
                {
                    moveY = ( deltaY > 0) ? 1 : -1;
                }                
                float newX = transform.position.x + moveX * speed * Time.deltaTime;
                float newY = transform.position.y + moveY * speed * Time.deltaTime;
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
