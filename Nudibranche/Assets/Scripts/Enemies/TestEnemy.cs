using Character;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (PlayerController.Instance.vulnerable)
        {
            PlayerController.Instance.TakeDamage(1);
        }
    }
}
