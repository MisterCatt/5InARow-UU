using UnityEngine;

public class ST_Player2Turn : State
{
    public override void EnterState()
    {
        GameController.Instance.IsPlayerTurn = false;
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        GameController.Instance.IsPlayerTurn = true;
    }
}
