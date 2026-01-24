using System.Collections;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MenuCamera : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float smoothTime = 0.25f;
    [SerializeField] float maxSpeed = 50f;
    
    [Header("Return To Origin")]
    [SerializeField] float returnDuration = 0.35f;
    [SerializeField]
    AnimationCurve returnCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 velocity;
    Vector3 targetPosition;
    Vector3 originPosition;

    Coroutine returnRoutine;
    bool isReturning;

    void Awake()
    {
        originPosition = transform.position;
        targetPosition = originPosition;
    }

    void LateUpdate()
    {
        if (isReturning)
            return;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime,
            maxSpeed
        );
    }

    public void MoveUp(float moveUnits) => MoveCamera(moveUnits, Direction.Up);
    public void MoveDown(float moveUnits) => MoveCamera(moveUnits, Direction.Down);
    public void MoveLeft(float moveUnits) => MoveCamera(moveUnits, Direction.Left);
    public void MoveRight(float moveUnits) => MoveCamera(moveUnits, Direction.Right);

    public void MoveCamera(float moveUnits, Direction direction)
    {
        if (isReturning)
            return;

        Vector3 delta = Vector3.zero;

        switch (direction)
        {
            case Direction.Left:
                delta = Vector3.left * moveUnits;
                break;
            case Direction.Right:
                delta = Vector3.right * moveUnits;
                break;
            case Direction.Up:
                delta = Vector3.up * moveUnits;
                break;
            case Direction.Down:
                delta = Vector3.down * moveUnits;
                break;
        }

        targetPosition += delta;
    }

    public void ReturnToOrigin()
    {
        if (returnRoutine != null)
            StopCoroutine(returnRoutine);

        returnRoutine = StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        isReturning = true;

        Vector3 from = transform.position;
        Vector3 to = originPosition;

        float elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / returnDuration);
            float eased = returnCurve.Evaluate(t);

            transform.position = Vector3.LerpUnclamped(from, to, eased);
            yield return null;
        }

        transform.position = to;

        targetPosition = originPosition;
        velocity = Vector3.zero;

        isReturning = false;
        returnRoutine = null;
    }
}
