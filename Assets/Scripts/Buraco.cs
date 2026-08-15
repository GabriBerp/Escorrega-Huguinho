using UnityEngine;
using UnityEngine.SceneManagement;

public class Buraco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou no buraco foi o jogador
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hugo caiu no buraco! Reiniciando a fase...");
            
            // Opcional: Desativa o visual do jogador para simular que ele caiu
            other.gameObject.SetActive(false); 
            
            ReiniciarFase();
        }
    }

    private void ReiniciarFase()
    {
        // Recarrega a cena ativa atual de forma limpa
        int cenaAtual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(cenaAtual);
    }
}
