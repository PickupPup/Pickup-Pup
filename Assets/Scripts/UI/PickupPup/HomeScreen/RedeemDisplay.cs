/*
 * Authors: James Hostetler, Isaiah Mann
 * Description: Controls the Redeem Display.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using k = PPGlobal;

public class RedeemDisplay : PPUIElement 
{
    [SerializeField]
    Image dogPortrait;
    [SerializeField]
    Text dogNameDisplay;
    [SerializeField]
	Text giftDescription;
    [SerializeField]
    Image giftPortrait;
	Image background;

	// Need Eventually For Polish
	[SerializeField]
	Button RedeemButton;

	public void Init(Dog dog)
    {
        if(dog.HasRedeemableGift)
        {
            dogNameDisplay.text = formatRedeemMessage(dog);
            dogPortrait.sprite = dog.Portrait;
            CurrencyData gift = dog.PeekAtGift;
			if(gift is SpecialGiftData)
			{
				(gift as SpecialGiftData).SetFinder(dog.Info);
			}
            giftDescription.text = gift.ToString();
            giftPortrait.sprite = gift.Icon;
			if(gift is DogFoodData)
			{
				giftPortrait.color = (gift as DogFoodData).Color;
			}
            RedeemButton.onClick.AddListener(
                delegate 
                {
                    redeemGift(dog);
                });
        }
    }
       
    #region MonoBehaviourExtended Overrides 

	protected override void setReferences()
	{
		base.setReferences();
        background = GetComponentInChildren<Image>();
	}

    #endregion

	// Fade-in Background
	new public void OnEnable()
	{
        base.OnEnable();
        checkReferences();
		background.CrossFadeAlpha(0.7f, 0.2f, false);
	}

    void redeemGift(Dog dog)
    {
        if(dog.OccupiedSlot && dog.OccupiedSlot is DogCollarSlot)
        {
            (dog.OccupiedSlot as DogCollarSlot).ToggleRedeemDisplayOpen(isOpen:false);
        }
        dog.RedeemGift();
        dog.LeaveCurrentSlot(callback:true, stopScouting:true);
        dataController.SaveGame();
		Destroy();
    }

    string formatRedeemMessage(Dog dog)
    {
        string formatText = languageDatabase.GetTerm(k.REDEEM_DISPLAY_TEXT_KEY);
        return string.Format(formatText, dog.Name);
    }

	// For Later Polish
	IEnumerator closeDisplayCoroutine(){
		RedeemButton.interactable = false;
		gameObject.SetActive(false);
		yield break;
	}
	
}
