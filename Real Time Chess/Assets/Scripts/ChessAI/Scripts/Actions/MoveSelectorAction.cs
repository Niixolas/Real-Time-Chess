using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChessAI/Actions/MoveSelectorAction")]
public class MoveSelectorAction : ChessAction
{
    public override void Act(StateController controller)
    {
        AIUtilities.AIMove = new Vector2(0, 0);

        Vector2Int targetLocation = AIUtilities.FindTargetLocation(controller.shortTermPieceToControl);

        Vector2 move = targetLocation - controller.bm.redSelection;
        move.Normalize();

        AIUtilities.AIPressed = true;
        AIUtilities.AIMove = move;
    }
}
