using UnityEngine;

public static class Trace
{
    public const float DEBUG_DRAW_DURATION = 1f;

    /// <summary>
    /// Casts a ray forward from the given transform up to the specified distance, checking for collisions against the specified layer mask.
    /// If debug is enabled, draws the ray, hit point, and remaining ray after the hit.
    /// </summary>
    /// <returns>True if the ray hit an object; otherwise, false.</returns>
    public static bool Forward(Transform origin, float distance, LayerMask mask, out RaycastHit hit, bool debug = false)
    { 
        Ray ray = new Ray(origin.position, origin.forward);
        bool valid = Physics.Raycast(ray, out hit, distance, mask);
        if (debug)
        {
            if (valid)
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, DEBUG_DRAW_DURATION);
                DrawHitPoint(hit.point, 0.1f, Color.cyan);
                Debug.DrawRay(hit.point, ray.direction * (distance - Vector3.Distance(ray.origin, hit.point)), Color.red, DEBUG_DRAW_DURATION);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, DEBUG_DRAW_DURATION);
            }
        }
        return valid;
    }

    /// <summary>
    /// Performs an overlap sphere check at the given origin and radius, returns all colliders inside the sphere.
    /// If debug is true, draws a debug sphere and lines from the origin to each collider's closest point.
    /// </summary>
    /// <returns>True if any colliders are found within the sphere; otherwise, false.</returns>
    public static bool Sphere(Vector3 origin, float radius, LayerMask mask, out Collider[] colliders, bool debug = false)
    {
        colliders = Physics.OverlapSphere(origin, radius, mask);

        if (debug)
        {
            Color color = colliders.Length > 0 ? Color.green : Color.red;

            foreach (var collider in colliders)
            {

                Vector3 closestPoint = collider.ClosestPoint(origin);
                Debug.DrawLine(origin, closestPoint, color, DEBUG_DRAW_DURATION);
                DrawHitPoint(closestPoint, 0.3f, Color.cyan);
            }

            DrawDebugSphere(origin, radius, color, DEBUG_DRAW_DURATION);
        }

        return colliders.Length > 0;
    }

    /// <summary>
    /// Draws a debug sphere at the current position for a given duration and color.
    /// </summary>
    public static void MovementSphere(Vector3 position, float duration, Color color)
    { 
        DrawDebugSphere(position, 0.02f, color, duration);
    }

    /// <summary>
    /// Draws a debug line between the last position and the current position for a given duration and color.
    /// </summary>
    public static void Movement(Vector3 lastPosition, Vector3 position, float duration, Color color)
    {
        Debug.DrawLine(lastPosition, position, color, duration);
    }

    public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration)
    {
        int segments = 20;
        float step = Mathf.PI * 2 / segments;

        for (int i = 0; i < segments; i++)
        {
            float theta1 = i * step;
            float theta2 = (i + 1) * step;

            // X-Y
            Debug.DrawLine(
                position + new Vector3(Mathf.Cos(theta1), Mathf.Sin(theta1), 0) * radius,
                position + new Vector3(Mathf.Cos(theta2), Mathf.Sin(theta2), 0) * radius,
                color, duration
            );

            // X-Z
            Debug.DrawLine(
                position + new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1)) * radius,
                position + new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2)) * radius,
                color, duration
            );

            // Y-Z
            Debug.DrawLine(
                position + new Vector3(0, Mathf.Cos(theta1), Mathf.Sin(theta1)) * radius,
                position + new Vector3(0, Mathf.Cos(theta2), Mathf.Sin(theta2)) * radius,
                color, duration
            );
        }
    }

    private static void DrawHitPoint(Vector3 point, float size, Color color)
    {
        Vector3 up = Vector3.up * size;
        Vector3 right = Vector3.right * size;
        Vector3 forward = Vector3.forward * size;

        Debug.DrawLine(point - up, point + up, color, DEBUG_DRAW_DURATION);
        Debug.DrawLine(point - right, point + right, color, DEBUG_DRAW_DURATION);
        Debug.DrawLine(point - forward, point + forward, color, DEBUG_DRAW_DURATION);
    }
}
