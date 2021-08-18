using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph
{
    List<GraphNode> nodes = new List<GraphNode>();
    List<GraphVertex> vertices = new List<GraphVertex>();

    public void Register(GraphNode node)
    {
        nodes.Add(node);
    }

    public void Register(GraphVertex vertex)
    {
        vertices.Add(vertex);
    }

}
