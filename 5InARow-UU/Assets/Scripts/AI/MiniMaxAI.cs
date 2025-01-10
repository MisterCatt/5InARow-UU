using UnityEngine;

public class MiniMaxAI : AIController
{
    public override void MakeMove()
    {
        CalculateMove();
    }
    protected override void CalculateMove()
    {
        Debug.Log("MINIMAX MADE A MOVE");
    }
}
