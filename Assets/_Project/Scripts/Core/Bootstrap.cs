using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        // Ensure LevelManager exists and persists across scenes
        if (LevelManager.Instance == null)
        {
            gameObject.AddComponent<LevelManager>();
            DontDestroyOnLoad(LevelManager.Instance);
        }

        // Ensure GameManager exists and persists across scenes
        if (GameManager.Instance == null)
        {
            var gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
            DontDestroyOnLoad(gm);
        }

        // Immediately load the first level (before Bootstrap is rendered)
        LevelManager.Instance.LoadLevel(0);
    }
}

