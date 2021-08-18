using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCollider : MonoBehaviour
{
    MatrixCollider matrixCollider;

    public Dictionary<Vector3Int, GraphNode> nodes
    {
        get; private set;
    } = new Dictionary<Vector3Int, GraphNode>();

    CollisionMatrix collisionMatrix
    {
        get { return CollisionMatrix.instance; }
    }

    CollisionGraph collisionGraph
    {
        get { return CollisionGraph.instance; }
    }

    private void Awake()
    {
        matrixCollider = GetComponent<MatrixCollider>();
        collisionGraph.Register(this);
    }

    # region generation

    // used for generating the graph

    public void BuildNodes()
    {
        foreach (Vector3Int normal in VectorUtils.normals)
        {
            Vector3Int positionToCheck = matrixCollider.position + normal;
            MatrixCollider adjacentCollider = collisionMatrix.Get(positionToCheck);
            if (adjacentCollider == null)
            {
                nodes[normal] = collisionGraph.AddNode(normal, positionToCheck);
            }
        }
    }

    public void BuildVertices()
    {
        foreach (KeyValuePair<Vector3Int, GraphNode> item in nodes)
        {
            Vector3Int normal = item.Key;
            GraphNode node = item.Value;

            foreach (Direction direction in Direction.GetAll<Direction>())
            {
                if (direction != Direction.IDLE)
                {
                    GraphNode adjacentNode = DiscoverAdjacentNode(node, direction);
                    if (adjacentNode != null)
                    {
                        collisionGraph.AddVertex(node, adjacentNode, direction);
                    }
                }
            }
        }
    }

    private GraphNode DiscoverAdjacentNode(GraphNode node, Direction direction)
    {
        Vector3Int normal = VectorUtils.FloatToInt(node.normal);

        Vector3Int dirVect = direction.To3dPos(normal);
        Vector3Int positionToCheck = matrixCollider.position + dirVect;

        // check that the node is not obstructed
        Vector3Int obstructedPosition = positionToCheck + normal;
        MatrixCollider adjacentCollider = collisionMatrix.Get(obstructedPosition);
        if (adjacentCollider != null)
        {
            // in case the position is obstructed, return null
            return null;
        }

        return collisionGraph.GetNearestNode(positionToCheck, normal);
    }

    public GraphNode GetNode(Vector3Int normal)
    {
        if (nodes.ContainsKey(normal))
        {
            return nodes[normal];
        }
        return null;
    }

    # endregion
}
