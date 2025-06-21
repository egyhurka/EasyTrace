using UnityEngine;

public class EasyTrace : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask layerMask;

    public Camera camera;

    private Vector3 lastPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Trace.Forward(transform, distance, layerMask, out RaycastHit hit, true))
            {
                Debug.Log($"Hit: {hit.collider.name} at {hit.point}");
            }
        }

        // If left click pressed and right click released — shoot (only when not flying)
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            if (Trace.FromScreenPoint(camera, Input.mousePosition, distance * 2, layerMask, out RaycastHit hit, true))
            {
                // ...
            }
        }

        Trace.Movement(lastPosition, transform.position, 1f, Color.red);
        lastPosition = transform.position;
    }
}