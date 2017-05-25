/*
 * Authors: Timothy Ng, Grace Barrett-Snyder
 * Description: Controls the logic behind ads
 * Usage: [no notes]
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : SingletonController<AdController>
{
    const string ANDROID_ID = "1347448";
    const string IOS_ID = "1347449";

    const string AD_TYPE = "rewardedVideo";

    #region Static Accessors

    // Returns the Instance cast to the sublcass
    public static AdController GetInstance
    {
        get
        {
            return Instance as AdController;
        }
    }

    #endregion

    public void WatchAd(AdReward reward = null)
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

        StartCoroutine(showAdWhenReady(reward));
    }

    IEnumerator showAdWhenReady(AdReward reward = null)
    {
        while (!Advertisement.IsReady(AD_TYPE))
            yield return null;

        Advertisement.Show(AD_TYPE, new ShowOptions()
        {
            resultCallback = result =>
            {
                switch (result)
                {
                    case ShowResult.Finished:
                        Debug.Log("Advertisement Finished");
                        offerReward(reward);
                        break;
                    case ShowResult.Failed:
                        Debug.LogError("Advertisement Failed");
                        offerReward(reward);
                        break;
                }
            }
        });
    }

    void offerReward(AdReward reward)
    {
        if(reward != null)
        {
            Debug.Log("Offer a reward to the player");
            reward.OfferReward();
        }
    }

}
