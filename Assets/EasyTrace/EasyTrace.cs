using UnityEngine;

public class EasyTrace : MonoBehaviour
{
    public float distance = 5f;
    public LayerMask layerMask;

    private Vector3 lastPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Trace.Forward(transform, distance, layerMask, out RaycastHit hit, true))
            {
                Trace.Sphere(hit.point, 5f, layerMask, out Collider[] hits, true);
                foreach (var item in hits)
                {
                    Debug.Log("Hit:" + item.name);
                }
            }
        }

        Trace.Movement(lastPosition, transform.position, 1f, Color.red);
        lastPosition = transform.position;
    }
}