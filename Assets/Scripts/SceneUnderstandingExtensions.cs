using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using NumericsConversion;

#if ENABLE_WINMD_SUPPORT

using Microsoft.MixedReality.SceneUnderstanding;
using Windows.Perception.Spatial.Preview;
using Windows.Perception.Spatial;
using UnityEngine.XR.WSA;

internal static class SceneUnderstandingExtensions
{
    internal static IEnumerable<SceneObject> SceneObjectsOfType(this Scene scene, SceneObjectKind kind)
    {
        return (scene.SceneObjects.Where(so => so.Kind == kind));
    }
    internal static float Area(this SceneQuad sceneQuad)
    {
        return (sceneQuad.Extents.X * sceneQuad.Extents.Y);
    }
    internal static float QuadArea(this SceneObject sceneObject)
    {
        return (sceneObject.Quads.Sum(q => q.Area()));
    }
    internal static SceneObject LargestSceneObjectOfType(this Scene scene, SceneObjectKind kind)
    {
        var objectsOfKind = scene.SceneObjectsOfType(kind);

        // MaxBy is what I want really. 
        // See https://stackoverflow.com/questions/1101841/how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-return-th
        return (objectsOfKind.OrderByDescending(s => s.QuadArea()).FirstOrDefault());
    }
    internal static Matrix4x4? GetUnityTransform(this Scene scene)
    {
        Matrix4x4? transform = null;

        var sceneCoordSystem = SpatialGraphInteropPreview.CreateCoordinateSystemForNode(scene.OriginSpatialGraphNodeId);

        var unityCoordSystem =
            (SpatialCoordinateSystem)System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(
                WorldManager.GetNativeISpatialCoordinateSystemPtr());

        var unityTransform = sceneCoordSystem.TryGetTransformTo(unityCoordSystem);

        if (unityTransform.HasValue)
        {
            transform = unityTransform.Value.ToUnity();
        }
        // TODO: Am I supposed to Release() this or not?
        Marshal.ReleaseComObject(unityCoordSystem);

        return (transform);
    }
}
#endif