using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int playerLives = 3;
    public int fragmentsCollected = 0;
    public int totalFragments = 3;

    [Header("UI References")]
    public Text livesText;
    public Text fragmentsText;
    public GameObject gameOverPanel;

    private Vector3 checkpointPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        checkpointPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        UpdateUI();
    }

    public void CollectFragment()
    {
        fragmentsCollected++;
        UpdateUI();

        if (fragmentsCollected >= totalFragments)
        {
            Debug.Log("¡Todos los fragmentos recolectados!");
            // Aquí activar final del nivel
        }
    }

    public void PlayerDeath()
    {
        playerLives--;
        UpdateUI();

        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        Debug.Log("Checkpoint activado en: " + position);
    }

    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = checkpointPosition;
        
        // Reset player state
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar juego
    }

    private void UpdateUI()
    {
        if (livesText != null) livesText.text = $"Vidas: {playerLives}";
        if (fragmentsText != null) fragmentsText.text = $"Fragmentos: {fragmentsCollected}/{totalFragments}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}   