// --------------------------------------------------------------------------------------------------------
//  EasyTrace Utility
//  Version: 1.1.0
//  Created by: egyhurka
//
// NOTE:
// Debug visualizations (Debug.DrawLine, Debug.DrawRay, etc.) only appear in the Unity Editor's Scene view.
// They DO NOT appear in builds. The raycasts or logic still execute, but the visuals are editor-only.
// --------------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// EasyTrace utility class providing various raycast and debug helpers.
/// </summary>
public static class Trace
{
    public const float DEBUG_DRAW_DURATION = 1f;

    public static LayerMask DEFAULT_LAYER = LayerMask.GetMask("Default");
    public static LayerMask ALL_LAYERS = ~0;

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

    /// <summary>
    /// Casts multiple rays in a grid pattern across the camera's far frustum plane and returns all valid hits.
    /// </summary>
    public static bool FieldOfView(Camera camera, LayerMask mask, out RaycastHit[] hits, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        if (debug)
            DrawFrustum(camera);
        var frustum = FrustumCalculate(camera);

        float distance = Vector3.Distance(camera.transform.position, frustum.far.center);

        List<Vector3> points = new List<Vector3>();

        int pointsX = 30;
        int pointsY = 20;

        float stepX = frustum.far.width / (pointsX - 1);
        float stepY = frustum.far.height / (pointsY - 1);

        Vector3 right = frustum.far.right.normalized;
        Vector3 up = frustum.far.up.normalized;

        for (int i = 0; i < pointsX; i++)
        {
            for (int j = 0; j < pointsY; j++)
            {
                float offsetX = (i - (pointsX - 1) / 2f) * stepX;
                float offsetY = (j - (pointsY - 1) / 2f) * stepY;
                Vector3 point = frustum.far.center + right * offsetX + up * offsetY;
                points.Add(point);
            }
        }

        List<RaycastHit> validHits = new List<RaycastHit>();

        foreach (var point in points)
        {
            Vector3 direction = (point - camera.transform.position).normalized;

            if (Forward(camera.transform.position, direction, distance, mask, out RaycastHit hit))
            {
                if (debug)
                    DrawDebugSphere(hit.point, 0.1f, Color.green, duration);

                validHits.Add(hit);
            }
            else
            {
                if (debug)
                    DrawDebugSphere(point, 0.5f, Color.red, duration);
            }
        }

        hits = validHits.ToArray();
        return hits.Length > 0;
    }

    /// <summary>
    /// Casts multiple rays in a grid pattern across a custom distance from the camera and returns all valid hits.
    /// </summary>
    public static bool FieldOfView(Camera camera, float distance, LayerMask mask, out RaycastHit[] hits, bool debug = false, float duration = DEBUG_DRAW_DURATION)
    {
        if (debug)
            DrawFrustum(camera, distance);

        var far = FrustumPlaneCalculate(camera, distance);

        List<Vector3> points = new List<Vector3>();

        int minPointsX = 5;
        int maxPointsX = 50;
        int minPointsY = 3;
        int maxPointsY = 25;

        int pointsX = Mathf.Clamp((int)(distance * 2), minPointsX, maxPointsX);
        int pointsY = Mathf.Clamp((int)(distance), minPointsY, maxPointsY);

        float stepX = far.width / (pointsX - 1);
        float stepY = far.height / (pointsY - 1);

        Vector3 right = far.right.normalized;
        Vector3 up = far.up.normalized;

        for (int i = 0; i < pointsX; i++)
        {
            for (int j = 0; j < pointsY; j++)
            {
                float offsetX = (i - (pointsX - 1) / 2f) * stepX;
                float offsetY = (j - (pointsY - 1) / 2f) * stepY;
                Vector3 point = far.center + right * offsetX + up * offsetY;
                points.Add(point);
            }
        }

        List<RaycastHit> validHits = new List<RaycastHit>();

        foreach (var point in points)
        {
            Vector3 direction = (point - camera.transform.position).normalized;

            if (Forward(camera.transform.position, direction, distance, mask, out RaycastHit hit))
            {
                if (debug)
                    DrawDebugSphere(hit.point, 0.1f, Color.green, duration);
                validHits.Add(hit);
            }
            else
            {
                if (debug)
                { 
                    float minRadius = 0.005f;
                    float maxRadius = 0.05f;

                    float radius = Mathf.Clamp(0.01f * distance, minRadius, maxRadius);

                    DrawDebugSphere(point, radius, Color.red, duration);
                }
            }
        }

        hits = validHits.ToArray();
        return hits.Length > 0;
    }

