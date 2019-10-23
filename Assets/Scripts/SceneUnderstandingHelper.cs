using NumericsConversion;
using System;
using System.Threading.Tasks;
using UnityEngine;

#if ENABLE_WINMD_SUPPORT
using Microsoft.MixedReality.SceneUnderstanding;


internal static class SceneUnderstandingHelper 
{
    internal async static Task<bool> CanComputeAsync()
    {
        if (!canCompute.HasValue)
        {
            canCompute = SceneObserver.IsSupported();

            if ((bool)canCompute)
            {
                var access = await SceneObserver.RequestAccessAsync();

                canCompute = access == SceneObserverAccessStatus.Allowed;
            }
        }
        return ((bool)canCompute);
    }
    internal async static Task<GameObject> ParentGameObjectOnLargestPlatformAsync(GameObject gameObject,
        float searchRadius = 3.0f)
    {
        GameObject parent = null;

        var querySettings = new SceneQuerySettings()
        {
            EnableWorldMesh = false,
            EnableSceneObjectQuads = true,
            EnableSceneObjectMeshes = false,
            EnableOnlyObservedSceneObjects = false
        };
        var scene = await SceneObserver.ComputeAsync(querySettings, searchRadius);

        if (scene != null)
        {
            // Note - we are taking the position of the 'largest' (by area) scene object
            // of type platform here by looking at the quads that make it up.
            // We might need to, instead, query those quads & find their centre position
            // via the FindCentermostPlacement() method and then somehow coalesce those
            // positions to come up with a position instead. i.e. not sure this is at all
            // the 'right' thing to do in terms of coming up with a position.
            var largestPlatform = scene.LargestSceneObjectOfType(SceneObjectKind.Platform);

            if (largestPlatform != null)
            {
                // Where would this platform be in Unity space?
                var unityTransform = scene.GetUnityTransform();

                if (unityTransform.HasValue)
                {
                    parent = new GameObject();

                    parent.transform.SetPositionAndRotation(
                        unityTransform.Value.GetColumn(3), unityTransform.Value.rotation);

                    gameObject.transform.SetParent(parent.transform, false);

                    gameObject.transform.localPosition = largestPlatform.Position.ToUnity();
                    gameObject.transform.localRotation = largestPlatform.Orientation.ToUnity();
                }
            }
        }
        return (parent);
    }
    static bool? canCompute = null;
}
#endif