/*
 * Author: James Hostetler
 * Description: Controls the Redeem Display.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedeemDisplay : PPUIElement {
  
    [SerializeField]
	Text giftDescription;
	PPLivingroomUIController room;
	GiftItem gift;
	Image background;
	CanvasRenderer canv;

	// Need Eventually For Polish
	[SerializeField]
	Button RedeemButton;
	[SerializeField]
	Button RedeemReturnButton;


	protected override void setReferences()
	{
		base.setReferences();
		background = GetComponent<Image>();
		canv = GetComponent<CanvasRenderer>();
	}

	// Fade-in Background
	public void OnEnable()
	{
		canv.SetAlpha(0);
		background.CrossFadeAlpha(0.7f, 0.2f, false);
	}

	// Updates To The Selected Gift
	public void UpdateDisplay(GiftItem gift, PPLivingroomUIController room)
	{
		this.gift = gift;
		this.room = room;
		RedeemButton.interactable = true;
		RedeemReturnButton.interactable = true;
		giftDescription.text = gift.GiftName.ToUpper();
	}

	public void RedeemGift()
	{
		game.TryRedeemGift(this.gift);
		CloseDisplay();
	}

	public void CloseDisplay()
	{
		StartCoroutine(closeDisplayCoroutine());
	}

	// For Later Polish
	IEnumerator closeDisplayCoroutine(){
		RedeemButton.interactable = false;
		RedeemReturnButton.interactable = false;
		gameObject.SetActive(false);
		yield break;
	}
}