    /// <summary>
    /// Removes duplicate hits that reference the same collider from the provided RaycastHit array.
    /// </summary>
    /// <param name="hits">
    /// The array of <see cref="RaycastHit"/> results to filter. 
    /// This parameter is passed by reference and will be replaced with a new array containing only the first hit for each unique collider.
    /// </param>
    public static void RemoveSameHits(ref RaycastHit[] hits)
    {
        HashSet<Collider> uniqueColliders = new HashSet<Collider>();
        List<RaycastHit> filteredHits = new List<RaycastHit>();
        foreach (var hit in hits)
        {
            if (uniqueColliders.Add(hit.collider))
            {
                filteredHits.Add(hit);
            }
        }
        hits = filteredHits.ToArray();
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
                DrawHitPoint(closestPoint, 0.3f, Color.cyan, duration);
            }

            DrawDebugSphere(origin, radius, color, duration);
        }

        return colliders.Length > 0;
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Can not detect anything, but can be used to draw a debug ray in the editor.
    /// </summary>
    public static void DrawDebugRay(bool raycastIsValid, float distance, Ray ray, RaycastHit hit, float duration = DEBUG_DRAW_DURATION)
    {
        if (raycastIsValid)
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.green, duration);
            DrawHitPoint(hit.point, 0.1f, Color.cyan, duration);
            Debug.DrawRay(hit.point, ray.direction * (distance - Vector3.Distance(ray.origin, hit.point)), Color.red, duration);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration);
        }
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug line between the last position and the current position for a given duration and color.
    /// </summary>
    public static void DrawMovement(Vector3 lastPosition, Vector3 position, float duration, Color color)
    {
        Debug.DrawLine(lastPosition, position, color, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug sphere at the current position for a given duration and color.
    /// </summary>
    public static void DrawMovementSphere(Vector3 position, float duration, Color color)
    {
        DrawDebugSphere(position, 0.02f, color, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug sphere at the specified position with the given radius, color, and duration.
    /// </summary>
    public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration = DEBUG_DRAW_DURATION, int segments = 20)
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
    /// <summary>
    /// Draws a cross at the specified point to indicate a hit location.
    /// </summary>
    public static void DrawHitPoint(Vector3 point, float size, Color color, float duration = DEBUG_DRAW_DURATION)
    {
        Vector3 up = Vector3.up * size;
        Vector3 right = Vector3.right * size;
        Vector3 forward = Vector3.forward * size;

        Debug.DrawLine(point - up, point + up, color, duration);
        Debug.DrawLine(point - right, point + right, color, duration);
        Debug.DrawLine(point - forward, point + forward, color, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a frustum plane at a specified distance from the camera.
    /// </summary>
    public static void DrawFovPlaneDistance(Camera camera, float distance, Color? color = null, float duration = DEBUG_DRAW_DURATION)
    {
        Color finalColor = color ?? Color.yellow;

        FrustumPlaneData planeData = FrustumPlaneCalculate(camera, distance);
        DrawFrustumPlane(planeData, finalColor, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug representation of the camera's view frustum in the Scene view.
    /// </summary>
    public static void DrawFrustum(Camera camera, Color? color = null, float duration = DEBUG_DRAW_DURATION)
    {
        Color finalColor = color ?? Color.yellow;

        var frustum = FrustumCalculate(camera);

        DrawFrustumPlane(frustum.near, finalColor, duration);
        DrawFrustumPlane(frustum.far, finalColor, duration);

        // Connect near and far planes
        Debug.DrawLine(frustum.near.topLeft, frustum.far.topLeft, finalColor, duration);
        Debug.DrawLine(frustum.near.topRight, frustum.far.topRight, finalColor, duration);
        Debug.DrawLine(frustum.near.bottomLeft, frustum.far.bottomLeft, finalColor, duration);
        Debug.DrawLine(frustum.near.bottomRight, frustum.far.bottomRight, finalColor, duration);
    }

    // ONLY WORKS IN EDITOR
    /// <summary>
    /// Draws a debug representation of the camera's view frustum at a specified distance.
    public static void DrawFrustum(Camera camera, float distance, Color? color = null, float duration = DEBUG_DRAW_DURATION)
    {
        Color finalColor = color ?? Color.yellow;

        FrustumPlaneData planeData = FrustumPlaneCalculate(camera, distance);
        DrawFrustumPlane(planeData, finalColor, duration);

        FrustumPlaneData frustum = FrustumPlaneCalculate(camera, camera.nearClipPlane);

        // Connect near and distant planes
        Debug.DrawLine(frustum.topLeft, planeData.topLeft, finalColor, duration);
        Debug.DrawLine(frustum.topRight, planeData.topRight, finalColor, duration);
        Debug.DrawLine(frustum.bottomLeft, planeData.bottomLeft, finalColor, duration);
        Debug.DrawLine(frustum.bottomRight, planeData.bottomRight, finalColor, duration);
    }

    /// <summary>
    /// Draws the outline of a frustum plane using debug lines.
    /// </summary>
    /// <param name="data">
    /// Pass a value returned by <see cref="FrustumPlaneCalculate"/> to draw a specific frustum plane (e.g., near or far),
    /// or use the result of <see cref="FrustumCalculate"/> to access both near and far planes.
    /// </param>
    public static void DrawFrustumPlane(FrustumPlaneData data, Color color, float duration = DEBUG_DRAW_DURATION)
    {
        Debug.DrawLine(data.topLeft, data.topRight, color, duration);
        Debug.DrawLine(data.bottomLeft, data.bottomRight, color, duration);
        Debug.DrawLine(data.bottomLeft, data.topLeft, color, duration);
        Debug.DrawLine(data.bottomRight, data.topRight, color, duration);

        // Cross inside the far plane
        //Debug.DrawLine(data.bottomLeft, data.topRight, color, duration);
        //Debug.DrawLine(data.topLeft, data.bottomRight, color, duration);
    }

    /// <summary>
    /// Stores geometric data for a single frustum plane (center, axes, corners, size).
    /// </summary>
    public struct FrustumPlaneData
    {
        public Vector3 center;
        public Vector3 up;
        public Vector3 right;
        public float width;
        public float height;
        public Vector3 topLeft, topRight, bottomLeft, bottomRight;
    }

    /// <summary>
    /// Calculates the frustum plane's geometry at a given distance from the camera.
    /// </summary>
    public static FrustumPlaneData FrustumPlaneCalculate(Camera camera, float planeClip)
    {
        float fov = camera.fieldOfView;
        float height = 2f * planeClip * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float width = height * camera.aspect;

        Vector3 center = camera.transform.position + camera.transform.forward * planeClip;
        Vector3 up = camera.transform.up * (height / 2f);
        Vector3 right = camera.transform.right * (width / 2f);

        Vector3 topLeft = center + up - right;
        Vector3 topRight = center + up + right;
        Vector3 bottomLeft = center - up - right;
        Vector3 bottomRight = center - up + right;

        return new FrustumPlaneData
        {
            center = center,
            up = up,
            right = right,
            width = width,
            height = height,
            topLeft = topLeft,
            topRight = topRight,
            bottomLeft = bottomLeft,
            bottomRight = bottomRight
        };
    }

    /// <summary>
    /// Calculates the near and far frustum planes for the given camera.
    /// </summary>
    public static (FrustumPlaneData near, FrustumPlaneData far) FrustumCalculate(Camera camera)
    { 
        FrustumPlaneData near = FrustumPlaneCalculate(camera, camera.nearClipPlane);
        FrustumPlaneData far = FrustumPlaneCalculate(camera, camera.farClipPlane);

        return (near, far);
    }
}
