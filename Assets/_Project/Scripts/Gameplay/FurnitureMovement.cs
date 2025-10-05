using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FurnitureMovement : MonoBehaviour
{
    [Header("Room Bounds")]
    public float minX, maxX, minZ, maxZ;

    private Vector3 screenPoint;
    private Vector3 offset;
    private float y0;

    private enum Axis { None, X, Z }
    private Axis moveAxis = Axis.None;

    [Header("Anti-overlap")]
    [Range(1f, 1.2f)]
    public float overlapExpand = 1.05f;

    [Header("Sensitivity")]
    [SerializeField] private float axisThreshold = 0.05f;
    // Lower value → axis is chosen faster

    [Header("Special Rules")]
    [SerializeField] private bool canTouchSpecial = false;
    // true → this object CAN overlap SpecialObject (e.g. bed with character)
    // false → this object CANNOT overlap SpecialObject (default furniture)

    private Vector3 startDragPos;

    void Start()
    {
        y0 = transform.position.y;
    }

    void OnMouseDown()
    {
        // Block input if game already ended
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            Debug.Log("FurnitureMovement: game ended → movement blocked");
            return;
        }

        if (!Camera.main) return;

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        moveAxis = Axis.None;
        startDragPos = transform.position;
    }

    void OnMouseDrag()
    {
        // Block input if game already ended
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            Debug.Log("FurnitureMovement: game ended → movement blocked");
            return;
        }

        if (!Camera.main) return;

        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

        // --- axis selection ---
        if (moveAxis == Axis.None)
        {
            Vector3 delta = cursorPos - transform.position;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z) && Mathf.Abs(delta.x) > axisThreshold)
                moveAxis = Axis.X;
            else if (Mathf.Abs(delta.z) > Mathf.Abs(delta.x) && Mathf.Abs(delta.z) > axisThreshold)
                moveAxis = Axis.Z;
            else
                return; // too small movement
        }

        // --- target position ---
        Vector3 target = transform.position;

        if (moveAxis == Axis.X)
        {
            float newX = Mathf.Clamp(cursorPos.x, minX, maxX);
            target = new Vector3(newX, y0, transform.position.z);
        }
        else if (moveAxis == Axis.Z)
        {
            float newZ = Mathf.Clamp(cursorPos.z, minZ, maxZ);
            target = new Vector3(transform.position.x, y0, newZ);
        }

        // --- collision check ---
        Collider col = GetComponent<Collider>();
        Bounds b = col.bounds;
        b.center = target;
        Vector3 half = b.extents * overlapExpand;

        Collider[] hits = Physics.OverlapBox(
            b.center,
            half,
            Quaternion.identity,
            ~0,
            QueryTriggerInteraction.Collide);

        bool overlaps = false;

        foreach (var h in hits)
        {
            if (h == col) continue;

            // Always block collision with furniture and disappearing objects
            if (h.CompareTag(Tags.Furniture) || h.CompareTag("DisappearingObject"))
            {
                overlaps = true;
                break;
            }

            // Block SpecialObject only if this item is not allowed to touch it
            if (!canTouchSpecial && h.CompareTag(Tags.SpecialObject))
            {
                overlaps = true;
                break;
            }
        }

        if (!overlaps)
            transform.position = target;
    }

    void OnMouseUp()
    {
        moveAxis = Axis.None;

        // Register a move if object was actually moved
        if ((transform.position - startDragPos).sqrMagnitude > 0.001f)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsGameEnded)
            {
                GameManager.Instance.RegisterMove();
                Debug.Log("FurnitureMovement: move registered");
            }
        }
    }
}
