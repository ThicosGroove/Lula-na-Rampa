using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = .1f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    private PlayerController playerController;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;


    private void Awake()
    {
        inputManager = InputManager.Instance;
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }


    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minDistance &&
            (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 2f);

            Vector3 direction3D = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction3D.x, direction3D.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Left");
        }
        else if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Right");
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Down");
        }
    }
}
