using Character;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (PlayerController.instance.canGethit)
        {
            PlayerController.instance.TakeDamage(1);
        }
    }
}
