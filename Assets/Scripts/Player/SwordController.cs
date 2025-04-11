using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField][HideInInspector] private PlayerController playerController;
    [SerializeField][HideInInspector] private PlayerCombat playerCombat;

    [Header("--- Animation Settings ---")]
    [SerializeField] private Animator swordAnimator;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerCombat = GetComponentInParent<PlayerCombat>();
        swordAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAnimations();
    }
    void HandleAnimations()
    {
        swordAnimator.SetBool("isDashing", playerController.isDashing);
    }
}