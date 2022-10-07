using Character;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D col)
    {
        PlayerController.instance.TakeDamage(1);
        Debug.Log("YE");
    }
}
