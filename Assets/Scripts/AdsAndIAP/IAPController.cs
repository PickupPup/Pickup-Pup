﻿/*
 * Authors: Timothy Ng
 * Description: Handles purchases
 * Usage: [no notes]
 */

using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : SingletonController<IAPController>, IStoreListener
{
    const string PRODUCT_1000_COINS = "coins1000";

	IStoreController storeController;
	IExtensionProvider storeExtensionProvider;

    #region Static Accessors

    // Returns the Instance cast to the sublcass
    public static IAPController GetInstance
    {
        get
        {
            return Instance as IAPController;
        }
    }

    #endregion

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        InitializePurchasing();
    }

    #endregion

    #region IStoreListener Interface

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if(String.Equals(args.purchasedProduct.definition.id, PRODUCT_1000_COINS, StringComparison.Ordinal))
        {
            PPGameController.GetInstance.ChangeCoins(1000);
        }
        else
        {
            Debug.LogError(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    #endregion

    bool isInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void InitializePurchasing()
    {
        if(isInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(PRODUCT_1000_COINS, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    void buyProductID(string productId)
    {
        if(isInitialized())
        {
            Product product = storeController.products.WithID(productId);

            if(product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            { 
                Debug.LogError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.LogError("BuyProductID FAIL. Not initialized.");
        }
    }

    public void PurchaseCoins1000()
    {
        buyProductID(PRODUCT_1000_COINS);
    }

}
