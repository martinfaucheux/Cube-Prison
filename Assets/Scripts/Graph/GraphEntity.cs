using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEntity : MonoBehaviour
{
    GraphNode currentNode;

    void Start()
    {
        SetStartNode();
    }

    public bool IsValidDirection(Direction direction)
    {
        GraphNode neighborNode = currentNode.GetNeighbor(direction);
        // TODO: add more conditions if some entities can block the node
        return (neighborNode != null);
    }

    public void Move(Direction direction)
    {
        currentNode = currentNode.GetNeighbor(direction);
    }

    private void SetStartNode()
    {
        Vector3Int matrixPosition = CollisionMatrix.instance.ToMatrixPosition(transform.position);
        currentNode = CollisionGraph.instance.GetNearestNode(matrixPosition, Vector3Int.up);
    }
}
