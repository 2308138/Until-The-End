using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public GameObject[] attackHitbox;

    [SerializeField][HideInInspector] private Animator playerAnimator;
    [SerializeField][HideInInspector] private Transform playerTransform;
    [SerializeField][HideInInspector] private SwordController swordController;
    [SerializeField][HideInInspector] private PlayerController playerController;

    [Header("--- Combat Settings ---")]
    [SerializeField] public int currentHealth = 0;
    [SerializeField] public int currentCombo = 0;
    [SerializeField] public float lungeForce = 0F;

    [SerializeField][HideInInspector] private int maxHealth = 5;
    [SerializeField][HideInInspector] private float damageCooldown = 1F;
    [SerializeField][HideInInspector] private float lastDamageTime = 0F;
    [SerializeField][HideInInspector] private float attackCooldown = 0.3F;
    [SerializeField][HideInInspector] private float lastAttackTime = -Mathf.Infinity;

    [SerializeField][HideInInspector] public bool isAttacking = false;
    [SerializeField][HideInInspector] public bool IsAttackingOnCooldown() { return Time.time < lastAttackTime + attackCooldown; }

    void Start()
    {
        currentHealth = maxHealth;
        playerAnimator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
        swordController = GetComponentInChildren<SwordController>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking && !IsAttackingOnCooldown())
        {
            HandleCombo();
            swordController.HandleAnimations(playerController.lastMoveDirection);
        }
    }

    void HandleCombo()
    {
        if (currentCombo >= attackHitbox.Length)
            ResetCombo();

        if (Input.GetKeyDown(KeyCode.K) && currentCombo <= (attackHitbox.Length))
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            currentCombo++;

            if (playerController.lastMoveDirection != Vector2.zero)
                playerController.playerRB.linearVelocity += playerController.lastMoveDirection * lungeForce;

            int hitboxIndex = Mathf.Clamp(currentCombo - 1, 0, attackHitbox.Length - 1);
            GameObject attack = Instantiate(attackHitbox[hitboxIndex], playerTransform.position, Quaternion.identity);
            attack.transform.right = playerController.lastMoveDirection;

            playerAnimator.SetBool("isAttacking", isAttacking);
            StartCoroutine(ResetAttackAnimation());

            Invoke(nameof(EndAttack), attackCooldown);
        }
    }

    void ResetCombo()
    {
        currentCombo = 0;
    }

    void EndAttack()
    {
        playerController.playerRB.linearVelocity = Vector2.zero;
        playerController.canMove = true;
        isAttacking = false;

        if (currentCombo >= 4)
            ResetCombo();
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.35F);
        isAttacking = false;
        playerAnimator.SetBool("isAttacking", isAttacking);
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