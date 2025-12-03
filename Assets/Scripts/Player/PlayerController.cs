using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 8f;
    public float jumpForce = 16f;
    public float jumpHoldMultiplier = 0.5f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Component References
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // State Variables
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private float horizontalInput;
    [Header("Combat")]
    public int maxHealth = 3;

    private int currentHealth;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        GetInput();
        HandleJump();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateCoyoteTime();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void HandleMovement()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(horizontalInput * runSpeed, rb.linearVelocity.y);

        // Flip sprite según dirección
        if (horizontalInput > 0) spriteRenderer.flipX = false;
        else if (horizontalInput < 0) spriteRenderer.flipX = true;
    }

    private void HandleJump()
    {
        // Coyote Time
        if (IsGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBufferTime;
        else jumpBufferCounter -= Time.deltaTime;

        // Ejecutar salto
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            jumpBufferCounter = 0;
        }

        // Salto variable (hold/release) - CORREGIDO
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpHoldMultiplier);
            isJumping = false;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (IsGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }

private void UpdateAnimations()
    {
        if (animator != null)
        {
            // CAMBIO AQUÍ: Usamos "Movement" porque así lo llamaste en el Animator
            animator.SetFloat("Movement", Mathf.Abs(horizontalInput)); 
            
            animator.SetBool("IsGrounded", IsGrounded());
            animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Visual debug para ground check
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Efecto de daño (parpadeo, retroceso, etc.)
            StartCoroutine(DamageEffect());
        }
    }

    private void Die()
    {
        GameManager.Instance.PlayerDeath();
    }

    private System.Collections.IEnumerator DamageEffect()
    {
        // Hacer que el jugador parpadee
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
    // Añade esto antes de la última llave de cierre }
    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true; // Para que pueda seguir maniobrando en el aire
    }
}