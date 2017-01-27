/*
 * Authors: James Hostetler, Isaiah Mann
 * Description: Controls the Redeem Display.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedeemDisplay : PPUIElement 
{
    [SerializeField]
    Image dogPortrait;
    [SerializeField]
	Text giftDescription;
    [SerializeField]
    Image giftPortrait;
	GiftItem gift;
	Image background;
    Dog dog;

	// Need Eventually For Polish
	[SerializeField]
	Button RedeemButton;
	[SerializeField]
	Button RedeemReturnButton;

    public void Init(Dog dog)
    {
        if(dog.HasRedeemableGift)
        {
            dogPortrait.sprite = dog.Portrait;
            CurrencyData gift = dog.PeekAtGift;
            giftDescription.text = gift.ToString();
            giftPortrait.sprite = gift.Icon;
            this.dog = dog;
            RedeemButton.onClick.AddListener(
                delegate 
                {
                    redeemGift(dog, scoutAgain:false);
                });
            RedeemReturnButton.onClick.AddListener(
                delegate 
                {
                    redeemGift(dog, scoutAgain:true);
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
	public void OnEnable()
	{
        checkReferences();
		background.CrossFadeAlpha(0.7f, 0.2f, false);
	}

	// Updates To The Selected Gift
	public void UpdateDisplay(GiftItem gift, PPLivingRoomUIController room)
	{
		this.gift = gift;
		RedeemButton.interactable = true;
		RedeemReturnButton.interactable = true;
		giftDescription.text = gift.GiftName.ToUpper();
	}

	public void RedeemGift()
	{
		gameController.TryRedeemGift(this.gift);
		CloseDisplay();
	}

	public void CloseDisplay()
	{
		StartCoroutine(closeDisplayCoroutine());
	}
     
    void redeemGift(Dog dog, bool scoutAgain)
    {
        dog.RedeemGift();
        if(scoutAgain)
        {
            dog.TrySendToScout();
        }
        else
        {
            dog.LeaveCurrentSlot(callback:true);
        }
        gameObject.SetActive(false);
    }

	// For Later Polish
	IEnumerator closeDisplayCoroutine(){
		RedeemButton.interactable = false;
		RedeemReturnButton.interactable = false;
		gameObject.SetActive(false);
		yield break;
	}
	
}
