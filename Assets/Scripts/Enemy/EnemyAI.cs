using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField][HideInInspector] private Transform target;

    [Header("--- Movement Settings ---")]
    [SerializeField] public float moveSpeed = 0F;

    [Header("--- Combat Settings ---")]
    [SerializeField] public int currentHealth = 0;

    [SerializeField][HideInInspector] private float damageCooldown = 0.2F;
    [SerializeField][HideInInspector] private float lastDamageTime = 0F;

    void Update()
    {
        if (target == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack") && Time.time > lastDamageTime + damageCooldown)
        {
            TakeDamage(1);
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}