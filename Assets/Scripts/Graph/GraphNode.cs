using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GraphNode : ScriptableObject
{
    Vector3 normal;
    Vector3 realWorldPosition;
    Graph graph;

    Dictionary<Direction, GraphNode> neighbors;

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
            directionWord = "FORWARD";
        }
        else if (normal.x < 0)
        {
            directionWord = "BACK";
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
            directionWord = "LEFT";
        }
        else if (normal.z < 0)
        {
            directionWord = "RIGHT";
        }
        return directionWord + "(" + realWorldPosition.x + ", " + realWorldPosition.y + ", " + realWorldPosition.z + ")";
    }
}
