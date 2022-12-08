using UnityEngine;

public class EnemyColliderDamage : MonoBehaviour
{
    [Header("Damage which enemy takes to character when they collided")]
    [SerializeField] private float damage;

    private void OnCollisionEnter2D(Collision2D playerCollision)
    {
        if (playerCollision.gameObject.tag == "Player")
        {
            playerCollision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
