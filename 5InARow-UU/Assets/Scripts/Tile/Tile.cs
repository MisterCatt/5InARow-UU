using System;
using System.Linq;
using UnityEngine;

public enum TileState { NEUTRAL, PLAYER1, PLAYER2}

public class Tile : MonoBehaviour
{
    public TileState State = TileState.NEUTRAL;

    public Vector2 TilePosition;

    public void PlaceTile(TileState player)
    {
        State = player;

        GetComponent<SpriteRenderer>().color = player switch
        {
            TileState.PLAYER1 => Color.blue,
            TileState.PLAYER2 => Color.red,
            TileState.NEUTRAL => Color.white,
            _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
        };

        foreach (var tile in GameController.Instance.GameMap.Where(tile => tile.IsNode(TilePosition)))
        {
            tile.State = player;
        }

        GameController.Instance.SwapTurn();
    }

    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = State switch
        {
            TileState.PLAYER1 => new Color(0, 0, 0.8f),
            TileState.PLAYER2 => new Color(0.8f, 0, 0),
            TileState.NEUTRAL => new Color(0.8f, 0.8f, 0.8f),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = State switch
        {
            TileState.PLAYER1 => Color.blue,
            TileState.PLAYER2 => Color.red,
            TileState.NEUTRAL => Color.white,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    void OnMouseDown()
    {
        if (State != TileState.NEUTRAL)
            return;

        PlaceTile(GameController.Instance.IsPlayerTurn ? TileState.PLAYER1 : TileState.PLAYER2);
    }
}
