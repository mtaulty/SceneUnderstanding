using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class LargePlatformPositioningBehaviour : MonoBehaviour
{
    // Naturally, this could have parameters for things like;
    // 1) the type of object to look for (wall, platform, etc)
    // 2) the search radius
    // 3) UI to display while positioning
    // 4) UI to display if positioning can't be done
    // 5) the minimum size of the object to look for
    // etc. etc.
    // All I've done so far is to assume no UI and 'position on the largest platform'
    // and that implementation is sketchy.
    async void Update()
    {        
        if (!this.positionAttempted)
        {
            this.positionAttempted = true;

#if ENABLE_WINMD_SUPPORT

            var canCompute = await SceneUnderstandingHelper.CanComputeAsync();

            if (canCompute)
            {
                var parent = await SceneUnderstandingHelper.ParentGameObjectOnLargestPlatformAsync(this.gameObject);
            }
#endif
        }
    }
    bool positionAttempted = false;
}