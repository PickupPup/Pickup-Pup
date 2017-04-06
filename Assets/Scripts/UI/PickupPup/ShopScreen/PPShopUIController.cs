/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls the shop screen
 */

using UnityEngine;

public class PPShopUIController : PPUIController
{
    [Header("Internal References")]
    [SerializeField]
    GameObject shopBrowser;
    [SerializeField]
    GameObject purchasePage;

    ShopItem[] items;
    ShopItemSlot[] itemSlots;
    ShopDatabase shop;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
		promptID = PromptID.ShopPrompt;
        base.setReferences();
        EventController.Event(PPEvent.LoadShop);
        itemSlots = GetComponentsInChildren<ShopItemSlot>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        shop = ShopDatabase.Instance;
        shop.Initialize();
        items = shop.Items;
        populateShop(items);
    }

    #endregion

    void populateShop(ShopItem[] items)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ShopItemSlot itemSlot = itemSlots[i];
            itemSlot.Init(items[i]);
        }
    }

}
