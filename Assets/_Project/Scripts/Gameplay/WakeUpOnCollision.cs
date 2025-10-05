using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WakeUpOnCollision : MonoBehaviour
{
    [SerializeField] string wakeUpTag = Tags.SpecialObject;

    void OnTriggerEnter(Collider other)
    {
        // If character collides with a SpecialObject → trigger Game Over
        if (other.CompareTag(wakeUpTag))
        {
            Debug.Log("WakeUpOnCollision: character woke up → GAME OVER");
            GameEvents.OnGameOver?.Invoke(GameOverReason.WokeUp);
        }
    }
}



