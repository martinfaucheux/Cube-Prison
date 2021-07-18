using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphVertex
{
    public GraphNode headNode { get; private set; }
    public GraphNode tailNode { get; private set; }
    public Direction direction { get; private set; }
    Graph graph;

    public GraphVertex(GraphNode headNode, GraphNode tailNode, Direction direction, Graph graph)
    {
        this.headNode = headNode;
        this.tailNode = tailNode;
        this.direction = direction;
        this.graph = graph;

        graph.Register(this);
        headNode.AddNeighbor(tailNode, direction);
    }
}
