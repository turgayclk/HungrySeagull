using UnityEngine;

public class LightTower : MonoBehaviour
{
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void StartBoxCollider()
    {
        Invoke(nameof(EnableBoxCollider), 1f);
    }

    void EnableBoxCollider()
    {
        boxCollider.enabled = true;
    }
}
