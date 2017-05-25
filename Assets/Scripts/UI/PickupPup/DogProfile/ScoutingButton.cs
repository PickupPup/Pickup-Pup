/*
 * Author(s): Grace Barrett-Snyder
 * Description: An in-game reward for watching an ad that gives the player coins. 
 */

using UnityEngine;
using UnityEngine.UI;

public class ScoutingButton : PPUIButton
{
    [SerializeField]
    Image iconImage;

    [SerializeField]
    Sprite collarIcon;
    [SerializeField]
    Sprite whistleIcon;

    Dog dog;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        image = iconImage;
    }

    #endregion

    public void SetIcon(Dog dog)
    {
        this.dog = dog;

        if(dog.IsScouting)
        {
            image.sprite = whistleIcon;
        }
        else
        {
            image.sprite = collarIcon;
        }
    }

    public void OnPress()
    {
        if(dog.IsScouting)
        {
            AdController.GetInstance.WatchAd(new DogReturnAdReward(dog));
        }
        else
        {
            //TODO
        }
    }
}
