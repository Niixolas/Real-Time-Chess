using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChessAI/Decisions/ChangePieceDecision")]
public class ChangePieceDecision : ChessDecision
{
    public override bool Decide(StateController controller)
    {
        return false;
    }
}
