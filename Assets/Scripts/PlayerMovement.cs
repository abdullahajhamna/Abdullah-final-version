using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float rotationSpeed = 15f; // Controls how fast the character rotates

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float targetRotation = 0f; // 0 = right, 180 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Handle rotation based on movement direction
        if (moveInput > 0 && !isFacingRight)
        {
            isFacingRight = true;
            targetRotation = 0f;
        }
        else if (moveInput < 0 && isFacingRight)
        {
            isFacingRight = false;
            targetRotation = 180f;
        }

        // Smooth rotation
        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0f, targetRotation, 0f);
        transform.rotation = Quaternion.Lerp(currentRot, targetRot, rotationSpeed * Time.deltaTime);
    }

    // Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}