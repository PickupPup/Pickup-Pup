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

    PPTuning tuning;

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

    protected override void fetchReferences()
    {
        base.fetchReferences();
        tuning = game.Tuning;
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
                priceText.color = tuning.UnaffordableTextColor;
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
        adoptButton.interactable = false;
        adoptButton.image.color = tuning.AdoptedBackgroundColor;
        adoptButtonText.text = tuning.AdoptedText;
        adoptButtonText.color = tuning.AdoptedTextColor;
        costField.Hide();
    }

}
