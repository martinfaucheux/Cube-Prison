using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GraphCollider), true)]
public class GraphColliderEditor : Editor
{
    float size = 1f;
    float arrowDistance = 1f;

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {

            GraphCollider collider = (GraphCollider)target;
            DrawNormal(collider);

        }
    }

    private void DrawNormal(GraphCollider collider)
    {
        Transform transform = collider.transform;

        Color orangeColor = new Color(255 / 255f, 153 / 255f, 51 / 255f);

        foreach (KeyValuePair<Vector3Int, GraphNode> nodeItem in collider.nodes)
        {
            Vector3Int normal = nodeItem.Key;
            GraphNode node = nodeItem.Value;

            Vector3 arrowPosition = transform.position + arrowDistance * (Vector3)normal;

            Handles.color = Color.black;
            Handles.ArrowHandleCap(
                0,
                arrowPosition,
                // transform.rotation * Quaternion.LookRotation(normal),
                Quaternion.LookRotation(normal),
                size,
                EventType.Repaint
            );

            foreach (KeyValuePair<Direction, GraphVertex> vertexItem in node.vertices)
            {
                Direction direction = vertexItem.Key;
                GraphVertex vertex = vertexItem.Value;
                GraphNode adjacentNode = vertex.tailNode;

                Vector3 dirVect = (adjacentNode.realWorldPosition - node.realWorldPosition);

                Handles.color = (adjacentNode is VoidNode) ? orangeColor : Color.white;
                Handles.ArrowHandleCap(
                    0,
                    arrowPosition,
                    // transform.rotation * Quaternion.LookRotation(normal),
                    Quaternion.LookRotation(dirVect),
                    size,
                    EventType.Repaint
                );
            }
        }
    }
}