using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    private InputTouchControls inputActions;
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputTouchControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        Debug.LogWarning("Habilitado");
    }

    private void OnDisable()
    {        
        inputActions.Disable();
    }

    void Start()
    {
        inputActions.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        inputActions.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnStartTouch?.Invoke(Utils.ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(Utils.ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
    }
}

