using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private SeagullController seagullController; // Mart� controller referans�

    private bool isBeingEaten = false; // Tekrar tetiklenmeyi engeller

    private void OnTriggerEnter(Collider other)
    {
        if (isBeingEaten) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Mart� bal��� g�rd�!");
            isBeingEaten = true;

            FishJump fishJump = GetComponent<FishJump>();
            if (fishJump != null)
            {
                fishJump.enabled = false; // Bal���n z�plamas�n� devre d��� b�rak
            }

            // MiniGameManager �zerinden yakalama ba�lat
            if (seagullController != null)
            {
                Debug.Log("Fish'de ba�lat�yor seagullController.StartCatchMiniGame");
                seagullController.StartCatchMiniGame(transform);
            }
        }
    }
}
