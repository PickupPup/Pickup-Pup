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

	// Need Eventually For Polish
	[SerializeField]
	Button RedeemButton;

	public void Init(Dog dog)
    {
        if(dog.HasRedeemableGift)
        {
            dogPortrait.sprite = dog.Portrait;
            CurrencyData gift = dog.PeekAtGift;
            giftDescription.text = gift.ToString();
            giftPortrait.sprite = gift.Icon;
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
     
    void redeemGift(Dog dog)
    {
        dog.RedeemGift();
		dog.LeaveCurrentSlot(callback:true);
		Destroy();
    }

	// For Later Polish
	IEnumerator closeDisplayCoroutine(){
		RedeemButton.interactable = false;
		gameObject.SetActive(false);
		yield break;
	}
	
}
