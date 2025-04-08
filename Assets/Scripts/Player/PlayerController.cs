using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public LayerMask enemyLayer;

    [SerializeField][HideInInspector] private Collider2D playerCol;
    [SerializeField][HideInInspector] private Rigidbody2D playerRB;
    [SerializeField][HideInInspector] Vector2 movementInput;
    [SerializeField][HideInInspector] Vector2 lastMoveDirection = Vector2.down;
    [SerializeField][HideInInspector] private Animator playerAnim;
    [SerializeField][HideInInspector] private Vector2 dashDirection;

    [Header("--- Movement Settings ---")]
    [SerializeField] public float moveSpeed = 0F;
    [SerializeField] public float dashSpeed = 0F;
    [SerializeField] public float dashDuration = 0F;
    [SerializeField] public float dashCooldown = 0F;

    [SerializeField][HideInInspector] private float lastDashTime = 0F;
    [SerializeField][HideInInspector] private bool isDashing = false;

    void Start()
    {
        playerCol = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleDash();
    }

    void HandleMovement()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementInput != Vector2.zero)
            lastMoveDirection = movementInput;
        playerRB.linearVelocity = movementInput * moveSpeed;
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
        {
            isDashing = true;
            lastDashTime = Time.time;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            playerCol.enabled = false;
        }

        playerRB.MovePosition(playerRB.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime);

        if (isDashing)
        {
            playerRB.MovePosition(playerRB.position + dashDirection * dashSpeed * Time.fixedDeltaTime);

            if (Time.time > lastDashTime + dashDuration)
            {
                isDashing = false;
                playerCol.enabled = true;
            }
        }
    }

    void HandleAnimation()
    {

    }
}