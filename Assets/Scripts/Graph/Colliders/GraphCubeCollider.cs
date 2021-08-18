using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCubeCollider : GraphCollider
{


    public override void BuildNodes()
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

    public override void BuildVertices()
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
}
