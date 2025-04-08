using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame() {
        #if UNITY_EDITOR            // When the application is running in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else                       // when application is built
            Application.Quit();
        #endif
    }
}
