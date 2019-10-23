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

                // Not yet sure whether I should be checking the orientation of the platform
                // that we have found here and then rotating based upon it but, for the moment
                // I'm going to say that this model should face the user and should not be rotated
                // around x,z so that it (hopefully) sits flat on the platform in question.
                var lookPos = CameraCache.Main.transform.position;
                lookPos.y = this.gameObject.transform.position.y;

                this.gameObject.transform.LookAt(lookPos);
            }
#endif
        }
    }
    bool positionAttempted = false;
}