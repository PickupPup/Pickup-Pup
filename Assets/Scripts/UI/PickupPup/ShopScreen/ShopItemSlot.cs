/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image itemImage;
	[SerializeField]
	Text amountText;

    [SerializeField]
    PriceTag priceTag;

    PPShopUIController shop;
    ShopItem item;

    public void Init(PPShopUIController shop, ShopItem item)
    {
        this.shop = shop;
        this.item = item;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
		if(amountText)
		{
			amountText.text = item.Value.ToString();
		}
        priceTag.Set(item.Cost);
        priceTag.ShowPurchasable();
        itemImage.sprite = item.Icon;
    }

    public void Buy()
    {
        gameController.TryBuyItem(item);
    }

}
