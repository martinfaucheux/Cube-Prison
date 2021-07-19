using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNode : GraphNode
{
    public EventNode(Vector3 normal, Vector3 realWorldPosition, Graph graph) : base(normal, realWorldPosition, graph) { }

    public void OnEnter(GraphEntity graphEntity) { }

    public void OnExit(GraphEntity graphEntity) { }

}
