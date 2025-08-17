using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform targetPlayer;
    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        // Pozisyonu ku�un rotasyonuna g�re offset ile ayarla
        transform.position = targetPlayer.position + targetPlayer.rotation * offset;

       transform.LookAt(targetPlayer.transform);
    }
}
