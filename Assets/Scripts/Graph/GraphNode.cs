using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraphNode
{
    public Vector3 normal { get; private set; }
    public Vector3 realWorldPosition { get; private set; }
    Graph graph;

    public Vector3Int matrixPosition
    {
        get { return CollisionMatrix.instance.ToMatrixPosition(realWorldPosition); }
    }

    public Dictionary<Direction, GraphVertex> vertices
    {
        get; private set;
    } = new Dictionary<Direction, GraphVertex>();

    public GraphNode(Vector3 normal, Vector3 realWorldPosition, Graph graph)
    {
        this.normal = normal;
        this.realWorldPosition = realWorldPosition;
        this.graph = graph;

        graph.Register(this);
    }

    public void AddVertex(GraphVertex vertex)
    {
        Direction direction = vertex.direction;
        if (!vertices.ContainsKey(direction))
        {
            vertices[direction] = vertex;
        }
        else
        {
            throw new System.Exception(
                "Node " + this.ToString() + " has already a vertex for direction " + direction.ToString()
            );
        }
    }

    public GraphVertex GetVertex(Direction direction)
    {
        if (vertices.ContainsKey(direction))
        {
            return vertices[direction];
        }
        return null;
    }

    public GraphNode GetNeighbor(Direction direction)
    {
        GraphVertex vertex = GetVertex(direction);
        if (vertex != null)
        {
            return vertex.tailNode;
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
