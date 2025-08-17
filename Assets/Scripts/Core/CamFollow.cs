using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform targetPlayer;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        // Pozisyonu kuþun rotasyonuna göre offset ile ayarla
        transform.position = targetPlayer.position + targetPlayer.rotation * offset;

       transform.LookAt(targetPlayer.transform);
    }
}
