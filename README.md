# 🎯 EasyTrace - Raycast Debug Utility for Unity

**Version:** 1.0.0  
**Author:** [egyhurka](https://github.com/egyhurka)  
**License:** MIT

## Overview

**EasyTrace** is a lightweight and extensible utility for working with raycasts, debug drawing, and collision helpers in Unity.

> All visual debug helpers (like `Debug.DrawLine`, `DrawRay`, `DrawSphere`) **only appear in the Unity Editor Scene View**. They are invisible in builds, but the logic still executes.

---

## ✅ Features

- 🔍 Simple `Raycast`, `SphereCast`, and screen-to-world tracing
- 🧠 Debug visualization helpers (lines, spheres, points)
- 🧱 Static utility class - no setup needed
- 🖱️ Includes example MonoBehaviour (`EasyTrace.cs`) for usage
- 🎨 Debug options are optional and fully customizable

---

## 📦 Installation

Just copy `Trace.cs` and (optionally) `EasyTrace.cs` into your Unity project.

---

## 🛠️ Included Files

| File          | Description                                  |
|---------------|----------------------------------------------|
| `Trace.cs`    | Static class for all tracing + debug         |
| `EasyTrace.cs`| Example MonoBehaviour using `Trace` tools    |

---

## 📄 License

This project is licensed under the **MIT License** – feel free to use, modify, and distribute.

## 📘 Usage

### 1. Raycast Forward

```csharp
if (Trace.Forward(transform, 10f, Trace.DEFAULT_LAYER, out RaycastHit hit, true))
{
    Debug.Log(hit.collider.name);
}
```

### 2. Raycast From Screen Point

```csharp
if (Trace.FromScreenPoint(Camera.main, Input.mousePosition, 100f, Trace.DEFAULT_LAYER, out RaycastHit hit, true))
{
    // Use hit.point, hit.normal, etc.
}
```

### 3. Raycast To a Specified FOV Plane & Remove Same Hits Function
#### If you do not specify a distance, the rays are cast to the camera’s far clip plane by default.
```csharp
// WARNING: You must **remove** the RaycastHit[] declaration here!
RaycastHit[] hits;
// The 'hits' array will be assigned by the FieldOfView method via the 'out' parameter.
Trace.FieldOfView(camera, 20f, layerMask, out hits, true);
Trace.RemoveSameHits(ref hits);
```

### 3. Overlap Sphere

```csharp
if (Trace.Sphere(transform.position, 3f, Trace.DEFAULT_LAYER, out Collider[] hits, true))
{
    foreach (var hit in hits)
        Debug.Log("Nearby: " + hit.name);
}
```
## Draw Functions (Editor-only)

```csharp
public static void DrawDebugRay(bool valid, float distance, Ray ray, RaycastHit hit, float duration = DEBUG_DRAW_DURATION)
```

```csharp
public static void DrawMovement(Vector3 lastPosition, Vector3 position, float duration, Color color)
```

```csharp
public static void DrawMovementSphere(Vector3 position, float duration, Color color)
```

```csharp
public static void DrawDebugSphere(Vector3 position, float radius, Color color, float duration = DEBUG_DRAW_DURATION, int segments = 20)
```

```csharp
public static void DrawHitPoint(Vector3 point, float size, Color color, float duration = DEBUG_DRAW_DURATION)
```

```csharp
public static void DrawFrustum(Camera camera, Color color, float duration = DEBUG_DRAW_DURATION)
```

```csharp
public static void DrawFovPlaneDistance(Camera camera, float distance, Color? color = null, float duration = DEBUG_DRAW_DURATION)
```