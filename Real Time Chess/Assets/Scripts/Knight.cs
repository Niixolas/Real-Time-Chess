﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool isMovePossible(int x, int y, ChessPiece target)
    {
        if (target == null)
        {
            // Knight movement
            if ( ( (Mathf.Abs(currentX - x) == 2) && (Mathf.Abs(currentY - y) == 1) ) || ( (Mathf.Abs(currentX - x) == 1) && (Mathf.Abs(currentY - y) == 2) ) )
            {
                return true;
            }

        }
        return false;
    }

    public override void movePiece()
    {
        Vector2 movement = Controller.getKnightMovement(isWhite ? 1 : 2);
        targetSquare = Utilities.getBoardCoordinates(transform.position.x, transform.position.y);
        Vector2 destination = Utilities.getBoardCoordinates(transform.position.x + movement.x, transform.position.y + movement.y);
        if (destination.x <= 7 && destination.x >= 0 && destination.y <= 7 && destination.y >= 0)
        {
            if (isMovePossible((int)destination.x, (int)destination.y, Utilities.chessBoard[(int)destination.x, (int)destination.y]))
            {
                isMoving = true;
                targetSquare = destination;
                targetPosition = Utilities.getTileCenter((int)targetSquare.x, (int)targetSquare.y);
                currentX = (int)targetPosition.x;
                currentY = (int)targetPosition.y;
                Utilities.chessBoard[(int)transform.position.x, (int)transform.position.y] = null;
            }
        }
    }

}
