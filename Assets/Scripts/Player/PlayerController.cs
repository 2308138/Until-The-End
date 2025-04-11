using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public LayerMask enemyLayer;

    [SerializeField][HideInInspector] private Collider2D playerCol;
    [SerializeField][HideInInspector] private Rigidbody2D playerRB;
    [SerializeField][HideInInspector] Vector2 movementInput;
    [SerializeField][HideInInspector] Vector2 lastMoveDirection = Vector2.down;
    [SerializeField][HideInInspector] private Vector2 dashDirection;
    [SerializeField][HideInInspector] private Animator playerAnimator;
    [SerializeField][HideInInspector] private SpriteRenderer playerSprite;

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
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
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

        if (movementInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(movementInput.x), 1, 1);

        bool isMoving = movementInput != Vector2.zero;
        playerAnimator.SetBool("isMoving", isMoving);
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
        {
            isDashing = true;
            lastDashTime = Time.time;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            playerCol.enabled = false;

            playerAnimator.SetTrigger("Dash");
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
}