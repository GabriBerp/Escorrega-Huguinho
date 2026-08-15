using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("A tag configurada no Objeto do Jogador (Hugo).")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no gatilho é o jogador
        if (other.CompareTag(playerTag))
        {
            CarregarProximaFase();
        }
    }

    private void CarregarProximaFase()
    {
        // Pega o índice da cena atual
        int indiceCenaAtual = SceneManager.GetActiveScene().buildIndex;
        // Calcula o índice da próxima cena
        int indiceProximaCena = indiceCenaAtual + 1;

        // Verifica se existe uma próxima cena configurada no Build Settings
        if (indiceProximaCena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(indiceProximaCena);
        }
        else
        {
            Debug.Log("Você venceu o jogo! Não há mais fases.");
            // Opcional: Voltar para a tela de menu (Cena 0)
            // SceneManager.LoadScene(0);
        }
    }
}
