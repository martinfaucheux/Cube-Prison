using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphEntity))]
public class CubeMoveAnimator : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.3f;
    [SerializeField] float fallDuration = 0.1f;

    [SerializeField] Transform cubeMeshTransform;
    [SerializeField] Transform bumpTransform;

    private GraphEntity _graphEntity;

    private static int _voidFallUnits = 10;

    public bool isMoving { get; private set; } = false;

    private void Start()
    {
        _graphEntity = GetComponent<GraphEntity>();
    }

    public void AnimateMove(GraphVertex graphVertex)
    {
        Direction direction = graphVertex.direction;
        Vector3 initRealPosition = graphVertex.headNode.realWorldPosition;
        Vector3 newPosition = graphVertex.tailNode.realWorldPosition;

        // TODO: this should handle free fall

        // case of instant replace
        bool isInstantReplace = graphVertex.tailNode is StartNode;
        if (isInstantReplace)
        {
            StartCoroutine(InstantPlaceToPosition(newPosition));
            return;
        }

        int fallUnits = IsFreeFall(graphVertex) ? _voidFallUnits : GetFallUnits(graphVertex);
        Vector3Int rollDisplacement = direction.To3dPos(graphVertex.tailNode.normal);

        StartCoroutine(RollCoroutine(rollDisplacement, fallUnits));
    }

    private IEnumerator InstantPlaceToPosition(Vector3 realWorldPosition)
    {
        // in case of free fall, the cube needs to be instantly reinitialized
        // NOTE: this is a coroutine to be compliante with Roll coroutine
        // also this could potentially be included in the RollCoroutine as special case
        transform.position = realWorldPosition;
        yield return null;
        _graphEntity.EndMove();
    }

    private IEnumerator RollCoroutine(Vector3Int rollDisplacement, int fallUnits)
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

        _graphEntity.EndMove();
    }

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
