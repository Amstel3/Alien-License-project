using UnityEngine;
using UnityEngine.SceneManagement;

public static class BootstrapRedirector
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnsureBootstrap()
    {
#if UNITY_EDITOR
        // If the game is started from a Level scene instead of Bootstrap,
        // automatically redirect to Bootstrap (Editor only)
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.name != "Bootstrap")
        {
            Debug.Log($"[BootstrapRedirector] Editor start from {activeScene.name} → redirecting to Bootstrap");
            SceneManager.LoadScene("Bootstrap");
        }
#endif
    }
}

