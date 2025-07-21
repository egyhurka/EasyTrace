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

### 3. Overlap Sphere

```csharp
if (Trace.Sphere(transform.position, 3f, Trace.DEFAULT_LAYER, out Collider[] hits, true))
{
    foreach (var hit in hits)
        Debug.Log("Nearby: " + hit.name);
}
```

### 4. Draw Movement Trail (Editor-only)

```csharp
Trace.Movement(lastPosition, transform.position, 1f, Color.red);
lastPosition = transform.position;
```

### 5. Draw Sphere Movement Trail (Editor-only)

```csharp
Trace.MovementSphere(transform.position, 1f, Color.red);
```

## 🛠️ Included Files

| File          | Description                                  |
|---------------|----------------------------------------------|
| `Trace.cs`    | Static class for all tracing + debug         |
| `EasyTrace.cs`| Example MonoBehaviour using `Trace` tools    |

---

## 📄 License

This project is licensed under the **MIT License** – feel free to use, modify, and distribute.
