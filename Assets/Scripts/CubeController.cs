using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private MovableCube _movableCube;

    private void Start()
    {
        _movableCube = GetComponent<MovableCube>();
        _movableCube.Move(Direction.IDLE);
    }

    void Update()
    {
        if (!_movableCube.isMoving)
        {
            Direction direction = GetInputDirection();
            if (direction != Direction.IDLE)
            {
                _movableCube.Move(direction);
            }
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
