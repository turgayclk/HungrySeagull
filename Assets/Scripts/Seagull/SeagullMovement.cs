using UnityEngine;

public class SeagullMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float drag = 1f;
    [SerializeField] private float speedChangeRate = 2f; // hız geçişini yumuşatma katsayısı

    [Header("Rotation Settings")]
    [SerializeField] private float turnSpeed = 50f;
    [SerializeField] private float pitchSpeed = 30f;
    [SerializeField] private float rollAmount = 30f;
    [SerializeField] private float horizontalPitchAmount = 10f;

    [Header("Glide Settings")]
    [SerializeField] private float glideGravity = 0.5f;

    private Rigidbody rb;
    private float currentSpeed;
    private float targetSpeed;

    public bool isSpeedButtonPressed = false;
    private const float speedButtonBoost = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentSpeed = minSpeed + 1f;
        targetSpeed = currentSpeed;
    }

    public void Move(float horizontal, float vertical, ControlType controlType, bool boost = false)
    {
        // --- ROTATION ---
        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward, -vertical * pitchSpeed * Time.deltaTime, Space.Self);

        // Roll & Pitch görsellik
        float targetRoll = -horizontal * rollAmount;
        Vector3 localEuler = transform.localEulerAngles;
        localEuler.z = Mathf.LerpAngle(localEuler.z, targetRoll, Time.deltaTime * 2f);
        float targetPitch = -horizontal * horizontalPitchAmount;
        localEuler.x = Mathf.LerpAngle(localEuler.x, targetPitch, Time.deltaTime * 2f);
        transform.localEulerAngles = localEuler;

        // --- SPEED CALC ---
        float pitchAngle = transform.localEulerAngles.x;
        if (pitchAngle > 180f) pitchAngle -= 360f;

        // Boost tuşu
        if (isSpeedButtonPressed || boost)
            targetSpeed += speedButtonBoost * Time.deltaTime;

        // Type1 özel → joystick yönüne göre hız fiziği
        if (controlType == ControlType.Type1)
        {
            if (pitchAngle < -1f) // aşağı dalış
                targetSpeed += Mathf.Abs(pitchAngle) * acceleration * Time.deltaTime;
            else if (pitchAngle > 1f) // yukarı tırmanış
                targetSpeed -= pitchAngle * deceleration * Time.deltaTime;
            else // düz uçuş
                targetSpeed -= drag * Time.deltaTime;
        }

        // Clamp
        targetSpeed = Mathf.Clamp(targetSpeed, minSpeed, maxSpeed);

        // YUMUŞAK GEÇİŞ → currentSpeed yavaş yavaş targetSpeed’e yaklaşır
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

        // --- FORWARD MOVE ---
        rb.linearVelocity = transform.right * currentSpeed;

        // Glide
        rb.linearVelocity += Vector3.down * glideGravity * Time.fixedDeltaTime;
    }

    public void OnSpeedButtonDown() => isSpeedButtonPressed = true;
    public void OnSpeedButtonUp() => isSpeedButtonPressed = false;

    public float GetSpeed() => currentSpeed;
}
