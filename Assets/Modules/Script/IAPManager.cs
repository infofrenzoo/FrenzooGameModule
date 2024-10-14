using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public enum IAPStatus
{
    Initialize,
    GetIAPProductList,
    Purchase,
    ProcessPurchase,
    OnPurchaseFailed,
}
public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public static IAPManager Instance;
    private IStoreController controller;
    private IExtensionProvider extensions;
    private IAppleExtensions m_AppleExtensions;
    //private IUnityChannelExtensions m_UnityChannelExtensions;
    private ITransactionHistoryExtensions m_TransactionHistoryExtensions;
    private IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;
    private bool m_IsGooglePlayStoreSelected;
    private bool m_IsAppleStoreSelected;

    public Action<string> OnIAPFailed;
    Action<Product> OnSucceed;
    Action<Product, PurchaseFailureReason> OnFailed;

    public Action OnInitializedAction = null;
    public bool IsInited = false;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void SetOnInitAction(Action action)
    {
        if (IsInited)
            action();
        else
        {
            OnInitializedAction = action;
        }
    }

    private bool IsInitialized()
    {
        bool inited = controller != null && extensions != null;
        if (!inited)
        {
            if (OnIAPFailed != null)
                OnIAPFailed("IAP Not Initialized");
        }
        return inited;
    }


    public void Init()
    {
        var module = StandardPurchasingModule.Instance();
        m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;
        m_IsAppleStoreSelected = Application.platform == RuntimePlatform.IPhonePlayer;
        var builder = ConfigurationBuilder.Instance(module);

//        foreach (var iap in ServiceManager.Instance.AppDataService.GetIAPList())
//        {
//            builder.AddProduct(iap.Sku, ProductType.Consumable, new IDs
//            {
//#if UNITY_ANDROID && !UNITY_KINDLE && !IS_UDP
//                    { iap.Sku, GooglePlay.Name },
//#elif UNITY_IPHONE
//                    { iap.Sku, MacAppStore.Name },
//#elif IS_UDP
//                    { iap.Sku, UDP.Name },
//#else
//                    { iap.Sku, AmazonApps.Name }
//#endif
//            });
//        }

        UnityPurchasing.Initialize(this, builder);
        //Debug.Log("Initialized builder");
    }

    public List<Product> GetIAPProductList()
    {
        if (!IsInitialized())
            return new List<Product>();
        var pros = controller.products;
        List<Product> list = new List<Product>();
        list = controller.products.all.ToList();
        return list;
    }

    public string GetProductPrice(string productId)
    {
        if (controller != null)
        {
            Product product = controller.products.WithID(productId);
            if (product != null)
            {
                return product.metadata.localizedPriceString;
            }
            return "-";
        }
        return "-";
    }

    Queue<Dictionary<HashSet<ProductDefinition>, Action<IEnumerable<Product>>>> FetchIapSkusQueue = new Queue<Dictionary<HashSet<ProductDefinition>, Action<IEnumerable<Product>>>>();
    private bool isFetching = false;

    List<string> skuListFetchAfterInit = new List<string>();
    public void FetchAdditionProduct(string sku, Action<Product> successAction = null)
    {
        if (string.IsNullOrEmpty(sku))
            return;
        FetchAdditionProduct(new List<string> { sku }, (ie) => successAction?.Invoke(ie.FirstOrDefault()));
    }

    public void FetchAdditionProduct(List<string> skuList, Action<IEnumerable<Product>> successAction = null)
    {
#if UNITY_EDITOR
        return;
#endif
        if (Application.isEditor)
            return;
        List<string> _sku = skuList;

        if (!IsInitialized())
        {
            var tempSkuList = _sku.Where(x => !skuListFetchAfterInit.Contains(x)).ToList();
            if (tempSkuList.Count > 0)
                skuListFetchAfterInit.AddRange(tempSkuList);
            else
            {
                skuListFetchAfterInit.AddRange(skuList);
            }
            return;
        }

        IEnumerable<Product> pro = controller.products.all.Where(x => _sku.Contains(x.definition.id));

        if (pro != null && pro.Count() > 0)
        {
            successAction?.Invoke(pro);
            return;
        }

        HashSet<ProductDefinition> itemsHashSet = new HashSet<ProductDefinition>();

        for (int i = 0; i < _sku.Count; i++)
        {
            itemsHashSet.Add(new ProductDefinition(_sku[i], ProductType.Consumable));
        }

        FetchIapSkusQueue.Enqueue(new Dictionary<HashSet<ProductDefinition>, Action<IEnumerable<Product>>>() { { itemsHashSet, successAction } });

        if (!isFetching)
            ModuleManager.Instance.DelayFrame(2, () =>
            {
                FetchNextSku();
            });

        return;
        ////MeGirl.Threading.UIThreadDispatcher.Instance.EnqueueOnMainThread(() => {
        //controller.FetchAdditionalProducts(itemsHashSet,
        //   () =>
        //   {
        //       if (successAction != null)
        //       {
        //           successAction(controller.products.all.Where(x => _sku.Contains(x.definition.id)));
        //       }
        //   },
        //   (failReason, message) =>
        //   {
        //       //FetchAdditionProduct(sku);
        //   });
        ////});
    }

    private void FetchNextSku()
    {
        if (FetchIapSkusQueue.Count > 0)
        {
            var iapSkuWithAction = FetchIapSkusQueue.Peek().First();

            isFetching = true;

            controller.FetchAdditionalProducts(iapSkuWithAction.Key,
             () =>
             {
                 Debug.LogWarning($"Fetch product Done: {string.Join(",", iapSkuWithAction.Key.Select(x => x.id))}");


                 if (iapSkuWithAction.Value != null)
                 {
                     var _sku = iapSkuWithAction.Key.Select(x => x.id);
                     iapSkuWithAction.Value(controller.products.all.Where(x => _sku.Contains(x.definition.id)));
                 }

                 OnProductsFetched();
             },
             (failReason, message) =>
             {
                 Debug.LogWarning($"Fetch product Fail: {string.Join(",", iapSkuWithAction.Key.Select(x => x.id))}");
                 //FetchAdditionProduct(sku);
                
                 OnProductsFetched();
             });
        }
    }

    public void OnProductsFetched()
    {
        FetchIapSkusQueue.Dequeue();

        isFetching = false;
        ModuleManager.Instance.DelayFrame(2, () =>
        {
            FetchNextSku();
        });

    }

    /// <summary>
    /// USE IAPService instead
    /// </summary>
    /// <param name="product"></param>
    /// <param name="onSucceed"></param>
    /// <param name="onFailed"></param>
    public void Purchase(Product product, Action<Product> onSucceed, Action<Product, PurchaseFailureReason> onFailed)
    {
        if (!IsInitialized())
            return;
        OnSucceed = onSucceed;
        OnFailed = onFailed;
        controller.InitiatePurchase(product);
    }
    /// <summary>
    /// USE IAPService instead
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="onSucceed"></param>
    /// <param name="onFailed"></param>
    public void Purchase(string productId, Action<Product> onSucceed, Action<Product, PurchaseFailureReason> onFailed)
    {
        if (!IsInitialized())
            return;
        OnSucceed = onSucceed;
        OnFailed = onFailed;
        controller.InitiatePurchase(productId);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        //m_UnityChannelExtensions = extensions.GetExtension<IUnityChannelExtensions>();
        m_TransactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        ModuleManager.Instance.DelayFrame(2, () =>
        {
            FetchAdditionProduct(skuListFetchAfterInit);
        });


        Debug.Log("IAP Initialized");
        IsInited = true;
        if (OnInitializedAction != null)
        {
            OnInitializedAction();
        }
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (!IsInitialized())
            return PurchaseProcessingResult.Pending;
        Debug.Log("Purchase Complete.");
        if (OnSucceed != null)
            OnSucceed(e.purchasedProduct);
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        if (!IsInitialized())
            return;
        if (OnFailed != null)
            OnFailed(i, p);
        Debug.Log("OnPurchaseFailed: " + p.ToString());

        // Detailed debugging information
        Debug.Log("Store specific error code: " + m_TransactionHistoryExtensions.GetLastStoreSpecificPurchaseErrorCode());
        if (m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription() != null)
        {
            Debug.Log("Purchase failure description message: " +
                      m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription().message);
        }

    }

    public void RestoreTransactions()
    {
        if (m_IsGooglePlayStoreSelected)
        {
            m_GooglePlayStoreExtensions.RestoreTransactions(OnTransactionsRestored);
        }
        else if (m_IsAppleStoreSelected)
        {
            m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
        }
    }

    private void OnTransactionsRestored(bool success, string message)
    {
        Debug.Log("Transactions restored." + success);
        if (!string.IsNullOrEmpty(message))
            Debug.LogWarning("restore message:" + message);
    }

    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");

                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }

        Debug.LogError("message:" + message);
    }

	public void OnPurchaseFailed(Product p, PurchaseFailureDescription failureDescription)
	{
        if (!IsInitialized())
            return;
        if (OnFailed != null)
            OnFailed(p, failureDescription.reason);
        Debug.Log("OnPurchaseFailed: " + p.ToString());

        // Detailed debugging information
        Debug.Log("Store specific error code: " + m_TransactionHistoryExtensions.GetLastStoreSpecificPurchaseErrorCode());
        if (m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription() != null)
        {
            Debug.Log("Purchase failure description message: " +
                      m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription().message);
        }
    }
}