using UnityEngine;

public class EnemyWeakPoint : MonoBehaviour
{
    private EnemyAI mainEnemyScript;

    void Start()
    {
        mainEnemyScript = GetComponentInParent<EnemyAI>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Impulsar al jugador hacia arriba (rebote)
            PlayerController player = collision.GetComponent<PlayerController>();
            // Nota: Asegúrate de tener el método Bounce() público en PlayerController
            if (player != null)
            {
                player.Bounce(); 
            }
            else 
            {
                // Fallback por si no tienes el método Bounce
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if(rb != null) rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
            }

            // 2. Avisar al script principal que el enemigo debe morir
            if (mainEnemyScript != null)
            {
                mainEnemyScript.Morir();
            }
            
            // Destruimos este trigger inmediatamente para que no se pueda pisar dos veces
            Destroy(gameObject); 
        }
    }
}