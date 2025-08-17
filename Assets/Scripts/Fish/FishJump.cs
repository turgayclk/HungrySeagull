using UnityEngine;
using System.Collections;

public class FishJump : MonoBehaviour
{
    public ParticleSystem jumpParticle;
    public float minWaitTime = 3f;
    public float maxWaitTime = 7f;
    public float jumpForce = 5f;
    public float gravity = 9.8f;
    public float jumpParticleDuration = 1f;

    Vector3 startPos;
    Vector3 velocity;
    bool isJumping = false;

    void Start()
    {
        startPos = transform.position;
        jumpParticle.Stop();
        StartCoroutine(FishRoutine());
    }

    public void StartRoutineFish()
    {
        transform.position = RandomPos();
        startPos = transform.position;
        jumpParticle.Stop();
        StartCoroutine(FishRoutine());
    }

    IEnumerator FishRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            jumpParticle.Play();
            yield return new WaitForSeconds(jumpParticleDuration);
            jumpParticle.Stop();

            yield return new WaitForSeconds(waitTime - jumpParticleDuration);

            Jump();

            while (isJumping)
                yield return null;

            transform.position = RandomPos();
            startPos = transform.position;
        }
    }

    public Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-73, -6), startPos.y, Random.Range(30, -36));
    }

    void Jump()
    {
        velocity = Vector3.up * jumpForce;
        isJumping = true;
    }

    void Update()
    {
        FishGravity();
    }

    void FishGravity()
    {
        if (isJumping)
        {
            // Gravity uygulama
            velocity.y -= gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            // Yükseklik durumuna göre rotasyonu ayarla
            Vector3 rot = transform.localEulerAngles;

            if (velocity.y > 0) // Çýkýþ
            {
                float targetX = -90f;
                rot.x = Mathf.LerpAngle(rot.x, targetX, Time.deltaTime * 2f);
            }
            else if (velocity.y < 0) // Düþüþ
            {
                float targetX = 70;
                rot.x = Mathf.LerpAngle(rot.x, targetX, Time.deltaTime * 2f);
            }

            transform.localEulerAngles = rot;

            // Suya deðince durdur ve rotasyonu sýfýrla
            if (transform.position.y <= startPos.y)
            {
                transform.position = startPos;
                velocity = Vector3.zero;
                isJumping = false;

                // Rotasyonu yumuþak þekilde sýfýrla
                StartCoroutine(ResetRotation());
            }
        }
    }

    IEnumerator ResetRotation()
    {
        float t = 0;
        Vector3 currentRot = transform.localEulerAngles;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            currentRot.x = Mathf.LerpAngle(currentRot.x, 0f, t);
            transform.localEulerAngles = currentRot;
            yield return null;
        }
    }

}
