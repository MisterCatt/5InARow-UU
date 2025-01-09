using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StateMachine
{
    [SerializeField]
    private bool isMainManager = false;

    public static GameManager Instance;

    bool OpponentHuman = false;

    public Vector2 boardSize = new Vector2(15,15);

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(isMainManager)
            SwitchState<ST_MainMenu>();
    }

    public void StartGame(bool human)
    {
        OpponentHuman = human;
        SceneManager.LoadScene(1);
    }


    void Update()
    {
        UpdateStateMachine();
    }
}
