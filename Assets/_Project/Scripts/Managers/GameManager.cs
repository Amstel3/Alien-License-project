using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int maxMoves;
    private int currentMoves = 0;
    private bool isGameEnded = false;
    public bool IsGameEnded => isGameEnded;

    void Awake()
    {
        // Setup singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager: Initialized");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        // Subscribe to events
        GameEvents.OnWin += Win;
        GameEvents.OnGameOver += GameOver;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from events
        GameEvents.OnWin -= Win;
        GameEvents.OnGameOver -= GameOver;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Bootstrap") return;

        Debug.Log($"GameManager: Scene loaded → {scene.name}");

        isGameEnded = false;
        currentMoves = 0;

        // Get level settings from LevelManager
        if (LevelManager.Instance != null)
        {
            var data = LevelManager.Instance.GetCurrentLevelData();
            maxMoves = (data != null) ? data.maxMoves : 5;
        }
        else
        {
            Debug.LogWarning("GameManager: LevelManager not found → using default maxMoves = 5");
            maxMoves = 5;
        }

        // Update UI with current moves
        GameEvents.OnMovesChanged?.Invoke(currentMoves, maxMoves);
    }

    public void RegisterMove()
    {
        if (isGameEnded)
        {
            Debug.Log("GameManager: Move ignored — game has ended");
            return;
        }

        currentMoves++;
        Debug.Log($"GameManager: Move registered → {currentMoves}/{maxMoves}");

        // Notify UI
        GameEvents.OnMovesChanged?.Invoke(currentMoves, maxMoves);

        if (currentMoves >= maxMoves)
        {
            GameEvents.OnGameOver?.Invoke(GameOverReason.OutOfMoves);
        }
    }

    private void GameOver(GameOverReason reason)
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log($"GameManager: Game over → reason: {reason}");

        // ⏳ Reload current level with delay
        LevelManager.Instance?.ReloadCurrentLevel(2f);
    }

    private void Win()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("GameManager: Victory!");

        // ⏳ Load next level with delay
        LevelManager.Instance?.LoadNextLevel(2f);
    }
}
