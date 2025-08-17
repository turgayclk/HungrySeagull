using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private SeagullController seagullController; // Martý controller referansý

    private bool isBeingEaten = false; // Tekrar tetiklenmeyi engeller

    private void OnTriggerEnter(Collider other)
    {
        if (isBeingEaten) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Martý balýðý gördü!");
            isBeingEaten = true;

            FishJump fishJump = GetComponent<FishJump>();
            if (fishJump != null)
            {
                fishJump.enabled = false; // Balýðýn zýplamasýný devre dýþý býrak
            }

            // MiniGameManager üzerinden yakalama baþlat
            if (seagullController != null)
            {
                Debug.Log("Fish'de baþlatýyor seagullController.StartCatchMiniGame");
                seagullController.StartCatchMiniGame(transform);
            }
        }
    }
}
