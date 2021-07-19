using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCube2 : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.3f;
    [SerializeField] float fallDuration = 0.1f;

    [SerializeField] Transform cubeMeshTransform;
    [SerializeField] Transform bumpTransform;
    private MatrixCollider matrixCollider;
    private GraphEntity graphEntity;
    private bool _isFreeFall = false;
    private Vector3Int _iniMatrixPos;

    public bool isMoving { get; private set; } = false;

    private void Start()
    {
        matrixCollider = GetComponent<MatrixCollider>();
        graphEntity = GetComponent<GraphEntity>();
        _iniMatrixPos = matrixCollider.position;
        Move(Direction.IDLE);
    }

    public void Move(Direction direction)
    {
        // Debug.Log(matrixCollider.IsValidDirection(direction));
        if (!isMoving && graphEntity.IsValidDirection(direction))
        {
            Vector3 initRealPosition = graphEntity.currentNode.realWorldPosition;
            GraphVertex graphVertex = graphEntity.Move(direction);
            Vector3 newPosition = graphEntity.currentNode.realWorldPosition;

            int fallUnits = graphVertex.IsFreeFall() ? 10 : graphVertex.GetFallUnits();

            Vector3Int rollDisplacement = direction.To3dPos(graphEntity.currentNode.normal);

            StartCoroutine(MoveCoroutine(rollDisplacement, fallUnits));
        }
    }

    private IEnumerator MoveCoroutine(Vector3Int rollDisplacement, int fallUnits)
    {
        isMoving = true;
        Vector3 initPosition = transform.position;
        Quaternion initRotation = transform.rotation;

        // Vector2Int displacement2d = direction.ToPos();
        // Vector3 rollDisplacement = new Vector3(displacement2d.x, 0f, displacement2d.y);
        Vector3 targetPosition = initPosition + rollDisplacement;

        Quaternion appliedRotation = Quaternion.FromToRotation(Vector3.up, rollDisplacement);
        Quaternion targetRotation = initRotation * appliedRotation;

        float diagonal = 1.41421356237f;

        float timeSinceStart = 0f;

        while (timeSinceStart < moveDuration)
        {
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


        Vector3 fallenPosition = targetPosition - fallUnits * Vector3Int.up;
        timeSinceStart = 0f;
        float totalFallDuration = fallUnits * fallDuration;

        while (timeSinceStart < totalFallDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, fallenPosition, timeSinceStart / totalFallDuration);
            timeSinceStart += Time.deltaTime;
            yield return null;
        }
        transform.position = fallenPosition;
        isMoving = false;

        if (_isFreeFall)
        {
            ReInitialize();
        }
    }

    private void ReInitialize()
    {
        _isFreeFall = false;
        transform.position = matrixCollider.realWorldPosition;
        Move(Direction.IDLE); // check for fall
    }

}
