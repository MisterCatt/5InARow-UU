using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DanielSimpleAI : AIController
{
    private List<AiNode> aiNodes = new List<AiNode>();
    private List<Node> nodesToCheck = new List<Node>();

    private string _comparitor = "00000";

    private AiNode bestNode;

    public override void MakeMove()
    {
        UpdateGameBoard();

        CalculateMove();
        GameController.Instance.GetTileAtPosition(bestNode.node.Position).PlaceTile(TileState.PLAYER2);
    }
    protected override void CalculateMove()
    {
        foreach (Node node in nodesToCheck)
        {
            LookRight(node);
            LookUp(node);
            LookDiagonalUp(node);
            LookDiagonalDown(node);
        }

        bestNode = aiNodes[0];
        foreach (var aiNode in aiNodes.Where(aiNode => bestNode.GetNodeValue() < aiNode.GetNodeValue()))
        {
            bestNode = aiNode;
        }
    }

    Vector2Int CalculatePoints(string comparitor)
    {
        switch (comparitor)
        {
            //Winner nodes
            case "01111":
                return new Vector2Int(0, 10000);
            case "10111":
                return new Vector2Int(1, 10000);
            case "11011":
                return new Vector2Int(2, 10000);
            case "11101":
                return new Vector2Int(3, 10000);
            case "11110":
                return new Vector2Int(4, 10000);
            //EnemyWinNodes
            case "02222":
                return new Vector2Int(0, 9999);
            case "20222":
                return new Vector2Int(1, 9999);
            case "22022":
                return new Vector2Int(2, 9999);
            case "22202":
                return new Vector2Int(3, 9999);
            case "22220":
                return new Vector2Int(4, 9999);
            //High point nodes
            case "01110":
                return new Vector2Int(0, 200);
            case "11100":
                return new Vector2Int(3, 200);
            case "11010":
                return new Vector2Int(2, 200);
            case "10110":
                return new Vector2Int(1, 200);
            case "00111":
                return new Vector2Int(1, 200);
            //Enemy High point nodes
            case "02220":
                return new Vector2Int(0, 199);
            case "22200":
                return new Vector2Int(3, 199);
            case "22020":
                return new Vector2Int(2, 199);
            case "20220":
                return new Vector2Int(1, 199);
            case "00222":
                return new Vector2Int(1, 199);

            default:
                return new Vector2Int(1, 0);
        }
    }

    private void LookRight(Node node, int iterations = 5)
    {
        StringBuilder sb = new StringBuilder(_comparitor);
        AiNode aiNode = new AiNode(node);

        for (var x = 0; x < iterations; x++)
        {
            if (node.Position.x + x >= 15)
                break;

            switch (GameController.Instance.GetTileAt(new Vector2(node.Position.x + x, node.Position.y)).State)
            {
                case TileState.NEUTRAL:
                    sb[x] = '0';
                    break;
                case TileState.PLAYER1:
                    sb[x] = '2';
                    if(node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
                case TileState.PLAYER2:
                    sb[x] = '1';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue()+2);
                    break;
            }
        }

        Vector2Int nodePoints = CalculatePoints(sb.ToString());

        if (nodePoints.y < 1)
        {
            aiNodes.Add(aiNode);
        }
        else
        {
            aiNode = new AiNode(new Node(new Vector2(node.Position.x + nodePoints.x, node.Position.y)));
            aiNode.SetValue(nodePoints.y);
            aiNodes.Add(aiNode);
        }
        
        _comparitor = "00000";
    }

    private void LookUp(Node node, int iterations = 5)
    {
        StringBuilder sb = new StringBuilder(_comparitor);
        AiNode aiNode = new AiNode(node);

        for (var y = 0; y < iterations; y++)
        {
            if (node.Position.y + y >= 15)
                break;

            switch (GameController.Instance.GetTileAt(new Vector2(node.Position.x, node.Position.y+y)).State)
            {
                case TileState.NEUTRAL:
                    sb[y] = '0';
                    break;
                case TileState.PLAYER1:
                    sb[y] = '2';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
                case TileState.PLAYER2:
                    sb[y] = '1';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
            }
        }

        Vector2Int nodePoints = CalculatePoints(sb.ToString());

        if (nodePoints.y < 1)
        {
            aiNodes.Add(aiNode);
        }
        else
        {
            aiNode = new AiNode(new Node(new Vector2(node.Position.x, node.Position.y + nodePoints.x)));
            aiNode.SetValue(nodePoints.y);
            aiNodes.Add(aiNode);
        }

        _comparitor = "00000";
    }

    private void LookDiagonalUp(Node node, int iterations = 5)
    {
        StringBuilder sb = new StringBuilder(_comparitor);
        AiNode aiNode = new AiNode(node);

        for (var y = 0; y < iterations; y++)
        {
            if (node.Position.y + y >= 15)
                break;

            if (node.Position.x + y >= 15)
                break;

            switch (GameController.Instance.GetTileAt(new Vector2(node.Position.x + y, node.Position.y + y)).State)
            {
                case TileState.NEUTRAL:
                    sb[y] = '0';
                    break;
                case TileState.PLAYER1:
                    sb[y] = '2';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
                case TileState.PLAYER2:
                    sb[y] = '1';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
            }
        }

        Vector2Int nodePoints = CalculatePoints(sb.ToString());

        if (nodePoints.y < 1)
        {
            aiNodes.Add(aiNode);
        }
        else
        {
            aiNode = new AiNode(new Node(new Vector2(node.Position.x + nodePoints.x, node.Position.y + nodePoints.x)));
            aiNode.SetValue(nodePoints.y);
            aiNodes.Add(aiNode);
        }

        _comparitor = "00000";
    }

    private void LookDiagonalDown(Node node, int iterations = 5)
    {
        StringBuilder sb = new StringBuilder(_comparitor);
        AiNode aiNode = new AiNode(node);

        for (var y = 0; y < iterations; y++)
        {
            if (node.Position.y - y < 0)
                break;

            if (node.Position.x + y >= 15)
                break;

            switch (GameController.Instance.GetTileAt(new Vector2(node.Position.x + y, node.Position.y - y)).State)
            {
                case TileState.NEUTRAL:
                    sb[y] = '0';
                    break;
                case TileState.PLAYER1:
                    sb[y] = '2';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
                case TileState.PLAYER2:
                    sb[y] = '1';
                    if (node.State == TileState.NEUTRAL)
                        aiNode.SetValue(aiNode.GetNodeValue() + 2);
                    break;
            }
        }

        Vector2Int nodePoints = CalculatePoints(sb.ToString());

        if (nodePoints.y < 1)
        {
            aiNodes.Add(aiNode);
        }
        else
        {
            aiNode = new AiNode(new Node(new Vector2(node.Position.x + nodePoints.x, node.Position.y - nodePoints.x)));
            aiNode.SetValue(nodePoints.y);
            aiNodes.Add(aiNode);
        }

        _comparitor = "00000";
    }

    protected override void UpdateGameBoard()
    {
        aiNodes.Clear();
        nodesToCheck.Clear();
        bestNode = null;

        foreach(var node in GameController.Instance.placedNodes)
        {
            for(var x = -1; x <= 1; x++)
                for(var y = -1; y <= 1; y++)
                {
                    nodesToCheck.Add(GameController.Instance.GetTileAt(new Vector2(node.Position.x + x, node.Position.y + y)));
                }
        }
    }
}
