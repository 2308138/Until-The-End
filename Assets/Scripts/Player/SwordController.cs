using System.Collections;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField][HideInInspector] private PlayerController playerController;
    [SerializeField][HideInInspector] private PlayerCombat playerCombat;
    [SerializeField][HideInInspector] private Transform swordTransform;
    [SerializeField][HideInInspector] private Animator swordAnimator;
    [SerializeField][HideInInspector] private Transform playerTransform;

    [Header("--- Animation Settings ---")]
    [SerializeField] public Vector2 animationOffset = new Vector3(0F, 0F);

    [SerializeField][HideInInspector] private Vector3 restingLocalPos;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerCombat = GetComponentInParent<PlayerCombat>();
        swordAnimator = GetComponent<Animator>();
        swordTransform = GetComponent<Transform>();
        playerTransform = GetComponentInParent<Transform>();
        restingLocalPos = transform.localPosition;
        
    }

    void Update()
    {
        HandleDashAnimation();
    }

    public void HandleDashAnimation()
    {
        swordAnimator.SetBool("isDashing", playerController.isDashing);
    }

    public void HandleAttackAnimations(Vector2 direction)
    {
        if (playerCombat.isAttacking)
        {
            Vector3 frontOffset = new Vector3(animationOffset.x, animationOffset.y, 0F);
            swordTransform.localPosition = restingLocalPos + frontOffset;

            int triggerIndex = Mathf.Clamp(playerCombat.currentCombo, 1, 3);

            if (triggerIndex <= 2)
            {
                Invoke(nameof(ResetPosition), 0.3F);
                swordAnimator.SetTrigger("Attack" + triggerIndex);
            }
            else if (triggerIndex == 3)
            {
                Invoke(nameof(ResetPosition), 0.385F);
                swordAnimator.SetTrigger("Attack" + triggerIndex);
            }
        }
    }

    void ResetPosition ()
    {
        transform.localPosition = restingLocalPos;
    }
}