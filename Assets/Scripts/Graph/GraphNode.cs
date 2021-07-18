using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphNode
{
    public Vector3 normal { get; private set; }
    public Vector3 realWorldPosition { get; private set; }
    Graph graph;

    public Dictionary<Direction, GraphNode> neighbors
    {
        get; private set;
    }

    public GraphNode(Vector3 normal, Vector3 realWorldPosition, Graph graph)
    {
        this.normal = normal;
        this.realWorldPosition = realWorldPosition;
        this.graph = graph;
        this.neighbors = new Dictionary<Direction, GraphNode>();

        graph.Register(this);
    }

    public void AddNeighbor(GraphNode neighbor, Direction direction)
    {
        if (!neighbors.ContainsKey(direction))
        {
            neighbors[direction] = neighbor;
        }
        else
        {
            throw new System.Exception(
                "Node " + this.ToString() + " has already a neighbor for direction " + direction.ToString()
            );
        }
    }

    public GraphNode GetNeighbor(Direction direction)
    {
        if (neighbors.ContainsKey(direction))
        {
            return neighbors[direction];
        }
        return null;
    }

    public override string ToString()
    {
        string directionWord = "";
        if (normal.x > 0)
        {
            directionWord = "RIGHT";
        }
        else if (normal.x < 0)
        {
            directionWord = "LEFT";
        }
        else if (normal.y > 0)
        {
            directionWord = "UP";
        }
        else if (normal.y < 0)
        {
            directionWord = "DOWN";
        }
        else if (normal.z > 0)
        {
            directionWord = "FORWARD";
        }
        else if (normal.z < 0)
        {
            directionWord = "BACK";
        }
        return directionWord + " (" + realWorldPosition.x + ", " + realWorldPosition.y + ", " + realWorldPosition.z + ")";
    }
}
