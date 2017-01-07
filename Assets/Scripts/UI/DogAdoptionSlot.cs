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
        if (checkAdoption())
        {
            dataController.ChangeCoins(-dog.CostToAdopt);
            dataController.ChangeOpenHomeSlots(-1);
            dataController.Adopt(dog);
            priceOrAdoptionStatus.text = "Adopted";
            priceBackgroundImage.color = Color.red;
        }
    }

    bool checkAdoption()
    {
        if (dataController.Coins.Amount < dog.CostToAdopt || dataController.OpenHomeSlots.Amount <= 0)
        {
            return false;
        }
        return true;
    }

}
