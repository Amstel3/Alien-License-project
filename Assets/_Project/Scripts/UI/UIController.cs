using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text gameOverText;   // standard Text (for Game Over)
    [SerializeField] private Text winText;        // standard Text (for Win)
    [SerializeField] private TMP_Text movesText;  // TMP_Text (for move counter)

    void OnEnable()
    {
        // Subscribe to game events
        GameEvents.OnGameOver += ShowGameOver;
        GameEvents.OnWin += ShowWin;
        GameEvents.OnMovesChanged += UpdateMoves;
    }

    void OnDisable()
    {
        // Unsubscribe from game events
        GameEvents.OnGameOver -= ShowGameOver;
        GameEvents.OnWin -= ShowWin;
        GameEvents.OnMovesChanged -= UpdateMoves;
    }

    void Start()
    {
        // Hide UI elements at start
        if (gameOverText) gameOverText.gameObject.SetActive(false);
        if (winText) winText.gameObject.SetActive(false);
    }

    private void ShowGameOver(GameOverReason reason)
    {
        if (gameOverText)
        {
            gameOverText.gameObject.SetActive(true);

            switch (reason)
            {
                case GameOverReason.OutOfMoves:
                    gameOverText.text = "No moves left!";
                    break;
                case GameOverReason.WokeUp:
                    gameOverText.text = "Woke up!";
                    break;
            }
        }
    }

    private void ShowWin()
    {
        if (winText) winText.gameObject.SetActive(true);
    }

    private void UpdateMoves(int current, int max)
    {
        if (movesText != null)
            movesText.text = $"Moves: {current}/{max}";
    }
}
