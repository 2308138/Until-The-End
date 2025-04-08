using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public GameObject basicAttackHitbox;
    [SerializeField] public GameObject finalAttackHitbox;

    [SerializeField][HideInInspector] private Animator animator;

    [Header("--- Combat Settings ---")]
    [SerializeField] public int currentHealth = 0;
    [SerializeField] public int currentCombo = 0;

    [SerializeField][HideInInspector] private int maxHealth = 5;
    [SerializeField][HideInInspector] private float damageCooldown = 1F;
    [SerializeField][HideInInspector] private float lastDamageTime = 0F;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K) && currentCombo < 3)
        {
            HandleCombo();
            HandleAnimation();
        }
    }

    void HandleCombo()
    {
        Vector2 direction = transform.position.normalized;

        if (currentCombo > 3)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentCombo++;
            currentCombo = Mathf.Clamp(currentCombo, 0, 3);

            switch (currentCombo)
            {
                case 1:
                    StartCoroutine(BasicAttackHitbox(direction));
                    break;
                case 2:
                    StartCoroutine(BasicAttackHitbox(direction));
                    break;
                case 3:
                    StartCoroutine(FinalAttackHitbox(direction));
                    break;
            }
        }

    }

    void HandleAnimation()
    {

    }

    IEnumerator BasicAttackHitbox(Vector2 attackDirection)
    {
        basicAttackHitbox.SetActive(true);
        basicAttackHitbox.transform.position = attackDirection;
        yield return new WaitForSeconds(0.2F);
        basicAttackHitbox.SetActive(false);
    }

    IEnumerator FinalAttackHitbox(Vector2 attackDirection)
    {
        finalAttackHitbox.SetActive(true);
        finalAttackHitbox.transform.position = attackDirection;
        yield return new WaitForSeconds(0.2F);
        finalAttackHitbox.SetActive(false);
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