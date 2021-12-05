using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private TouchControls touchControls;
    // Start is called before the first frame update
    void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }
    private void Start()
    {
        touchControls.TouchPad.TouchPress.started += context => StartTouch(context);
        touchControls.TouchPad.TouchPress.canceled += context => EndTouch(context);

    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        print("Touch Started " + touchControls.TouchPad.TouchPress.ReadValue<Vector2>());
        if (OnStartTouch != null)
            OnStartTouch(touchControls.TouchPad.TouchPress.ReadValue<Vector2>(), (float)context.startTime);
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        print("Touch Ended " + touchControls.TouchPad.TouchPress.ReadValue<Vector2>());
        if (OnEndTouch != null)
            OnEndTouch(touchControls.TouchPad.TouchPress.ReadValue<Vector2>(), (float)context.time);
        print("Time between touch start and end : " + ((float)context.time - (float)context.startTime));
    }
}