using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public int totalSpots = 3;
    private int activatedSpots = 0;
    public GameObject rewardObject; // Ej: Una plataforma que estaba desactivada

    void Start() {
        if(rewardObject != null) rewardObject.SetActive(false); // Ocultar recompensa al inicio
    }

    public void ValidateSpot()
    {
        activatedSpots++;
        Debug.Log("Spots activados: " + activatedSpots);

        if (activatedSpots >= totalSpots)
        {
            Debug.Log("¡Puzzle Resuelto! Restauración completada.");
            // Activar la recompensa
            if(rewardObject != null) rewardObject.SetActive(true);
            // Opcional: VFX de limpieza o sonido de éxito
        }
    }
}