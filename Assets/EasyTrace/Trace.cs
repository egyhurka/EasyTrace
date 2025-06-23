// --------------------------------------------------------------------------------------------------------
//  EasyTrace Utility
//  Version: 1.0.0
//  Created by: egyhurka
//
// NOTE:
// Debug visualizations (Debug.DrawLine, Debug.DrawRay, etc.) only appear in the Unity Editor's Scene view.
// They DO NOT appear in builds. The raycasts or logic still execute, but the visuals are editor-only.
// --------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// EasyTrace utility class providing various raycast and debug helpers.
/// </summary>
public static class Trace
{
    public const float DEBUG_DRAW_DURATION = 1f;

    public static LayerMask DEFAULT_LAYER = LayerMask.GetMask("Default");

    /// <summary>
    /// Casts a ray forward from the given transform up to the specified distance, checking for collisions against the specified layer mask.
    /// If debug is enabled, draws the ray, hit point, and remaining ray after the hit.
    /// </summary>
    /// <returns>True if the ray hit an object; otherwise, false.</returns>
    public static bool Forward(Transform origin, float distance, LayerMask mask, out RaycastHit hit, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        Ray ray = new Ray(origin.position, origin.forward);
        bool valid = Physics.Raycast(ray, out hit, distance, mask);
        if (debug)
        {
            DrawDebugRay(valid, distance, ray, hit, duration);
        }

        return valid;
    }

    /// <summary>
    /// Casts a ray forward from the given transform up to the specified distance, checking for collisions against the specified layer mask.
    /// If debug is enabled, draws the ray, hit point, and remaining ray after the hit.
    /// </summary>
    /// <returns>True if the ray hit an object; otherwise, false.</returns>
    public static bool Forward(Vector3 origin, Vector3 forward, float distance, LayerMask mask, out RaycastHit hit, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        Ray ray = new Ray(origin, forward);
        bool valid = Physics.Raycast(ray, out hit, distance, mask);
        if (debug)
        {
            DrawDebugRay(valid, distance, ray, hit, duration);
        }

        return valid;
    }

    /// <summary>
    /// Performs a raycast from a screen point into the world using the given camera.
    /// </summary>
    /// <param name="screenPoint">The screen position (e.g. Input.mousePosition) to cast from.</param>
    /// <returns>True if something was hit; otherwise, false.</returns>
    public static bool FromScreenPoint(Camera camera, Vector3 screenPoint, float distance, LayerMask mask, out RaycastHit hit, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        bool valid = Physics.Raycast(ray, out hit, distance, mask);

        if (debug)
        {
            DrawDebugRay(valid, distance, ray, hit, duration);
        }

        return valid;
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Can not detect anything, but can be used to draw a debug ray in the editor.
    /// </summary>
    public static void DrawDebugRay(bool valid, float distance, Ray ray, RaycastHit hit, float duration = DEBUG_DRAW_DURATION)
    {
        if (valid)
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, duration);
            DrawHitPoint(hit.point, 0.1f, Color.cyan);
            Debug.DrawRay(hit.point, ray.direction * (distance - Vector3.Distance(ray.origin, hit.point)), Color.red, duration);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration);
        }
    }

    /// <summary>
    /// Performs an overlap sphere check at the given origin and radius, returns all colliders inside the sphere.
    /// If debug is true, draws a debug sphere and lines from the origin to each collider's closest point.
    /// </summary>
    /// <returns>True if any colliders are found within the sphere; otherwise, false.</returns>
    public static bool Sphere(Vector3 origin, float radius, LayerMask mask, out Collider[] colliders, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        colliders = Physics.OverlapSphere(origin, radius, mask);

        if (debug)
        {
            Color color = colliders.Length > 0 ? Color.green : Color.red;

            foreach (var collider in colliders)
            {

                Vector3 closestPoint = collider.ClosestPoint(origin);
                Debug.DrawLine(origin, closestPoint, color, duration);
                DrawHitPoint(closestPoint, 0.3f, Color.cyan);
            }

            DrawDebugSphere(origin, radius, color, duration);
        }

        return colliders.Length > 0;
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug sphere at the current position for a given duration and color.
    /// </summary>
    public static void MovementSphere(Vector3 position, float duration, Color color)
    {
        DrawDebugSphere(position, 0.02f, color, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug line between the last position and the current position for a given duration and color.
    /// </summary>
    public static void Movement(Vector3 lastPosition, Vector3 position, float duration, Color color)
    {
        Debug.DrawLine(lastPosition, position, color, duration);
    }

    // ONLY WORKS IN EDITOR
    public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration, int segments = 20)
    {
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

    // ONLY WORKS IN EDITOR
    public static void DrawHitPoint(Vector3 point, float size, Color color, float duration = DEBUG_DRAW_DURATION)
    {
        Vector3 up = Vector3.up * size;
        Vector3 right = Vector3.right * size;
        Vector3 forward = Vector3.forward * size;

        Debug.DrawLine(point - up, point + up, color, duration);
        Debug.DrawLine(point - right, point + right, color, duration);
        Debug.DrawLine(point - forward, point + forward, color, duration);
    }
}
