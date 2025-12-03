using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Animator animator; // Arrastra el Animator aquí en el inspector
    public enum EnemyState { Patrol, Alert, Charge, Stagger, Dead } // Agregado estado Dead
    
    [Header("AI Settings")]
    public float patrolSpeed = 2f;
    public float chargeSpeed = 5f;
    public float detectionRange = 3f;
    public float telegraphTime = 1f;
    public float staggerTime = 1f;

    [Header("Combat")]
    public int maxHealth = 2;
    public int contactDamage = 1;

    [Header("References")]
    public Transform player;
    public LayerMask groundLayer;
    public LayerMask playerLayer; // Asegúrate de asignar esto en el inspector

    private EnemyState currentState;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private float stateTimer;
    private bool facingRight = true;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Si no asignaste el animator en el inspector, intenta obtenerlo
        if (animator == null) animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        
        if (player == null) 
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if(playerObj != null) player = playerObj.transform;
        }
            
        TransitionToState(EnemyState.Patrol);
    }

    void Update()
    {
        if (currentState == EnemyState.Dead) return; // Si está muerto, no hace nada
        StateMachineUpdate();
        UpdateAnimator(); // Actualizar animaciones constantemente
    }

    void FixedUpdate()
    {
        if (currentState == EnemyState.Dead) return;
        StateMachineFixedUpdate();
    }

    private void StateMachineUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Alert:
                AlertUpdate();
                break;
            case EnemyState.Charge:
                ChargeUpdate();
                break;
            case EnemyState.Stagger:
                StaggerUpdate();
                break;
        }
    }

    private void StateMachineFixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolFixedUpdate();
                break;
            case EnemyState.Charge:
                ChargeFixedUpdate();
                break;
        }
    }

    // --- LÓGICA DE ANIMACIÓN ---
    private void UpdateAnimator()
    {
        if (animator == null) return;

        // Si el estado es Charge (Perseguir) o Alert, activamos "Atacando"
        bool isAttacking = (currentState == EnemyState.Charge || currentState == EnemyState.Alert);
        animator.SetBool("Atacando", isAttacking);
    }

    #region State Behaviors
    private void PatrolUpdate()
    {
        // Detección del jugador
        if (player != null && PlayerInSight() && PlayerInRange())
        {
            TransitionToState(EnemyState.Alert);
        }
    }

    private void PatrolFixedUpdate()
    {
        rb.linearVelocity = new Vector2(patrolSpeed * (facingRight ? 1 : -1), rb.linearVelocity.y);
        
        if (!CheckGroundAhead())
        {
            Flip();
        }
    }

    private void AlertUpdate()
    {
        stateTimer -= Time.deltaTime;
        
        // Mirar hacia el jugador
        if (player != null)
        {
            bool shouldFaceRight = player.position.x > transform.position.x;
            if (facingRight != shouldFaceRight) Flip();
        }

        if (stateTimer <= 0)
        {
            TransitionToState(EnemyState.Charge);
        }
    }

    private void ChargeUpdate()
    {
        if (player != null && !PlayerInRange())
        {
            TransitionToState(EnemyState.Patrol);
        }
    }

    private void ChargeFixedUpdate()
    {
        float direction = facingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * chargeSpeed, rb.linearVelocity.y);
    }

    private void StaggerUpdate()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            TransitionToState(EnemyState.Patrol);
        }
    }
    #endregion

    private void TransitionToState(EnemyState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case EnemyState.Alert:
                stateTimer = telegraphTime;
                break;
            case EnemyState.Stagger:
                stateTimer = staggerTime;
                rb.linearVelocity = Vector2.zero; // Detenerse al recibir daño
                break;
        }
    }

    #region Helper Methods
    private bool PlayerInSight()
    {
        if (player == null) return false;
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);
        
        // Raycast para ver si hay paredes en medio
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);
        return hit.collider == null; 
    }

    private bool PlayerInRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    private bool CheckGroundAhead()
    {
        Vector2 checkPosition = (Vector2)transform.position + (facingRight ? Vector2.right : Vector2.left) * 0.5f;
        return Physics2D.OverlapCircle(checkPosition, 0.1f, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }
    #endregion

    // --- SALUD Y MUERTE ---

    public void Morir()
    {
        if (currentState == EnemyState.Dead) return;

        currentState = EnemyState.Dead;
        rb.linearVelocity = Vector2.zero;
        
        // Desactivar colisiones para que no dañe al jugador mientras muere
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; // Desactiva este script

        if (animator != null)
        {
            animator.SetTrigger("Muerte"); // Activa estado 'rocadead'
        }

        // Destruir el objeto después de 1 segundo (ajusta según dure tu animación)
        Destroy(gameObject, 1f); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Dead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí llamarías al script del jugador para hacerle daño
             // collision.gameObject.GetComponent<PlayerController>().TakeDamage(contactDamage);
             Debug.Log("Jugador golpeado");
        }
    }
    
    // Debug visual
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}