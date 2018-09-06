using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChessAI/Decisions/ChangePieceDecision")]
public class ChangePieceDecision : ChessDecision
{
    public override bool Decide(StateController controller)
    {
        if (controller.decisionTimer <= 0)
        {
            controller.decisionTimer = controller.decisionTime;

            if (!controller.CanPieceReachTarget(new Vector2Int(controller.bm.redSelectedPiece.CurrentX, controller.bm.redSelectedPiece.CurrentY), controller.openingTarget))
            {
                AIUtilities.AIPressed = true;
                controller.ChooseNewPiece();
                return true;
            }
        }


        return false;
    }
}
