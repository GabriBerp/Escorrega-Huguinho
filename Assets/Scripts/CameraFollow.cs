using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Alvo para Seguir")]
    [SerializeField] private Transform target;

    [Header("Configurações de Posição")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -8); // Distância da câmera em relação ao jogador

    [Header("Suavização")]
    [SerializeField] private float smoothTime = 0.3f; // Tempo de resposta da câmera (menor = mais rápida)

    private Vector3 currentVelocity = Vector3.zero;

    // LateUpdate roda após todos os movimentos do Update/FixedUpdate do jogador serem processados
    private void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            target = player.transform;
        }
        
        // 1. Calcula onde a câmera DEVE estar
        Vector3 targetPosition = target.position + offset;

        // 2. Move a câmera suavemente da posição atual para a posição desejada
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
