/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class ShopItemSlot : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image itemImage;

    [SerializeField]
    PriceTag priceTag;
    Button button;

	ShopAmountSelector amountSelector;
    ShopItem item;

	int amountToBuy = k.SINGLE_VALUE;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        button = GetComponent<Button>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if (dataController)
        {
            dataController.SubscribeToCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if (dataController)
        {
            dataController.UnsubscribeFromCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    #endregion

	public void Init(ShopItem item, ShopAmountSelector amountSelector)
    {
        subscribeEvents();
        this.item = item;
		this.amountSelector = amountSelector;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
        priceTag.Set(item.CostAmount);
        priceTag.ShowPurchasable();
        itemImage.sprite = item.Icon;
		itemImage.color = item.IconColor;
    }

    public void Buy()
    {
		amountSelector.SetItem(item);
		amountSelector.Show();
    }

	void resetAfterPurchase()
	{
		amountToBuy = k.SINGLE_VALUE;
	}

    void tryToggle(int amount)
    {
        if(button)
        {
			button.interactable = gameController.CanAfford(
				item.CostCurrencyType, 
				item.GetTotalCost(amountToBuy));
        }
    }

}
