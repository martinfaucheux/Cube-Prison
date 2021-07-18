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

    public void BuildNodes()
    {
        foreach (Vector3Int normal in normals)
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
                if (direction == Direction.IDLE)
                {
                    continue;
                }

                Vector3Int dirVect = direction.To3dPos(normal);
                Vector3Int positionToCheck = matrixCollider.position + dirVect;

                MatrixCollider adjacentMatrixCollider = collisionMatrix.Get(positionToCheck);
                if (adjacentMatrixCollider != null)
                {
                    GraphCollider adjacentGraphCollider = adjacentMatrixCollider.GetComponent<GraphCollider>();

                    // for now, only check for nodes with the same normal
                    GraphNode adjacentNode = adjacentGraphCollider.GetNode(normal);
                    if (adjacentNode != null)
                    {
                        collisionGraph.AddVertex(node, adjacentNode, direction);
                    }
                }
            }
        }
    }

    public GraphNode GetNode(Vector3Int normal)
    {
        if (nodes.ContainsKey(normal))
        {
            return nodes[normal];
        }
        return null;
    }

    public static Vector3Int[] normals
    {
        get
        {
            return new Vector3Int[]{
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
                Vector3Int.forward,
                Vector3Int.back,

            };
        }
    }

}
