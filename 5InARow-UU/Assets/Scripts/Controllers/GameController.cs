using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : StateMachine
{
    public static GameController Instance;

    public bool gameOver = false;

    public bool IsPlayerTurn = false;

    public GameObject Tile, WinScreen;
    public TMP_Text turnText;

    public List<Node> GameMap;

    [SerializeField]
    int totalRoundsAllowed = 0, currentRoundsPlayed = 0;

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

        GameMap = new List<Node>();

        for (int x = 0; x < GameManager.Instance.boardSize.x; x++)
        {
            for (int y = 0; y < GameManager.Instance.boardSize.y; y++)
            {
                var tile = Instantiate(Tile, new Vector3(x, y), Quaternion.identity);

                tile.GetComponent<Tile>().TilePosition = new Vector2(x, y);
                tile.name = $"Tile: {x},{y}";
                tile.GetComponent<Tile>().State = TileState.NEUTRAL;
                GameMap.Add(new Node(new Vector2(x, y), TileState.NEUTRAL));
            }
        }

        totalRoundsAllowed = (int)(GameManager.Instance.boardSize.x * GameManager.Instance.boardSize.y);
    }

    private void Start()
    {
        if(GameManager.Instance)
            GameManager.Instance.SwitchState<ST_GamePlay>();
        SwitchState<ST_PlayerTurn>();
    }

    public Node GetTileAt(Vector2 position) => GameMap.FirstOrDefault(node => node.IsNode(position));

    public void SwapTurn()
    {
        currentRoundsPlayed++;
        CheckWin();
        if (IsPlayerTurn)
        {
            SwitchState<ST_Player2Turn>();
        }
        else
        {
            SwitchState<ST_PlayerTurn>();
        }
        
    }


    public bool CheckRightTile(Node node, TileState state)
    {
        for (int i = 0; i < 5; i++)
        {
            Node n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    public bool CheckUpTile(Node node, TileState state)
    {
        for (int i = 0; i < 5; i++)
        {
            Node n = GetTileAt(new Vector2(node.Position.x, node.Position.y + i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    private bool CheckDiagonalTile(Node node, TileState state)
    {
        for (int i = 0; i < 5; i++)
        {
            Node n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y + i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    private bool CheckDiagonalDownTile(Node node, TileState state)
    {
        for (int i = 0; i < 5; i++)
        {
            Node n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y - i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    public void CheckWin()
    {      
        foreach (Node node in GameMap)
        {
            switch (node.State)
            {
                case TileState.NEUTRAL:
                    continue;
                case TileState.PLAYER1:
                    if (CheckRightTile(node, TileState.PLAYER1) || CheckUpTile(node, TileState.PLAYER1) || CheckDiagonalTile(node, TileState.PLAYER1) || CheckDiagonalDownTile(node, TileState.PLAYER1))
                    {
                        WinScreen.SetActive(true);
                        WinScreen.GetComponentInChildren<TMP_Text>().text = "Blue Win";
                        WinScreen.GetComponentInChildren<TMP_Text>().color = Color.blue;
                        gameOver = true;
                    }
                    break;
                case TileState.PLAYER2:
                    if (CheckRightTile(node, TileState.PLAYER2) || CheckUpTile(node, TileState.PLAYER2) || CheckDiagonalTile(node, TileState.PLAYER2) || CheckDiagonalDownTile(node, TileState.PLAYER2))
                    {
                        WinScreen.SetActive(true);
                        WinScreen.GetComponentInChildren<TMP_Text>().text = "Red Win";
                        WinScreen.GetComponentInChildren<TMP_Text>().color = Color.red;
                        gameOver = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if(!gameOver)
        if (currentRoundsPlayed >= totalRoundsAllowed)
        {
            WinScreen.SetActive(true);
            WinScreen.GetComponentInChildren<TMP_Text>().text = "Game tied";
            WinScreen.GetComponentInChildren<TMP_Text>().color = Color.black;
            return;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

[System.Serializable]
public class Node
{
    public Node(Vector2 pos, TileState st = TileState.NEUTRAL)
    {
        Position = pos;
        this.State = st;
    }

    public Vector2 Position;
    public TileState State;

    public Node Previous;

    public bool IsNode(Vector2 comparedPosition) => (Position == comparedPosition);
}
