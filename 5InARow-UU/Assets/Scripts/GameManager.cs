using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StateMachine
{
    [SerializeField]
    private bool isMainManager = false;

    public static GameManager Instance;

    public bool OpponentHuman = false;
    public int AIDifficulty = 0;

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

    public void StartGame()
    {
        OpponentHuman = true;
        SceneManager.LoadScene(1);
    }

    public void StartEasyGame()
    {
        AIDifficulty = 0;
        OpponentHuman = false;
        SceneManager.LoadScene(1);
    }

    public void StartHardGame()
    {
        AIDifficulty = 1;
        OpponentHuman = false;
        SceneManager.LoadScene(1);
    }


    void Update()
    {
        UpdateStateMachine();
    }
}
