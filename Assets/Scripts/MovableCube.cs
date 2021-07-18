using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCube : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.3f;
    [SerializeField] float fallDuration = 0.1f;
    
    [SerializeField] Transform cubeMeshTransform;
    [SerializeField] Transform bumpTransform;
    private MatrixCollider matrixCollider;
    private bool _isFreeFall = false;
    private Vector3Int _iniMatrixPos;

    public bool isMoving{get; private set;} = false;

    private void Start() {
        matrixCollider = GetComponent<MatrixCollider>();
        _iniMatrixPos = matrixCollider.position;
    }

    public void Move(Direction direction){
        // Debug.Log(matrixCollider.IsValidDirection(direction));
        if(!isMoving && matrixCollider.IsValidDirection(direction)){
            Vector3Int initMatrixPosition = matrixCollider.position;
            Vector3Int newMatrixPosition = matrixCollider.Move(direction);

            int fallUnits;
            if(newMatrixPosition == Vector3Int.zero){
                fallUnits = initMatrixPosition.y + 10;
                _isFreeFall = true;
            }
            else{
                fallUnits= (initMatrixPosition - newMatrixPosition).y;
            }

            StartCoroutine(MoveCoroutine(direction, fallUnits));
        }
    }

    private IEnumerator MoveCoroutine(Direction direction, int fallUnits){
        isMoving=true;
        Vector3 initPosition = transform.position;
        Quaternion initRotation = transform.rotation;

        Vector2Int displacement2d = direction.ToPos();
        Vector3 displacement = new Vector3(displacement2d.x, 0f, displacement2d.y);
        Vector3 targetPosition = initPosition + displacement;

        Quaternion appliedRotation = Quaternion.FromToRotation(Vector3.up, displacement);
        Quaternion targetRotation = initRotation * appliedRotation;

        float diagonal = 1.41421356237f;

        float timeSinceStart = 0f;
            if (direction != Direction.IDLE){
            while(timeSinceStart < moveDuration){
                float t = timeSinceStart / moveDuration;

                float bump = (diagonal - 1) / 2 * Mathf.Sin(Mathf.PI * t);

                transform.position = Vector3.Lerp(initPosition, targetPosition, t);
                bumpTransform.localPosition = bump * Vector3.up;
                cubeMeshTransform.rotation = Quaternion.Lerp(initRotation, targetRotation, t);
                
                timeSinceStart += Time.deltaTime;
                yield return null;
            }
            bumpTransform.localPosition = Vector3.zero;
            transform.position = targetPosition;
            cubeMeshTransform.rotation = targetRotation;
        }

        Vector3 fallenPosition = targetPosition - fallUnits * Vector3Int.up;
        timeSinceStart = 0f;
        float totalFallDuration = fallUnits * fallDuration;

        while(timeSinceStart < totalFallDuration){
            transform.position = Vector3.Lerp(targetPosition, fallenPosition, timeSinceStart / totalFallDuration);
            timeSinceStart += Time.deltaTime;
            yield return null;
        }
        transform.position = fallenPosition;
        isMoving=false;

        if(_isFreeFall){
            ReInitialize();
        }
    }

    private void ReInitialize(){
        _isFreeFall = false;
        transform.position = matrixCollider.realWorldPosition;
        Move(Direction.IDLE); // check for fall
    }

}
