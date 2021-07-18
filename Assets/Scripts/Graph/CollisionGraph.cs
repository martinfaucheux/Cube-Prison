using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGraph : MonoBehaviour
{
    Graph graph = new Graph();

    CollisionMatrix collisionMatrix
    {
        get { return CollisionMatrix.instance; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
