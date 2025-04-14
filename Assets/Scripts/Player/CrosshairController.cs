using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public Transform playerTransform;

    [SerializeField][HideInInspector] private Vector2 lastMoveDirection = Vector2.right;

    [Header("--- Crosshair Settings ---")]
    [SerializeField][HideInInspector] private float crosshairDistance = 0F;

    void Awake()
    {
        crosshairDistance = 1.2F;
    }

    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementInput != Vector2.zero)
        {
            lastMoveDirection = movementInput.normalized;
        }

        transform.position = playerTransform.position + (Vector3)(lastMoveDirection * crosshairDistance);
    }
}