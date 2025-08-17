using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class FPSManager : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float deltaTime = 0.0f;
    private float updateTimer = 0f;
    private float updateRate = 0.25f; // 4 FPS’de güncelle

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        updateTimer += Time.unscaledDeltaTime;

        if (updateTimer >= updateRate)
        {
            updateTimer = 0f;

            float fps = 1.0f / deltaTime;
            if (fpsText != null)
                fpsText.SetText(Mathf.Ceil(fps).ToString()); // TMP SetText GC’yi azaltýr
        }
    }

}
