using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonController<AdsManager> {

    #region Static Accessors

    // Returns the Instance cast to the sublcass
    public static AdsManager GetInstance
    {
        get
        {
            return Instance as AdsManager;
        }
    }

    #endregion

    const string ANDROID_ID = "1347448";
    const string IOS_ID = "1347449";

    const string adType = "rewardedVideo";

    public void WatchAd()
    {
        if (!Advertisement.isInitialized)
        {
#if UNITY_IOS
        Advertisement.Initialize(IOS_ID, true);
#endif

#if UNITY_ANDROID
            Advertisement.Initialize(ANDROID_ID, true);
#endif
        }


        StartCoroutine(ShowAdWhenReady());
    }

    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady(adType))
            yield return null;

        Advertisement.Show(adType, new ShowOptions()
        {
            resultCallback = result =>
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        Debug.Log("Advertisement Finish, reward player");
                        break;
                    case ShowResult.Failed:
                        Debug.LogError("Advertisement Failed");
                        break;
                    case ShowResult.Skipped:
                        Debug.Log("Advertisement Skipped, Do not reward player");
                        break;
                    default:
                        break;
                }
            }

        });
    }
}
