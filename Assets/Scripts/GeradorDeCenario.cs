using UnityEngine;
using System;

public class GeradorDeCenario : MonoBehaviour
{
    [Header("Prefabs dos Elementos")]
    [SerializeField] private GameObject prefabGelo;
    [SerializeField] private GameObject prefabParede;
    [SerializeField] private GameObject prefabHugo;
    [SerializeField] private GameObject prefabObjetivo;
    [SerializeField] private GameObject prefabBuraco; // Novo Prefab adicionado!

    [Header("Configurações da Grade")]
    [SerializeField] private float tamanhoDoBloco = 1.0f;

    [Header("Desenho da Fase (Matriz)")]
    [TextArea(10, 20)]
    [SerializeField] private string layoutDaFase = 
        "#######\n" +
        "#S...G#\n" +
        "#.###.#\n" +
        "#..O..#\n" +
        "#######";

    private void Start()
    {
        GerarFase();
    }

    public void GerarFase()
    {
        LimparCenario();

        string[] linhas = layoutDaFase.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int altura = linhas.Length;

        for (int z = 0; z < altura; z++)
        {
            string linhaAtual = linesAdaptadas(linhas[z]);
            int largura = linhaAtual.Length;

            for (int x = 0; x < largura; x++)
            {
                char caractere = linhaAtual[x];
                
                Vector3 posicao = new Vector3(x * tamanhoDoBloco, 0, (altura - 1 - z) * tamanhoDoBloco);

                // Só gera o chão de gelo se NÃO for um buraco ('O')
                if (caractere != 'O')
                {
                    InstanciarObjeto(prefabGelo, posicao, transform);
                }

                // Instancia o objeto correspondente ao caractere
                switch (caractere)
                {
                    case '#': // Parede
                        InstanciarObjeto(prefabParede, posicao + Vector3.up * 0.5f, transform);
                        break;

                    case 'S': // Hugo (Start)
                        InstanciarObjeto(prefabHugo, posicao + Vector3.up * 0.5f, null);
                        break;

                    case 'G': // Objetivo (Goal)
                        InstanciarObjeto(prefabObjetivo, posicao, transform);
                        break;

                    case 'O': // Buraco (Obstacle/Hazard)
                        InstanciarObjeto(prefabBuraco, posicao, transform);
                        break;

                    case '.': // Gelo vazio (já tratado no IF acima)
                    default:
                        break;
                }
            }
        }
    }

    private string linesAdaptadas(string linha)
    {
        // Remove espaços extras que possam quebrar a matriz no inspetor
        return linha.Trim();
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
