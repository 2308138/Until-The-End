using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public GameObject[] attackHitbox;

    [SerializeField][HideInInspector] private Animator playerAnimator;
    [SerializeField][HideInInspector] private Transform playerTransform;

    [Header("--- Combat Settings ---")]
    [SerializeField] public int currentHealth = 0;
    [SerializeField] public int currentCombo = 0;

    [SerializeField][HideInInspector] private int maxHealth = 5;
    [SerializeField][HideInInspector] private float damageCooldown = 1F;
    [SerializeField][HideInInspector] private float lastDamageTime = 0F;
    [SerializeField][HideInInspector] private Vector2 lastMoveDirection = Vector2.right;

    void Start()
    {
        currentHealth = maxHealth;
        playerAnimator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && currentCombo <= 4)
        {
            HandleCombo();
        }
    }

    void HandleCombo()
    {
        if (currentCombo >= attackHitbox.Length)
            ResetCombo();

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input != Vector2.zero)
            lastMoveDirection = input.normalized;

        if (Input.GetKeyDown(KeyCode.K) && currentCombo <= 4)
        {
            currentCombo = Mathf.Clamp(currentCombo, 0, attackHitbox.Length - 1);
            GameObject attack = Instantiate(attackHitbox[currentCombo], playerTransform.position, Quaternion.identity);
            attack.transform.right = lastMoveDirection;

            currentCombo++;
            if (currentCombo >= attackHitbox.Length)
                ResetCombo();

            playerAnimator.SetBool("isAttacking", true);
        }
    }

    void ResetCombo()
    {
        currentCombo = 0;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyAttack") && Time.time > lastDamageTime + damageCooldown)
        {
            TakeDamage(1);
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}