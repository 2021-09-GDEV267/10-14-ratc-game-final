// GENERATED AUTOMATICALLY FROM 'Assets/Input/TouchControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TouchControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchControls"",
    ""maps"": [
        {
            ""name"": ""TouchPad"",
            ""id"": ""c9a6b5e5-4e4a-4aa8-ba38-c17487a04d47"",
            ""actions"": [
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""PassThrough"",
                    ""id"": ""41bc8874-6db8-4460-b6ac-0000d6d0e525"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e8f2c549-7e28-4c92-bf9c-d77b0a199bba"",
                    ""path"": ""<Touchscreen>/press"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // TouchPad
        m_TouchPad = asset.FindActionMap("TouchPad", throwIfNotFound: true);
        m_TouchPad_TouchPress = m_TouchPad.FindAction("TouchPress", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // TouchPad
    private readonly InputActionMap m_TouchPad;
    private ITouchPadActions m_TouchPadActionsCallbackInterface;
    private readonly InputAction m_TouchPad_TouchPress;
    public struct TouchPadActions
    {
        private @TouchControls m_Wrapper;
        public TouchPadActions(@TouchControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchPress => m_Wrapper.m_TouchPad_TouchPress;
        public InputActionMap Get() { return m_Wrapper.m_TouchPad; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchPadActions set) { return set.Get(); }
        public void SetCallbacks(ITouchPadActions instance)
        {
            if (m_Wrapper.m_TouchPadActionsCallbackInterface != null)
            {
                @TouchPress.started -= m_Wrapper.m_TouchPadActionsCallbackInterface.OnTouchPress;
                @TouchPress.performed -= m_Wrapper.m_TouchPadActionsCallbackInterface.OnTouchPress;
                @TouchPress.canceled -= m_Wrapper.m_TouchPadActionsCallbackInterface.OnTouchPress;
            }
            m_Wrapper.m_TouchPadActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
            }
        }
    }
    public TouchPadActions @TouchPad => new TouchPadActions(this);
    public interface ITouchPadActions
    {
        void OnTouchPress(InputAction.CallbackContext context);
    }
}
