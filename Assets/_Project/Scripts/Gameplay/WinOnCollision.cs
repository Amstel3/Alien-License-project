using UnityEngine;

public class WinOnCollision : MonoBehaviour
{
    [SerializeField] string winTag = Tags.LightBeam; // tag of the win zone

    private void OnTriggerEnter(Collider other)
    {
        // If character enters the win zone → trigger Win event
        if (other.CompareTag(winTag))
        {
            Debug.Log("WinOnCollision: character touched LightBeam → WIN");
            GameEvents.OnWin?.Invoke();
        }
    }
}
