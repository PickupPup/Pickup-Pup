/*
 * Authors: Timothy Ng
 * Description: Controls the logic behind ads
 * Usage: [no notes]
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonController<AdsManager>
{

    const string ANDROID_ID = "1347448";
    const string IOS_ID = "1347449";

    const string AD_TYPE = "rewardedVideo";

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
        while (!Advertisement.IsReady(AD_TYPE))
            yield return null;

        Advertisement.Show(AD_TYPE, new ShowOptions()
        {
            resultCallback = result =>
            {
                switch (result)
                {
                    case ShowResult.Failed:
                        Debug.LogError("Advertisement Failed");
                        break;
                }
            }

        });
    }
}
