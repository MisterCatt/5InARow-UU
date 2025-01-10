using UnityEngine;

public class ST_AITurn : State
{
    public override void EnterState()
    {
        GameController.Instance.IsPlayerTurn = false;
        GameController.Instance.turnText.text = "Red Turn";
        GameController.Instance.turnText.color = Color.red;

        AIController.Instance.MakeMove();
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        GameController.Instance.IsPlayerTurn = true;
    }
}
