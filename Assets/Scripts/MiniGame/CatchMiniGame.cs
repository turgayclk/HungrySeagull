using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatchMiniGame : MonoBehaviour
{
    public RectTransform indicator; // hareket eden çizgi
    public RectTransform greenZone; // yeþil alan
    public float speed = 200f; // hareket hýzý
    private bool movingRight = true;
    private bool isPlaying = false;
    private System.Action<bool> onResult; // sonucu dýþarý bildirecek

    public void StartMiniGame(System.Action<bool> callback)
    {
        onResult = callback;
        isPlaying = true;
        gameObject.SetActive(true);

        float randomPos = Random.Range(-337, 359);

        greenZone.anchoredPosition = new Vector2(randomPos, greenZone.anchoredPosition.y);
    }

    private void Update()
    {
        if (!isPlaying) return;

        // Ýndicator hareketi
        float move = speed * Time.unscaledDeltaTime * (movingRight ? 1 : -1);
        indicator.anchoredPosition += new Vector2(move, 0);

        // Kenarlarda yön deðiþtirme
        if (indicator.anchoredPosition.x >= 451) movingRight = false;
        if (indicator.anchoredPosition.x <= -428) movingRight = true;

        // ekrana dokununca kontrol etme
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Mobil dokunma
        {
            bool success = RectTransformUtility.RectangleContainsScreenPoint(
                greenZone,
                indicator.position,
                null // Eðer kamera UI Camera deðilse burayý null býrakabilirsin
            );

            EndMiniGame(success);
        }

    }

    private void EndMiniGame(bool success)
    {
        isPlaying = false;
        gameObject.SetActive(false);
        onResult?.Invoke(success);
    }
}
