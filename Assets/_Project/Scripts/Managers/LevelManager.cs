using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    private int currentLevelIndex = 0;

    private void Awake()
    {
        // Setup singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load level by index
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            Debug.LogError($"LevelManager: invalid level index {index}");
            return;
        }

        currentLevelIndex = index;
        SceneManager.LoadScene(levels[index].sceneName);
    }

    // Load the next level (or loop back to the first one) with delay
    public void LoadNextLevel(float delay = 1.5f)
    {
        StartCoroutine(LoadNextLevelWithDelay(delay));
    }

    private IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int nextIndex = currentLevelIndex + 1;

        if (nextIndex < levels.Count)
            LoadLevel(nextIndex);
        else
            LoadLevel(0); // 🔁 loop back to first level
    }

    // Reload the current level with delay
    public void ReloadCurrentLevel(float delay = 1.5f)
    {
        StartCoroutine(ReloadLevelWithDelay(delay));
    }

    private IEnumerator ReloadLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadLevel(currentLevelIndex);
    }

    // Get data of the current level
    public LevelData GetCurrentLevelData()
    {
        if (currentLevelIndex < 0 || currentLevelIndex >= levels.Count)
        {
            Debug.LogError("LevelManager: CurrentLevelData invalid!");
            return null;
        }

        return levels[currentLevelIndex];
    }
}
