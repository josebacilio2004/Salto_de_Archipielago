using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActive = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            GameManager.Instance.SetCheckpoint(transform.position);
            AudioManager.Instance.PlayCheckpoint();
            
            if (animator != null) animator.SetBool("Active", true);
        }
    }
}
