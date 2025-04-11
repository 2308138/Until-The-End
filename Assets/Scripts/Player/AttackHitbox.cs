using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("--- Hitbox Settings ---")]
    [SerializeField] public float lifetime = 0F;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}