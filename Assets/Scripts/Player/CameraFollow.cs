using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Aquí pondremos al Player
    public float smoothSpeed = 0.125f; // Qué tan suave sigue la cámara
    public Vector3 offset = new Vector3(0, 0, -10); // Distancia (Z debe ser -10 para 2D)

    void LateUpdate() // Usamos LateUpdate para evitar temblores
    {
        if (target != null)
        {
            // Calculamos dónde queremos ir
            Vector3 desiredPosition = target.position + offset;
            
            // Suavizamos el movimiento (Lerp)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Opcional: Bloquear la cámara para que no baje demasiado (si cae al vacío)
            // if (smoothedPosition.y < 0) smoothedPosition.y = 0; 

            transform.position = smoothedPosition;
        }
    }
}