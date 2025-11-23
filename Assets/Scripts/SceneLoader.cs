using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Carga una escena por nombre
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Carga la escena GameOver directamente
    public void LoadGameOver()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
