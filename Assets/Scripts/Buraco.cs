using UnityEngine;
using UnityEngine.SceneManagement;

public class Buraco : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Certifique-se de que o Prefab do Hugo tenha a Tag "Player" configurada na Unity
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hugo caiu no buraco! Reiniciando a fase...");
            ReiniciarFase();
        }
    }

    private void ReiniciarFase()
    {
        // Recarrega a cena atual usando o gerenciador de cenas padrão
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
