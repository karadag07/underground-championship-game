using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 8f;

    [Header("Block Movement")]
    public float blockSpeedMultiplier = 0.4f; // %60 yavaşlama

    Rigidbody2D rb;
    float moveInput;
    bool isGrounded;

    PlayerStamina playerStamina;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        // 🔥 BLOCK YAPILIYORSA HIZ DÜŞÜR
        if (playerStamina != null && playerStamina.IsBlocking())
        {
            currentSpeed *= blockSpeedMultiplier;
        }

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}