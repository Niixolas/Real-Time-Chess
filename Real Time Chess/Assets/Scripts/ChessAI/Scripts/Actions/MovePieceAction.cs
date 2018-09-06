using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChessAI/Actions/MovePieceAction")]
public class MovePieceAction : ChessAction
{
    public override void Act(StateController controller)
    {
        AIUtilities.AIMove = Vector2.zero;

        List<Vector2Int> possibleMoves = controller.bm.redSelectedPiece.PossibleMoves();

        if (possibleMoves.Count == 0)
        {
            return;
        }

        Vector2Int bestMove = possibleMoves[0];
        Vector2Int targetLocation = AIUtilities.FindTargetLocation(controller.shortTermTarget);

        foreach (Vector2Int move in possibleMoves)
        {
            if ( (targetLocation - move).magnitude <= (targetLocation - bestMove).magnitude )
            {
                bestMove = move;
            }
        }

        Vector2 direction = bestMove - controller.bm.redSelection;
        direction.Normalize();

        AIUtilities.AIMove = direction;
    }
}
