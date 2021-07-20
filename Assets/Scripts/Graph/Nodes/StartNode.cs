using UnityEngine;

public class StartNode : EventNode
{
    public StartNode(Vector3 normal, Vector3 realWorldPosition, Graph graph) : base(normal, realWorldPosition, graph) { }
    public override void OnEnter(GraphEntity graphEntity)
    {
        base.OnEnter(graphEntity);
        graphEntity.Move(Direction.IDLE);
    }
}
