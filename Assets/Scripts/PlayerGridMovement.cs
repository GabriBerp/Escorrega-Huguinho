using System.Collections;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidadeMovimento = 8f;
    [SerializeField] private float tamanhoDoBloco = 1f;
    [SerializeField] private float multiplicadorShift = 1.5f; // O quanto ele corre (ex: 1.5x mais rápido)
    
    [Header("Detecção de Obstáculos")]
    [SerializeField] private LayerMask layerObstaculos; 

    [Header("Detecção de Pisos (Unity 6.5)")]
    [SerializeField] private LayerMask layerPisos;
    [SerializeField] private string tagPisoNormal = "PisoNormal";

    private bool estaDeslizando = false;

    void Update()
    {
        // Se já estiver em movimento/deslizando, não aceita novos comandos
        if (estaDeslizando) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Evita diagonal (foca no eixo dominante)
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

        while (true)
        {
            // 1. Calcula a próxima posição no grid
            Vector3 proximaPosicao = transform.position + direcao * tamanhoDoBloco;

            // 2. Verifica se há parede/obstáculo no caminho
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, direcao, out RaycastHit hitObstaculo, tamanhoDoBloco, layerObstaculos))
            {
                // Se houver parede, interrompe o movimento imediatamente
                break;
            }

            // --- NOVO: LÓGICA DE VELOCIDADE COM SHIFT ---
            float velocidadeAtualDoPasso = velocidadeMovimento;

            // Detecta se o Hugo está pisando no "Piso Normal" no início deste passo
            Vector3 origemRaioPisoAtual = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(origemRaioPisoAtual, Vector3.down, out RaycastHit hitPisoAtual, 1.0f, layerPisos))
            {
                // Se estiver no piso normal E segurando a tecla Shift (Esquerdo ou Direito)
                if (hitPisoAtual.collider.CompareTag(tagPisoNormal) && 
                    (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    velocidadeAtualDoPasso *= multiplicadorShift; // Aumenta a velocidade deste passo
                }
            }
            // ---------------------------------------------

            // 3. Move o Hugo até a próxima casa do grid de forma suave usando a velocidade calculada
            while (Vector3.Distance(transform.position, proximaPosicao) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, proximaPosicao, velocidadeAtualDoPasso * Time.deltaTime);
                yield return null;
            }

            // Garante o alinhamento perfeito no centro do bloco
            transform.position = proximaPosicao;

            // 4. Checa o piso abaixo do jogador para decidir se deve continuar deslizando
            Vector3 origemRaioProximoPiso = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(origemRaioProximoPiso, Vector3.down, out RaycastHit hitPiso, 1.0f, layerPisos))
            {
                // Se o piso alcançado for o normal, ele para de deslizar
                if (hitPiso.collider.CompareTag(tagPisoNormal))
                {
                    break; 
                }
            }
        }

        estaDeslizando = false;
    }
}
