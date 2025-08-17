using UnityEngine;
using TMPro;

public class MiniGameUI : MonoBehaviour
{
    public static MiniGameUI Instance;

    [Header("UI References")]
    public TextMeshProUGUI timeText; // Kalan zaman

    [Header("Timer Settings")]
    public int startTime; // Baþlangýç zamaný (saniye)
    private float currentTime; // Sayaç

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
            // Slow motion'da bile düzgün çalýþmasý için unscaledDeltaTime kullan
            currentTime -= Time.unscaledDeltaTime;
            currentTime = Mathf.Max(0, currentTime);

            timeText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }
}
