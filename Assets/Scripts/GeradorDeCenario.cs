using UnityEngine;

public class GeradorDeCenario : MonoBehaviour
{
    [Header("Imagem do Mapa")]
    [Tooltip("A imagem que define o cenário. Ative 'Read/Write' nas configurações dela!")]
    [SerializeField] private Texture2D mapaTextura;

    [Header("Prefabs dos Elementos")]
    [SerializeField] private GameObject prefabGelo;
    [SerializeField] private GameObject prefabParede;
    [SerializeField] private GameObject prefabHugo;
    [SerializeField] private GameObject prefabObjetivo;
    [SerializeField] private GameObject prefabBuraco;
    [SerializeField] private GameObject prefabPisoCaverna;

    [Header("Configurações da Grade")]
    [SerializeField] private float tamanhoDoBloco = 1.0f;

    private void Start()
    {
        GerarFase();
    }

    public void GerarFase()
    {
        if (mapaTextura == null)
        {
            Debug.LogError("Por favor, atribua uma textura de mapa no Inspector!");
            return;
        }

        LimparCenario();

        int largura = mapaTextura.width;
        int altura = mapaTextura.height;

        for (int x = 0; x < largura; x++)
        {
            for (int z = 0; z < altura; z++)
            {
                Color corDoPixel = mapaTextura.GetPixel(x, z);
                Vector3 posicao = new Vector3(x * tamanhoDoBloco, 0, z * tamanhoDoBloco);

                // GERAÇÃO DO CHÃO BASE:
                // Se for Buraco (Preto) ou Piso Caverna (Verde), NÃO criamos gelo por baixo
                if (!CompararCores(corDoPixel, Color.black) && !CompararCores(corDoPixel, Color.green))
                {
                    InstanciarObjeto(prefabGelo, posicao, transform);
                }

                // Decide o que colocar por cima ou como piso principal
                VerificarECriarElemento(corDoPixel, posicao);
            }
        }
    }

    private void VerificarECriarElemento(Color cor, Vector3 posicao)
    {
        // BRANCO -> Parede
        if (CompararCores(cor, Color.white))
        {
            InstanciarObjeto(prefabParede, posicao + Vector3.up * 0.5f, transform);
        }
        // VERMELHO -> Jogador (Hugo)
        else if (CompararCores(cor, Color.red))
        {
            InstanciarObjeto(prefabHugo, posicao + Vector3.up * 0.5f, null);
        }
        // AMARELO -> Objetivo
        else if (CompararCores(cor, Color.yellow))
        {
            InstanciarObjeto(prefabObjetivo, posicao, transform);
        }
        // PRETO -> Buraco
        else if (CompararCores(cor, Color.black))
        {
            InstanciarObjeto(prefabBuraco, posicao, transform);
        }
        // VERDE -> Novo Piso de Caverna (Normal)
        else if (CompararCores(cor, Color.green))
        {
            InstanciarObjeto(prefabPisoCaverna, posicao, transform);
        }
        // CIANO -> Apenas Gelo (Já tratado no chão base)
    }

    // Compara cores com margem de tolerância (evita bugs de cores quase idênticas)
    private bool CompararCores(Color c1, Color c2)
    {
        float limite = 0.1f;
        return Mathf.Abs(c1.r - c2.r) < limite &&
               Mathf.Abs(c1.g - c2.g) < limite &&
               Mathf.Abs(c1.b - c2.b) < limite;
    }

    private void InstanciarObjeto(GameObject prefab, Vector3 posicao, Transform pai)
    {
        if (prefab != null)
        {
            Instantiate(prefab, posicao, Quaternion.identity, pai);
        }
    }

    private void LimparCenario()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
