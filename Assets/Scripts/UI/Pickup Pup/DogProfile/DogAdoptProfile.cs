/*
 * Author: Grace Barrett-Snyder 
 * Description: UI for displaying an individual dog's profile in the Shelter scene
 */

using UnityEngine;
using UnityEngine.UI;

public class DogAdoptProfile : DogProfile
{
    [SerializeField]
    Text priceText;
    [SerializeField]
    Button adoptButton;

    Color defaultPriceColor;
    Color overpricedColor = Color.red;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        iconsObject.SetActive(false);
        defaultPriceColor = priceText.color;
    }

    #endregion

    #region DogProfile Overrides

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        priceText.text = dogInfo.CostToAdoptStr;

        checkReferences();

        if(!game.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt))
        {
            priceText.color = overpricedColor;
        }
        else
        {
            priceText.color = defaultPriceColor;
        }
    }

    #endregion

}
