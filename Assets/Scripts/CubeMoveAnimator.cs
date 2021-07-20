using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMoveAnimator : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.3f;
    [SerializeField] float fallDuration = 0.1f;

    [SerializeField] Transform cubeMeshTransform;
    [SerializeField] Transform bumpTransform;

    private static int voidFallUnits = 10;

    public bool isMoving { get; private set; } = false;

    public void AnimateMove(GraphVertex graphVertex)
    {
        Direction direction = graphVertex.direction;

        Vector3 initRealPosition = graphVertex.headNode.realWorldPosition;
        Vector3 newPosition = graphVertex.tailNode.realWorldPosition;

        int fallUnits = IsFreeFall(graphVertex) ? voidFallUnits : GetFallUnits(graphVertex);
        Vector3Int rollDisplacement = direction.To3dPos(graphVertex.tailNode.normal);

        StartCoroutine(MoveCoroutine(rollDisplacement, fallUnits));
    }

    private IEnumerator MoveCoroutine(Vector3Int rollDisplacement, int fallUnits)
    {
        isMoving = true;
        Vector3 initPosition = transform.position;
        Quaternion initRotation = transform.rotation;

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

        // if (_isFreeFall)
        // {
        //     ReInitialize();
        // }
    }

    // private void ReInitialize()
    // {
    //     _isFreeFall = false;
    //     transform.position = matrixCollider.realWorldPosition;
    //     Move(Direction.IDLE); // check for fall
    // }

    public static int GetFallUnits(GraphVertex graphVertex)
    {
        if (graphVertex.headNode.normal == graphVertex.tailNode.normal)
            return Mathf.RoundToInt(Vector3.Dot(
                graphVertex.headNode.realWorldPosition - graphVertex.tailNode.realWorldPosition,
                graphVertex.headNode.normal
            ));
        return 0;
    }

    public static bool IsFreeFall(GraphVertex graphVertex)
    {
        return graphVertex.tailNode is VoidNode;
    }

}
