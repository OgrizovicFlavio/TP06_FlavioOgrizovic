using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Canvas canvas;

    public PlayerSO playerSO;
    private AudioManager audioManager;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerRb2D;
    private bool isGrounded;
    private bool facingRight = true;
    private float disableTime = 0.1f;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        disableTime -= Time.deltaTime;

        if (disableTime < 0f)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerSO.groundCheckRadius, playerSO.worldLayerMask);
        }

    }

    public void Jump()
    {
        if (isGrounded)
        {
            audioManager.Play("PlayerJump");

            playerRb2D.AddForce(new Vector2(0f, playerSO.jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            disableTime = 0.1f;
        }
    }

    public void DoubleJump()
    {
        audioManager.Stop("PlayerJump");
        audioManager.Play("PlayerDoubleJump");

        playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, 0f);
        playerRb2D.AddForce(new Vector2(0f, playerSO.jumpForce), ForceMode2D.Impulse);
    }

    public void Move(float moveInput)
    {
        playerRb2D.velocity = new Vector2(moveInput * playerSO.movementSpeed, playerRb2D.velocity.y);

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        canvas.transform.Rotate(0f, 180f, 0f);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
