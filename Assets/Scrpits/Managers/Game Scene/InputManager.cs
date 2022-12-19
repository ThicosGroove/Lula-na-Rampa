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
        Debug.LogWarning("Desabilitado");
    }

    void Start()
    {
        inputActions.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        inputActions.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext cxt)
    {
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)cxt.startTime);       
    }

    private void EndTouchPrimary(InputAction.CallbackContext cxt)
    {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)cxt.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, inputActions.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}

