using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSlopeCollider : GraphCollider
{
    private static Vector3Int slopeVect { get { return new Vector3Int(0, 1, -1); } }
    private static Vector3Int[] normals
    {
        get
        {
            return new Vector3Int[]{
                // normal of the slope
                // this replace up and back
                slopeVect,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
                Vector3Int.forward,
            };
        }
    }


    public override void BuildNodes()
    {
        foreach (Vector3Int normal in normals)
        {
            if (normal == slopeVect)
            {
                // need to check back + up positions for the slope face
                bool isObstructed = false;
                Vector3Int[] positionsToCheck = new Vector3Int[] { Vector3Int.back, Vector3Int.up };
                foreach (Vector3Int positionToCheck in positionsToCheck)
                {
                    MatrixCollider adjacentCollider = collisionMatrix.Get(positionToCheck);
                    if (adjacentCollider != null)
                    {
                        isObstructed = true;
                    }

                    if (!isObstructed)
                    {
                        nodes[slopeVect] = collisionGraph.AddNode(normal, positionToCheck);
                    }
                }
            }
            else
            {
                Vector3Int positionToCheck = matrixCollider.position + normal;
                MatrixCollider adjacentCollider = collisionMatrix.Get(positionToCheck);
                if (adjacentCollider == null)
                {
                    nodes[normal] = collisionGraph.AddNode(normal, positionToCheck);
                }
            }
        }
    }

    public override void BuildVertices()
    {
        // foreach (KeyValuePair<Vector3Int, GraphNode> item in nodes)
        // {
        //     Vector3Int normal = item.Key;
        //     GraphNode node = item.Value;

        //     foreach (Direction direction in Direction.GetAll<Direction>())
        //     {
        //         if (direction != Direction.IDLE)
        //         {
        //             GraphNode adjacentNode = DiscoverAdjacentNode(node, direction);
        //             if (adjacentNode != null)
        //             {
        //                 collisionGraph.AddVertex(node, adjacentNode, direction);
        //             }
        //         }
        //     }
        // }
    }
}
