using UnityEngine;

public interface IControlInput
{
    // Oyuncunun y�nlendirme girdisi (x: sa�-sol, y: yukar�-a�a��)
    Vector2 GetDirection();

    // H�z art�rma butonu bas�l� m�
    bool IsSpeedBoostActive();
}