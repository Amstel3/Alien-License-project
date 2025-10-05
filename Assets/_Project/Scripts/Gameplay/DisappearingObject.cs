using UnityEngine;

public class DisappearingObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.LightZone))
        {
            // Count the move before destroying objects
            GameManager.Instance?.RegisterMove();

            Destroy(gameObject);       // destroy this object
            Destroy(other.gameObject); // destroy the light zone
        }
    }
}

