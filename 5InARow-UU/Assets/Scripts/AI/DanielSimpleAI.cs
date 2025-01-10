using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DanielSimpleAI : AIController
{
    public AiNode[,] AiNodeMap;
    private AiNode bestNode;

    public override void MakeMove()
    {
        CalculateMove();
        GameController.Instance.GetTileAtPosition(bestNode.node.Position).PlaceTile(TileState.PLAYER2);
    }
    protected override void CalculateMove()
    {
        UpdateGameBoard();

        foreach (Node node in GameMap)
        {
            if (node.State is not (TileState.NEUTRAL or TileState.PLAYER2)) continue;
            CheckHorizontal(node);
            CheckVertical(node);
            CheckDiagonal(node);
        }

        bestNode = AiNodeMap[0, 0];

        foreach (AiNode node in AiNodeMap)
        {
            if(node.GetNodeValue() > bestNode.GetNodeValue())
                bestNode = node;
        }

        
    }

    protected override void UpdateGameBoard()
    {
        GameMap = GameController.Instance.GameMap;
        AiNodeMap = new AiNode[15, 15];

        for(int x = 0; x < 15; x++)
        for (int y = 0; y < 15; y++)
        {
            AiNodeMap[x, y] = new AiNode(new Node(new Vector2()));
            AiNodeMap[x, y].node = GameMap[x, y];
        }
    }

    public bool CheckBoardForWin()
    {
        

        return false;
    }

    public void CheckHorizontal(Node node)
    {
        for (var i = 1; i < 5; i++)
        {
            if (node.Position.x + i >= 15)
                break;

            if (GameMap[(int)node.Position.x + i, (int)node.Position.y].State == TileState.PLAYER2)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].AddValue();
            }
            if (GameMap[(int)node.Position.x + i, (int)node.Position.y].State == TileState.PLAYER1)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].RemoveValue();
            }
        }
    }

    public void CheckVertical(Node node)
    {
        for (var i = 1; i < 5; i++)
        {
            if (node.Position.y + i >= 15)
                break;

            if ( GameController.Instance.GetTileAt(new Vector2(node.Position.x, node.Position.y+i)).State == TileState.PLAYER2)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].AddValue();
            }
            if (GameController.Instance.GetTileAt(new Vector2(node.Position.x, node.Position.y + i)).State == TileState.PLAYER1)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].RemoveValue();
            }
        }
    }

    public void CheckDiagonal(Node node)
    {
        for (var i = 1; i < 5; i++)
        {
            if(node.Position.x+i >= 15 || node.Position.y -i < 0)
                continue;

            if (GameMap[(int)node.Position.x+i, (int)node.Position.y - i].State == TileState.PLAYER2)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].AddValue();
            }
            if (GameMap[(int)node.Position.x + i, (int)node.Position.y-i].State == TileState.PLAYER1)
            {
                AiNodeMap[(int)node.Position.x, (int)node.Position.y].AddValue();
            }
        }
    }

}
