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

        AIUtilities.AIMove = move;

        //if (controller.bm.redSelection != targetLocation)
        //{
        //    if (targetLocation.x - controller.bm.redSelection.x < 0)
        //    {
        //        AIUtilities.AIMove.x = -1;
        //    }
        //    else if (targetLocation.x - controller.bm.redSelection.x > 0)
        //    {
        //        AIUtilities.AIMove.x = 1;
        //    }
        //    else
        //    {
        //        AIUtilities.AIMove.x = 0;
        //    }

        //    if (targetLocation.y - controller.bm.redSelection.y < 0)
        //    {
        //        AIUtilities.AIMove.y = -1;
        //    }
        //    else if (targetLocation.y - controller.bm.redSelection.y > 0)
        //    {
        //        AIUtilities.AIMove.y = 1;
        //    }
        //    else
        //    {
        //        AIUtilities.AIMove.y = 0;
        //    }
        //}
    }
}
