using System;
using System.Linq;
using UnityEngine;

public enum TileState { NEUTRAL, PLAYER1, PLAYER2}

public class Tile : MonoBehaviour
{
    public TileState State = TileState.NEUTRAL;

    public Vector2 TilePosition;

    public GameObject tilePiece;

    public void PlaceTile(TileState player)
    {
        if (State != TileState.NEUTRAL || GameController.Instance.gameOver)
            return;

        State = player;

        tilePiece.SetActive(true);
        tilePiece.GetComponent<SpriteRenderer>().color = player switch
        {
            TileState.PLAYER1 => Color.blue,
            TileState.PLAYER2 => Color.red,
            TileState.NEUTRAL => Color.white,
            _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
        };

        GameController.Instance.GameMap[(int)TilePosition.x, (int)TilePosition.y].State = player;

        GameController.Instance.placedNodes.Add(new Node(new Vector2((int)TilePosition.x, (int)TilePosition.y), player));

        GameController.Instance.SwapTurn();
    }

    void OnMouseEnter()
    {
        if (GameController.Instance.gameOver)
            return;

        GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);

        if (tilePiece.activeSelf)
            tilePiece.GetComponent<SpriteRenderer>().color = State switch
            {
                TileState.PLAYER1 => new Color(0, 0, 0.8f),
                TileState.PLAYER2 => new Color(0.8f, 0, 0),
                TileState.NEUTRAL => new Color(0.8f, 0.8f, 0.8f),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    void OnMouseExit()
    {
        if (GameController.Instance.gameOver)
            return;

        GetComponent<SpriteRenderer>().color = Color.white;

        if (tilePiece.activeSelf)
            tilePiece.GetComponent<SpriteRenderer>().color = State switch
            {
                TileState.PLAYER1 => Color.blue,
                TileState.PLAYER2 => Color.red,
                TileState.NEUTRAL => Color.white,
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    void OnMouseDown()
    {
        PlaceTile(GameController.Instance.IsPlayerTurn ? TileState.PLAYER1 : TileState.PLAYER2);
    }
}
