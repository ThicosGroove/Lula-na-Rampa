// 25/09/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerManager playerManager;
    private Camera mainCamera;
    private PlayerMovement movement;
    private PlayerInputActions inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float swipeThreshold = 0.05f;

    public bool canMove = false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Keyboard inputs
        inputActions.Player.Right.performed += ctx => MoveRight();
        inputActions.Player.Left.performed += ctx => MoveLeft();
        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Roll.performed += ctx => Roll();

        // Mouse and touch inputs
        inputActions.Player.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        inputActions.Player.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Right.performed -= ctx => MoveRight();
        inputActions.Player.Left.performed -= ctx => MoveLeft();
        inputActions.Player.Jump.performed -= ctx => Jump();
        inputActions.Player.Roll.performed -= ctx => Roll();

        inputActions.Player.PrimaryContact.started -= ctx => StartTouchPrimary(ctx);
        inputActions.Player.PrimaryContact.canceled -= ctx => EndTouchPrimary(ctx);
    }

    private void MoveRight()
    {
        if (!canMove) { return;  }

        movement.MoveToLane(1);
        movement.HandleRotation(1);
    }

    private void MoveLeft()
    {
        if (!canMove) { return; }

        movement.MoveToLane(-1);
        movement.HandleRotation(-1);
    }

    private void Jump()
    {
        if (!canMove) { return; }

        movement.Jump();
    }

    private void Roll()
    {
        if (!canMove) { return; }

        movement.Roll();
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        startTouchPosition = Utils.ScreenToWorld(mainCamera, inputActions.Player.PrimaryPosition.ReadValue<Vector2>());
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        endTouchPosition = Utils.ScreenToWorld(mainCamera, inputActions.Player.PrimaryPosition.ReadValue<Vector2>());
        DetectSwipe();
    }


    private void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > swipeThreshold)
            {
                MoveRight();
            }
            else if (swipeDelta.x < -swipeThreshold)
            {
                MoveLeft();
            }
        }
        else
        {
            if (swipeDelta.y > swipeThreshold)
            {
                Jump();
            }
            else if (swipeDelta.y < -swipeThreshold)
            {
                Roll();
            }
        }
    }
}