using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChessAI/Decisions/SelectPieceDecision")]
public class SelectPieceDecision : ChessDecision
{
    public override bool Decide(StateController controller)
    {
        Vector2Int targetLocation = AIUtilities.FindTargetLocation(controller.shortTermPieceToControl);
        if (targetLocation == controller.bm.redSelection)
        {
            AIUtilities.AIPressed = true;
            return true;
        }

        return false;
    }
}
