using System.Collections;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidadeMovimento = 8f;
    [SerializeField] private float tamanhoDoBloco = 1f;
    
    [Header("Detecção de Obstáculos")]
    // Defina a Layer das suas paredes/obstáculos como "Obstacle" no Unity
    [SerializeField] private LayerMask layerObstaculos; 

    private bool estaDeslizando = false;

    void Update()
    {
        // Se já estiver deslizando, ignora novos comandos do teclado
        if (estaDeslizando) return;

        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D ou Setas
        float vertical = Input.GetAxisRaw("Vertical");     // W/S ou Setas

        // Evita movimento diagonal (foca apenas na direção mais forte)
        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 direcao = new Vector3(horizontal, 0, vertical).normalized;
            StartCoroutine(RotinaDeslizar(direcao));
        }
    }

    private IEnumerator RotinaDeslizar(Vector3 direcao)
    {
        estaDeslizando = true;

        // Loop infinito de deslizamento: só para se encontrar um obstáculo
        while (true)
        {
            // Calcula a posição da próxima casa do Grid
            Vector3 proximaPosicao = transform.position + direcao * tamanhoDoBloco;

            // Lança um raio físico (Raycast) para ver se a próxima casa tem um obstáculo
            // Usando as APIs otimizadas do Unity 6
            if (Physics.Raycast(transform.position, direcao, out RaycastHit hit, tamanhoDoBloco, layerObstaculos))
            {
                // Se bateu em algo, sai do loop e para de deslizar!
                break;
            }

            // Move suavemente o Hugo até a próxima casa
            while (Vector3.Distance(transform.position, proximaPosicao) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, proximaPosicao, velocidadeMovimento * Time.deltaTime);
                yield return null;
            }

            // Ajusta a posição final para garantir que ele fique perfeitamente no centro do bloco
            transform.position = proximaPosicao;
        }

        estaDeslizando = false;
    }
}
