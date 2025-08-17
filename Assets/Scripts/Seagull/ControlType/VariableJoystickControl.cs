using UnityEngine;
using UnityEngine.UI;

public class VariableJoystickControl : IControlInput
{
    private Joystick joystick;
    private bool isSpeedPressed;

    public VariableJoystickControl(Joystick joystick)
    {
        this.joystick = joystick;
    }

    public Vector2 GetDirection()
    {
        Vector2 input = joystick.Direction;
        // Normalize the input vector to ensure consistent speed
        if (input.magnitude > 1f) input.Normalize();
        return input;
    }

    public bool IsSpeedBoostActive()
    {
        return isSpeedPressed;
    }
}
