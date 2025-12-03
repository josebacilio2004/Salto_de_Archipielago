using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    private Vector2 direction;

    // Se llama desde el EnemyAI
    public void Launch(Vector2 dir)
    {
        direction = dir;
        Destroy(gameObject, 3f); // Se autodestruye a los 3 seg
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Asume que tu Player tiene el método TakeDamage
            other.GetComponent<PlayerController>().TakeDamage(damage); 
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Se rompe al tocar suelo
        }
    }
}