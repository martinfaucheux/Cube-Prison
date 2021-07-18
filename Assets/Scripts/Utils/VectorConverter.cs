using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorConverter
{
    public static Vector3Int FloatToInt(Vector3 vect)
    {
        return Vector3Int.FloorToInt(vect + 0.5f * Vector3.one);
    }
}
