using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGraph : MonoBehaviour
{

    public static CollisionGraph instance;

    Graph _graph = new Graph();

    CollisionMatrix collisionMatrix
    {
        get { return CollisionMatrix.instance; }
    }

    List<GraphCollider> colliders = new List<GraphCollider>();


    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a CollisionMatrix.
            Destroy(gameObject);

    }

    private void Start()
    {
        BuildGraph();
    }

    private void BuildGraph()
    {
        foreach (GraphCollider graphCollider in colliders)
        {
            graphCollider.BuildNodes();
        }
        foreach (GraphCollider graphCollider in colliders)
        {
            graphCollider.BuildVertices();
        }
    }

    public void Register(GraphCollider collider)
    {
        colliders.Add(collider);
    }

    public GraphNode AddNode(Vector3 normal, Vector3 position)
    {
        return new GraphNode(normal, position, _graph);
    }

    public GraphVertex AddVertex(GraphNode headNode, GraphNode tailNode, Direction direction)
    {
        return new GraphVertex(headNode, tailNode, direction, _graph);
    }
}
