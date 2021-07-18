using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3Int FloatToInt(Vector3 vect)
    {
        return Vector3Int.FloorToInt(vect + 0.5f * Vector3.one);
    }
    public static Vector3Int[] normals
    {
        get
        {
            return new Vector3Int[]{
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
                Vector3Int.forward,
                Vector3Int.back,
            };
        }
    }
}
