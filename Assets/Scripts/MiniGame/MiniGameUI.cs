using UnityEngine;
using TMPro;

public class MiniGameUI : MonoBehaviour
{
    public static MiniGameUI Instance;

    [Header("UI References")]
    public TextMeshProUGUI timeText; // Kalan zaman

    [Header("Timer Settings")]
    public int startTime; // Ba�lang�� zaman� (saniye)
    private float currentTime; // Saya�

    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        currentTime = startTime;
    }

    public void StopTimer()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            // Slow motion'da bile d�zg�n �al��mas� i�in unscaledDeltaTime kullan
            currentTime -= Time.unscaledDeltaTime;
            currentTime = Mathf.Max(0, currentTime);

            timeText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }
}
