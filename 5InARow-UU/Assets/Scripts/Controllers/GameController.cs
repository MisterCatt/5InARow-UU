using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : StateMachine
{
    public static GameController Instance;

    public bool IsPlayerTurn = false;

    public GameObject Tile;

    public List<Node> GameMap;

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

        for (int x = 0; x < 15; x++)
        {
            for (int y = 0; y < 15; y++)
            {
                var tile = Instantiate(Tile, new Vector3(x, y), Quaternion.identity);

                tile.GetComponent<Tile>().TilePosition = new Vector2(x, y);
                tile.name = $"Tile: {x},{y}";
                tile.GetComponent<Tile>().State = TileState.NEUTRAL;
                GameMap.Add(new Node(new Vector2(x, y), TileState.NEUTRAL));
            }
        }
    }

    private void Start()
    {
        SwitchState<ST_PlayerTurn>();
    }

    public Node GetTileAt(Vector2 position) => GameMap.FirstOrDefault(node => node.IsNode(position));

    public void SwapTurn()
    {
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

    public void CheckWin()
    {
        foreach (Node node in GameMap)
        {
            switch (node.State)
            {
                case TileState.NEUTRAL:
                    continue;
                case TileState.PLAYER1:
                    if (CheckRightTile(node, TileState.PLAYER1))
                    {
                        Debug.Log("Blue win");
                    }
                    if (CheckUpTile(node, TileState.PLAYER1))
                    {
                        Debug.Log("Blue win");
                    }
                    if (CheckDiagonalTile(node, TileState.PLAYER1))
                    {
                        Debug.Log("Blue win");
                    }
                    break;
                case TileState.PLAYER2:
                    if (CheckRightTile(node, TileState.PLAYER2))
                    {
                        Debug.Log("Red win");
                    }
                    if (CheckUpTile(node, TileState.PLAYER2))
                    {
                        Debug.Log("Red win");
                    }
                    if (CheckDiagonalTile(node, TileState.PLAYER2))
                    {
                        Debug.Log("Red win");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
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
