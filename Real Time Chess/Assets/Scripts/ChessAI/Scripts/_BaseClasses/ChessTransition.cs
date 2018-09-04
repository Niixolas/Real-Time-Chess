using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChessTransition
{
    public ChessDecision decision;
    public ChessState trueState;
    public ChessState falseState;
}