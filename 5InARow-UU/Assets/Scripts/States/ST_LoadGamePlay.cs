using UnityEngine;
using UnityEngine.SceneManagement;

public class ST_LoadGamePlay : State
{
    public override void EnterState()
    {
        SceneManager.LoadScene(1);
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {

    }
}
