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
    Text adoptButtonText;
    [SerializeField]
    Button adoptButton;
    [SerializeField]
    UIElement costField;

    Color defaultPriceColor;
    Color overpricedColor = Color.red;
    Color adoptedTextColor = Color.white;

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

        if(checkAdopted(dogInfo))
        {
            showAdopted();
        }
        else
        {
            costField.Show();
            priceText.text = dogInfo.CostToAdoptStr;

            checkReferences();
            if (!game.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt))
            {
                priceText.color = overpricedColor;
            }
            else
            {
                priceText.color = defaultPriceColor;
            }
        }
    }

    #endregion

    bool checkAdopted(DogDescriptor dogInfo)
    {
        return PPDataController.GetInstance.AdoptedDogs.Contains(dogInfo);
    }

    void showAdopted()
    {
        adoptButton.image.color = Color.red;
        adoptButton.interactable = false;
        adoptButtonText.text = "ADOPTED";
        adoptButtonText.color = adoptedTextColor;
        costField.Hide();
    }

}
