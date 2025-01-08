using UnityEngine;
using UnityEngine.SceneManagement;

public class ST_PlayerTurn : State
{
    public override void EnterState()
    {
        GameController.Instance.IsPlayerTurn = true;
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        GameController.Instance.IsPlayerTurn = false;
    }
}
