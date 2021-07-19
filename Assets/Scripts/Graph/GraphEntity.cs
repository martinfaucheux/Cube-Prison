using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEntity : MonoBehaviour
{
    public GraphNode currentNode { get; private set; }

    MatrixCollider matrixCollider;

    void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
        SetStartNode();
    }

    public bool IsValidDirection(Direction direction)
    {
        GraphNode neighborNode = currentNode.GetNeighbor(direction);
        // TODO: add more conditions if some entities can block the node
        return (neighborNode != null);
    }

    public GraphVertex Move(Direction direction)
    {
        GraphVertex vertex = currentNode.GetVertex(direction);
        currentNode = vertex.tailNode;

        // mostly for debug
        matrixCollider.Move(currentNode.matrixPosition);
        return vertex;
    }

    private void SetStartNode()
    {
        // TODO: this could actually be baked before runtime
        Vector3Int matrixPosition = CollisionMatrix.instance.ToMatrixPosition(transform.position);
        currentNode = CollisionGraph.instance.GetNearestNode(matrixPosition, Vector3Int.up);
    }
}
