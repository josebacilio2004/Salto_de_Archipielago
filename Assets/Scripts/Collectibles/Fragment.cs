using UnityEngine;

public class Fragment : MonoBehaviour
{
    public int fragmentValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectFragment();
            AudioManager.Instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}
