using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GraphCollider))]
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

            Handles.color = Color.white;
            foreach (KeyValuePair<Direction, GraphNode> neighborItem in node.neighbors)
            {
                Direction direction = neighborItem.Key;
                GraphNode adjacentNode = neighborItem.Value;

                Vector3 dirVect = (adjacentNode.realWorldPosition - node.realWorldPosition);

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