using UnityEngine;

public interface IControlInput
{
    // Oyuncunun yönlendirme girdisi (x: sað-sol, y: yukarý-aþaðý)
    Vector2 GetDirection();

    // Hýz artýrma butonu basýlý mý
    bool IsSpeedBoostActive();
}