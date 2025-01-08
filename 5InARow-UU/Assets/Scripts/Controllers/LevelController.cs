using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

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
}


