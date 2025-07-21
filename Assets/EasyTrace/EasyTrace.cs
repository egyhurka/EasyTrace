using UnityEngine;

public class EasyTrace : MonoBehaviour
{
    public float distance = 5f;
    public Camera camera;

    private LayerMask layerMask;
    private Vector3 lastPosition;

    private void Start()
    {
        layerMask = Trace.DEFAULT_LAYER;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit[] hits;
            Trace.FieldOfView(camera, 20f, layerMask, out hits, true);
            Trace.RemoveSameHits(ref hits);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Trace.Forward(transform, distance + 5, layerMask, out RaycastHit hit, true))
            {
                Trace.Sphere(hit.point, 3f, layerMask, out Collider[] colliders, true);
                foreach (Collider collider in colliders)
                {
                    Debug.Log($"Hit: {collider.name} at {collider.ClosestPoint(hit.point)}");
                }
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

        Trace.DrawMovement(lastPosition, transform.position, 1f, Color.red);
        lastPosition = transform.position;
    }
}