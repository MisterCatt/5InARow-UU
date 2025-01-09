using UnityEngine;
using UnityEngine.SceneManagement;

public class ST_PlayerTurn : State
{
    public override void EnterState()
    {
        GameController.Instance.IsPlayerTurn = true;
        GameController.Instance.turnText.text = "Blue Turn";
        GameController.Instance.turnText.color = Color.blue;
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        GameController.Instance.IsPlayerTurn = false;
    }
}
