/*
 * Author: James Hostetler
 * Description: Controls the Livingroom UI.
 */

using UnityEngine;

public class PPLivingRoomUIController : PPUIController
{
	[SerializeField]
    CurrencyDisplay dogFoodDisplay;
	[SerializeField]
    CurrencyDisplay coinDisplay;
	[SerializeField]
	RedeemDisplay rDisplay;

	GiftItem[] gifts;
	GiftRedeemSlot[] giftSlots;
	GiftDatabase giftBase;

	#region MonoBehaviourExtended Overrides

    protected override void setReferences() 
	{
        base.setReferences();
		giftSlots = GetComponentsInChildren<GiftRedeemSlot>();
     	giftBase = GiftDatabase.Instance;
		giftBase.Initialize();
		gifts = giftBase.Gifts;
		generateGift(gifts);
    }

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		EventController.Event(PPEvent.LoadLivingroom);

		// Display Updated Currency
        dogFoodDisplay.SetCurrency(gameController.DogFood);
        coinDisplay.SetCurrency(gameController.Coins);
    }

	#endregion

	// TEMPORARY
	void generateGift(GiftItem[] gifts)
	{
		for (int i = 0; i < giftSlots.Length; i++)
		{
			GiftRedeemSlot giftSlot = giftSlots[i];
			giftSlot.Init(this, gifts[Random.Range(0,gifts.Length)]);
		}
	}

	public void RedeemGift(GiftItem gift)
	{
		rDisplay.gameObject.SetActive(true);
		rDisplay.UpdateDisplay(gift,this);
	}

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

	public void OnShopClick()
    {
        sceneController.LoadShop();
    }

	// Temporary Fix For Updating Currencies
	public void Update()
	{
        dogFoodDisplay.SetCurrency(gameController.DogFood);
        coinDisplay.SetCurrency(gameController.Coins);
        coinDisplay.OnUpdate();
        dogFoodDisplay.OnUpdate();

	}

}
