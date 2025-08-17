using UnityEngine;

public class JoystickControl : IControlInput
{
    private Joystick joystick;

    public JoystickControl(Joystick joystick)
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
        // ControlType1’de hýz butonu yok, her zaman false
        return false;
    }
}
