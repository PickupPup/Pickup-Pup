/*
 * Authors: Timothy Ng
 * Description: Handles purchases
 * Usage: [no notes]
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : SingletonController<IAPController>, IStoreListener
{
    const string PRODUCT_1000_GOLD = "gold1000";

    IStoreController m_StoreController;
    IExtensionProvider m_StoreExtensionProvider;

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

    #region IStoreListener Overrides
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_1000_GOLD, StringComparison.Ordinal))
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

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(PRODUCT_1000_GOLD, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
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

    public void PurchaseGold1000()
    {
        BuyProductID(PRODUCT_1000_GOLD);
    }

}
