using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidNode : EventNode
{
    public VoidNode(Vector3 normal, Vector3 realWorldPosition, Graph graph) : base(normal, realWorldPosition, graph) { }

    public override void OnEnter(GraphEntity graphEntity)
    {
        base.OnEnter(graphEntity);
        graphEntity.Move(Direction.IDLE);
    }

    public override string ToString()
    {
        return GetNormalString() + " (VOID)";
    }
}
