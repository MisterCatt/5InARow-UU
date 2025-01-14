using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : MonoBehaviour
{
    public static AIController Instance;

    public Node[,] GameMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public abstract void MakeMove();
    protected abstract void CalculateMove();

    protected virtual void UpdateGameBoard()
    {
        GameMap = GameController.Instance.GameMap;
    }
}

public class AiNode
{
    public AiNode(Node node)
    {
        this.node = node;
    }

    public Node node;

    private int point = 0;

    public void SetValue(int value) => point = value;
    public void RemoveValue() => point--;
    public int GetNodeValue() => point;
}
