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
            Transform transform = collider.transform;

            Handles.color = Color.black;

            foreach (KeyValuePair<Vector3Int, GraphNode> item in collider.nodes)
            {
                Vector3Int normal = item.Key;
                Handles.ArrowHandleCap(
                    0,
                    transform.position + arrowDistance * (Vector3)normal,
                    // transform.rotation * Quaternion.LookRotation(normal),
                    Quaternion.LookRotation(normal),
                    size,
                    EventType.Repaint
                );
            }
        }
    }
}