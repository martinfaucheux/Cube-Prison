using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMatrix : MonoBehaviour
{

    public static CollisionMatrix instance;

    [SerializeField] Vector3Int size;
    [SerializeField] Vector3 realWorldOffset;
    private Generic3dGrid<MatrixCollider> _grid;

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

    private void Start() {
        _grid = new Generic3dGrid<MatrixCollider>(size);
    }

    public MatrixCollider Get(Vector3Int position) => _grid.Get(position);
    public MatrixCollider Get(int x, int y, int z) => _grid.Get(x, y, z);

    public Vector3Int GetPosition(MatrixCollider collider) => _grid.GetPosition(collider);

    public Vector3Int Register(MatrixCollider collider){
        Vector3Int matrixPosition = ToMatrixPosition(collider.transform.position);

        if(Get(matrixPosition) == null){
            _grid[matrixPosition] = collider;
        }
        else {
            throw new System.Exception("There is already a collider at position " + matrixPosition.ToString());
        }
        return matrixPosition;
    }

    public void Remove(MatrixCollider colldier){
        _grid.Remove(colldier);
    }

    public Vector3Int ToMatrixPosition(Vector3 realWorldPosition){
        Vector3 normalizedPosition = realWorldPosition - realWorldOffset;
        return Vector3Int.FloorToInt(normalizedPosition + 0.5f * Vector3.one);
    }

    public Vector3 ToRealPosition(Vector3Int matrixPosition){
        return matrixPosition + realWorldOffset;
    }

    public Vector3Int Move(MatrixCollider collider, Vector3Int position){
        MatrixCollider floorCollider = GetFloor(position, collider);

        if(floorCollider != null){
            Vector3Int newPosition = floorCollider.position + Vector3Int.up;
            _grid[newPosition] = collider;
            return newPosition;
        }

        // this means the cube has fallen
        _grid[collider.initPosition] = collider;
        return Vector3Int.zero;
    }

    private MatrixCollider GetFloor(Vector3Int startPosition, MatrixCollider ignoreCollider = null){

        for(int y = startPosition.y; y >= 0; y--){
            Vector3Int posToCheck = new Vector3Int(startPosition.x, y, startPosition.z);

            MatrixCollider collider = Get(posToCheck);
            if(collider != null && collider != ignoreCollider){
                return collider;
            }
        }
        return null;
    }

    public bool IsOutOfBound(Vector3Int position){
        return (
            position.x < 0 
            || position.x >= size.x
            || position.y < 0 
            || position.y >= size.y
            || position.z < 0 
            || position.z >= size.z
        );
    }



}
