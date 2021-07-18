using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixCollider : MonoBehaviour
{
    public Vector3Int position;
    public Vector3Int initPosition {get; private set;}

    private CollisionMatrix collisionMatrix{
        get => CollisionMatrix.instance;
    }

    public Vector3 realWorldPosition{
        get{
            return collisionMatrix.ToRealPosition(position);
        }
    }

    void Start() {
        Register();
        SetThisPosition();
        initPosition = position;
    }

    public bool IsValidDirection(Direction direction){
        Vector3Int positionToCheck = position + direction.To3dPos();
        MatrixCollider collider = collisionMatrix.Get(positionToCheck);

        // don't collide with itself
        return collider == null || collider == this;

        // return (
        //     !collisionMatrix.IsOutOfBound(positionToCheck)
        //     && (
        //         collider == null
        //         || collider == this // don't collide with itself
        //     )
        // );
    }

    private void Register(){
        collisionMatrix.Register(this);
    }


    public Vector3Int Move(Vector3Int targetPosition){
        Vector3Int newPosition = collisionMatrix.Move(this, targetPosition);
        SetThisPosition();
        return newPosition;
    }
    public Vector3Int Move(Direction direction) => Move(position + direction.To3dPos());

    private void SetThisPosition(){
        position = collisionMatrix.GetPosition(this);
    }    
}
