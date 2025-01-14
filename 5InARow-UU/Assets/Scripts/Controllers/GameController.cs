using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : StateMachine
{
    public static GameController Instance;

    public bool gameOver;

    public bool IsPlayerTurn = false;

    public GameObject Tile, WinScreen;
    public TMP_Text turnText;

    public Node[,] GameMap;
    public List<Node> placedNodes;

    public List<GameObject> AITypes;

    [SerializeField] private int totalRoundsAllowed, currentRoundsPlayed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        GameMap = new Node[15,15];

        for (var x = 0; x < 15; x++)
        for (var y = 0; y < 15; y++)
        {
            var tile = Instantiate(Tile, new Vector3(x, y), Quaternion.identity);

            tile.GetComponent<Tile>().TilePosition = new Vector2(x, y);
            tile.name = $"{x},{y}";
            tile.GetComponent<Tile>().State = TileState.NEUTRAL;
            GameMap[x, y] = new Node(new Vector2(x, y));
        }

        totalRoundsAllowed = 15 * 15;

        placedNodes = new List<Node>();

        if (GameManager.Instance)
            if(!GameManager.Instance.OpponentHuman)
                AITypes[GameManager.Instance.AIDifficulty].SetActive(true);
    }

    private void Start()
    {
        if (GameManager.Instance)
            GameManager.Instance.SwitchState<ST_GamePlay>();
        SwitchState<ST_PlayerTurn>();
    }

    public Node GetTileAt(Vector2 position)
    {
        return GameMap[(int)position.x, (int)position.y];
    }

    public void SwapTurn()
    {
        currentRoundsPlayed++;
        CheckWin();

        if (GameManager.Instance.OpponentHuman)
        {
            if (IsPlayerTurn)
                SwitchState<ST_Player2Turn>();
            else
                SwitchState<ST_PlayerTurn>();
        }
        else
        {
            if (IsPlayerTurn)
                SwitchState<ST_AITurn>();
            else
                SwitchState<ST_PlayerTurn>();
        }
    }


    public bool CheckRightTile(Node node, TileState state)
    {
        for (var i = 0; i < 5; i++)
        {
            var n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    public bool CheckUpTile(Node node, TileState state)
    {
        for (var i = 0; i < 5; i++)
        {
            var n = GetTileAt(new Vector2(node.Position.x, node.Position.y + i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    private bool CheckDiagonalTile(Node node, TileState state)
    {
        for (var i = 0; i < 5; i++)
        {
            var n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y + i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    private bool CheckDiagonalDownTile(Node node, TileState state)
    {
        for (var i = 0; i < 5; i++)
        {
            var n = GetTileAt(new Vector2(node.Position.x + i, node.Position.y - i));

            if (n == null || n.State != state)
                return false;
        }

        return true;
    }

    public void CheckWin()
    {
        foreach (var node in GameMap)
            switch (node.State)
            {
                case TileState.NEUTRAL:
                    continue;
                case TileState.PLAYER1:
                    if (CheckRightTile(node, TileState.PLAYER1) || CheckUpTile(node, TileState.PLAYER1) ||
                        CheckDiagonalTile(node, TileState.PLAYER1) || CheckDiagonalDownTile(node, TileState.PLAYER1))
                    {
                        WinScreen.SetActive(true);
                        WinScreen.GetComponentInChildren<TMP_Text>().text = "Blue Win";
                        WinScreen.GetComponentInChildren<TMP_Text>().color = Color.blue;
                        gameOver = true;
                    }

                    break;
                case TileState.PLAYER2:
                    if (CheckRightTile(node, TileState.PLAYER2) || CheckUpTile(node, TileState.PLAYER2) ||
                        CheckDiagonalTile(node, TileState.PLAYER2) || CheckDiagonalDownTile(node, TileState.PLAYER2))
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

        if (gameOver) return;
        if (currentRoundsPlayed < totalRoundsAllowed) return;
        WinScreen.SetActive(true);
        WinScreen.GetComponentInChildren<TMP_Text>().text = "Game tied";
        WinScreen.GetComponentInChildren<TMP_Text>().color = Color.black;
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        return GameObject.Find($"{(int)position.x},{(int)position.y}").GetComponent<Tile>();
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

[Serializable]
public class Node
{
    public Node(Vector2 pos, TileState st = TileState.NEUTRAL)
    {
        Position = pos;
        State = st;
    }

    public Vector2 Position;
    public TileState State;

    public bool IsNode(Vector2 comparedPosition)
    {
        return Position == comparedPosition;
    }
}
