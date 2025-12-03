using UnityEngine;

public class PuzzleSpot : MonoBehaviour
{
    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;
    // Arrastra el PuzzleManager de la escena aquí en el inspector
    public PuzzleManager puzzleManagerRef;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            ActivateSpot();
        }
    }

    void ActivateSpot()
    {
        isActivated = true;
        // Cambiar color a verde brillante para indicar "limpio"
        spriteRenderer.color = Color.green;
        // Avisar al manager
        puzzleManagerRef.ValidateSpot();
        // Opcional: SFX de planta creciendo
    }
}