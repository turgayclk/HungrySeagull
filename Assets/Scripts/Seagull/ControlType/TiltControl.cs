using UnityEngine;

public class TiltControl : IControlInput
{
    private Vector3 calibrationOffset;
    private float sensitivity;
    private float deadZone;
    private bool speedBoost;

    public TiltControl(float sensitivity, float deadZone)
    {
        this.sensitivity = sensitivity;
        this.deadZone = deadZone;
        Calibrate();
    }

    public void Calibrate()
    {
        calibrationOffset = Input.acceleration;
    }

    public Vector2 GetDirection()
    {
        Vector3 tilt = Input.acceleration - calibrationOffset;

        if (Mathf.Abs(tilt.x) < deadZone) tilt.x = 0f;
        if (Mathf.Abs(tilt.y) < deadZone) tilt.y = 0f;

        return new Vector2(tilt.x * sensitivity, -tilt.y * sensitivity);
    }

    public bool IsSpeedBoostActive()
    {
        return speedBoost;
    }
}
