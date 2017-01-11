/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot in the Shelter scene (has price tag).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogAdoptionSlot : DogSlot
{
    Text priceOrAdoptionStatus;
    Image priceBackgroundImage;

    Color adoptedColor = Color.red;

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);

        priceOrAdoptionStatus = GetComponentInChildren<Text>();
        priceOrAdoptionStatus.text = dog.CostToAdoptStr;

        priceBackgroundImage = images[2];
    }

    #endregion

    public void Adopt()
    {
        if(game.TryAdoptDog(dogInfo))
        {
            priceOrAdoptionStatus.text = "Adopted";
            priceBackgroundImage.color = Color.red;
        }
    }

}
