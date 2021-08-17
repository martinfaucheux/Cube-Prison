using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] float minMoveCooldown = 0.3f;

    private GraphEntity _graphEntity;

    private float _lastMoveTime;

    private bool canMove
    {
        get
        {
            return !_graphEntity.isMoving && (Time.time - _lastMoveTime) > minMoveCooldown;
        }
    }

    private void Start()
    {
        _graphEntity = GetComponent<GraphEntity>();
    }

    void Update()
    {
        if (!_graphEntity.isMoving)
        {
            Direction direction = GetInputDirection();
            if (direction != Direction.IDLE)
            {
                if (_graphEntity.IsValidDirection(direction))
                {
                    _graphEntity.Move(direction);
                }
            }
            _lastMoveTime = Time.time;
        }
    }

    private Direction GetInputDirection()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        Direction direction = Direction.IDLE;

        if (Mathf.Abs(verticalAxis) > Mathf.Epsilon)
        {
            direction = verticalAxis > 0 ? Direction.UP : Direction.DOWN;
        }
        else if (Mathf.Abs(horizontalAxis) > Mathf.Epsilon)
        {
            direction = horizontalAxis > 0 ? Direction.RIGHT : Direction.LEFT;
        }

        return direction;
    }
}
