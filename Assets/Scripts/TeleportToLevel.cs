using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToLevel : MonoBehaviour
{
    [Header("Configuración")]
    public string sceneName = "Nivel2"; // nombre de la escena a cargar
    public string playerTag = "Player"; // tag del jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"🚪 Teletransportando al jugador a la escena: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }
}
