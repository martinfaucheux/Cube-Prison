using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphCollider : MonoBehaviour
{
    protected MatrixCollider matrixCollider;

    public Dictionary<Vector3Int, GraphNode> nodes
    {
        get; private set;
    } = new Dictionary<Vector3Int, GraphNode>();

    protected CollisionMatrix collisionMatrix
    {
        get { return CollisionMatrix.instance; }
    }

    protected CollisionGraph collisionGraph
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

    public abstract void BuildNodes();

    public abstract void BuildVertices();

    protected GraphNode DiscoverAdjacentNode(Vector3Int normal, Vector3Int firstPositionToCheck)
    {
        // check that the node is not obstructed
        Vector3Int obstructedPosition = firstPositionToCheck + normal;
        MatrixCollider adjacentCollider = collisionMatrix.Get(obstructedPosition);
        if (adjacentCollider != null)
        {
            // in case the position is obstructed, return null
            return null;
        }

        return collisionGraph.GetNearestNode(firstPositionToCheck, normal);
    }

    protected GraphNode DiscoverAdjacentNode(GraphNode node, Direction direction)
    {
        Vector3Int normal = VectorUtils.FloatToInt(node.normal);
        Vector3Int dirVect = direction.To3dPos(normal);
        Vector3Int firstPositionToCheck = matrixCollider.position + dirVect;
        return DiscoverAdjacentNode(normal, firstPositionToCheck);
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
