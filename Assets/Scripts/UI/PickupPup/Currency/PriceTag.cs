/*
 * Author: Grace Barrett-Snyder
 * Description: Controls a price tag on an Adoption Slot, or Shop Item Slot
 */

using UnityEngine;
using UnityEngine.UI;

public class PriceTag : PPUIElement
{
    [SerializeField]
    UIElement iconHolder;
    [SerializeField]
    Image priceBackgroundImage;
    Text priceText;

    string nonPurchasableText;
    Color nonPurchasableTextColor;
    Color nonPurchasableBackgroundColor;

    int price;
    string priceStr;
    CurrencyData currency;
    CurrencyType currencyType;

    PPTuning tuning;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        priceText = GetComponentInChildren<Text>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
        tuning = gameController.Tuning;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if (dataController)
        {
            dataController.SubscribeToCurrencyChange(CurrencyType.Coins, updateTextColor);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if (dataController)
        {
            dataController.UnsubscribeFromCurrencyChange(CurrencyType.Coins, updateTextColor);
        }
    }

    #endregion

    public void Set(int price)
    {
        unsubscribeEvents();
        this.price = price;
        priceStr = PPData.FormatCost(price);

        checkReferences();
        ShowPurchasable();
        subscribeEvents();
    }

    public void SetNonPurchasable(string nonPurchasableText, Color nonPurchasableTextColor, Color nonPurchasableBackgroundColor)
    {
        this.nonPurchasableText = nonPurchasableText;
        this.nonPurchasableTextColor = nonPurchasableTextColor;
        this.nonPurchasableBackgroundColor = nonPurchasableBackgroundColor;   
    }

    public void ShowPurchasable()
    {
        setComponents(priceStr, tuning.DefaultTextColor, tuning.DefaultBackgroundColor, true);
        updateTextColor(gameController.Coins.Amount);
    }

    public void ShowNonPurchasable()
    {
        setComponents(nonPurchasableText, nonPurchasableTextColor, nonPurchasableBackgroundColor, false);
        unsubscribeEvents();
    }

    protected void setComponents(string priceTextString, Color priceTextColor,
        Color priceBackgroundColor, bool showIconHolder)
    {
        priceText.text = priceTextString;
        priceText.color = priceTextColor;
        priceBackgroundImage.color = priceBackgroundColor;
        if (showIconHolder)
        {
            iconHolder.Show();
        }
        else
        {
            iconHolder.Hide();
        }
    }

    void updateTextColor(int amount)
    {
        if (!gameController.CanAfford(currencyType, price))
        {
            priceText.color = tuning.UnaffordableTextColor;
        }
        else
        {
            priceText.color = tuning.DefaultPriceColor;
        }
    }

}
