using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGraph : MonoBehaviour
{

    public static CollisionGraph instance;

    Graph _graph = new Graph();
    List<GraphCollider> colliders = new List<GraphCollider>();

    public Dictionary<Vector3Int, GraphNode> voidNodes
    {
        get; private set;
    } = new Dictionary<Vector3Int, GraphNode>();

    public StartNode startNode { get; private set; }

    CollisionMatrix collisionMatrix
    {
        get { return CollisionMatrix.instance; }
    }

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
        BuildVoidNodes();
        foreach (GraphCollider graphCollider in colliders)
        {
            graphCollider.BuildNodes();
        }
        foreach (GraphCollider graphCollider in colliders)
        {
            graphCollider.BuildVertices();
        }
        BuildStartNode();
    }

    public void Register(GraphCollider collider)
    {
        colliders.Add(collider);
    }

    public GraphNode AddNode(Vector3 normal, Vector3Int matrixPosition)
    {
        Vector3 realWorldPosition = collisionMatrix.ToRealPosition(matrixPosition);
        return new BasicNode(normal, realWorldPosition, _graph);
    }

    public GraphVertex AddVertex(GraphNode headNode, GraphNode tailNode, Direction direction)
    {
        return new GraphVertex(headNode, tailNode, direction, _graph);
    }

    public GraphNode GetNearestNode(Vector3Int startMatrixPosition, Vector3Int normal)
    {
        Vector3Int nextPositionToCheck = startMatrixPosition;

        while (!collisionMatrix.IsOutOfBound(nextPositionToCheck))
        {
            MatrixCollider matrixCollider = collisionMatrix.Get(nextPositionToCheck);
            if (matrixCollider != null)
            {
                GraphCollider graphCollider = matrixCollider.GetComponent<GraphCollider>();
                if (graphCollider != null)
                {
                    // for now, only check for nodes with the same normal
                    GraphNode adjacentNode = graphCollider.GetNode(normal);
                    if (adjacentNode != null)
                    {
                        return adjacentNode;
                    }
                }
            }
            // if no node is found, check next position (-1 normal)
            nextPositionToCheck -= normal;
        }
        // return corresponding void node if nothing is found
        return voidNodes[normal];
    }

    private void BuildStartNode()
    {
        // NOTE: this assumes the startNode is always different from any other node
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        MatrixCollider matrixCollider = playerGameObject.GetComponent<MatrixCollider>();
        startNode = new StartNode(Vector3.up, matrixCollider.realWorldPosition, _graph);

        // build vertex from start to first valid position
        GraphNode startIdleNode = GetNearestNode(matrixCollider.position, Vector3Int.up);
        new GraphVertex(startNode, startIdleNode, Direction.IDLE, _graph);

        foreach (KeyValuePair<Vector3Int, GraphNode> pair in voidNodes)
        {
            GraphNode voidNode = pair.Value;
            new GraphVertex(voidNode, startNode, Direction.IDLE, _graph);

        }
    }

    private void BuildVoidNodes()
    {
        foreach (Vector3Int normal in VectorUtils.normals)
        {
            Vector3 position = -1000 * normal;
            VoidNode newNode = new VoidNode(normal, position, _graph);
            voidNodes[normal] = newNode;
        }
    }
}
