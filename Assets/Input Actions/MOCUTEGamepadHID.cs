using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// Using InputControlLayoutAttribute, we tell the system about the state
// struct we created, which includes where to find all the InputControl
// attributes that we placed on there. This is how the Input System knows
// what controls to create and how to configure them.
[InputControlLayout(stateType = typeof(MOCUTEGamepadInputReport))]
#if UNITY_EDITOR
[InitializeOnLoad] // Make sure static constructor is called during startup
#endif
public class MOCUTEGamepadHID : Gamepad
{
    static MOCUTEGamepadHID()
    {
        InputSystem.RegisterLayout<MOCUTEGamepadHID>("Logitech Dual Action (Unofficial)", 
            new InputDeviceMatcher()
                .WithInterface("HID")
                .WithManufacturer("MOCUTE")
                .WithProduct("MOCUTE Gamepad"));
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init() { }
}
