using UnityEngine;

public class SmoothCamera2DFollow : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float verticalOffset = 2f; // Extra space above player
    
    [Header("Advanced Settings")]
    [SerializeField] private bool enableXAxis = true;
    [SerializeField] private bool enableYAxis = true;
    [SerializeField] private float yAxisDeadzone = 1f; // Deadzone for vertical movement
    
    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate target position with offset
        Vector3 targetPosition = target.position + offset;
        
        // Apply vertical offset (shows more above player)
        targetPosition.y += verticalOffset;
        
        // Apply axis constraints
        if (!enableXAxis) targetPosition.x = transform.position.x;
        if (!enableYAxis) targetPosition.y = transform.position.y;
        
        // Apply deadzone for vertical movement
        if (Mathf.Abs(targetPosition.y - transform.position.y) < yAxisDeadzone)
        {
            targetPosition.y = transform.position.y;
        }

        // Smooth damp to target position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }

    // Call this when player dies/respawns
    public void SnapToTarget()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            velocity = Vector3.zero;
        }
    }
}