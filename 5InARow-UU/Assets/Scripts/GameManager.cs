using UnityEngine;

public class GameManager : StateMachine
{
    public static GameManager Instance;

    bool OpponentHuman = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SwitchState<ST_MainMenu>();
    }

    public void StartGame(bool human)
    {
        OpponentHuman = human;
        SwitchState<ST_LoadGamePlay>();

    }


    void Update()
    {
        UpdateStateMachine();
    }
}
