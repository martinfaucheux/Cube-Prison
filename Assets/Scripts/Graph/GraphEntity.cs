using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEntity : MonoBehaviour
{
    public EventNode currentNode { get; private set; }

    MatrixCollider matrixCollider;
    CubeMoveAnimator moveAnimator;

    public bool isMoving
    {
        get
        {
            if (moveAnimator != null)
            {
                return moveAnimator.isMoving;
            }
            return false;
        }
    }

    void Start()
    {
        moveAnimator = GetComponent<CubeMoveAnimator>();
        matrixCollider = GetComponent<MatrixCollider>();
        SetStartNode();
        currentNode.OnEnter(this);
    }

    public bool IsValidDirection(Direction direction)
    {
        GraphNode neighborNode = currentNode.GetNeighbor(direction);
        // TODO: add more conditions if some entities can block the node
        return (neighborNode != null);
    }

    public void Move(Direction direction)
    {
        GraphVertex graphVertex = currentNode.GetVertex(direction);
        currentNode = (EventNode)graphVertex.tailNode;

        // mostly for debug
        matrixCollider.Move(currentNode.matrixPosition);

        // animate movement in real world
        moveAnimator?.AnimateMove(graphVertex);
    }

    private void SetStartNode()
    {
        // TODO: should be different if entity is not the player
        currentNode = CollisionGraph.instance.startNode;
    }
}
