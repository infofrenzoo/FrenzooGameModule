using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
using System;

public partial class UserModuleData
{
	public bool IsPurchasedSubscription = false;
	public bool IsPurchasedNoAds = false;
	public bool IsPurchasedCleaner1 = false;
}

public enum ShopModuleActionType
{
	NONE,
	PURCHASE_SUBSCRIPTION_1,
	PURCHASE_NO_ADS_1,
	PURCHASE_CLEANER_1,
	PURCHASE_SECURITY_1,
	PURCHASE_CASH_1,
	PURCHASE_CASH_2,
	PURCHASE_CASH_3,
	PURCHASE_GEM_1,
	PURCHASE_GEM_2,
	PURCHASE_GEM_3,
	PURCHASE_GEM_4,
	PURCHASE_GEM_5,
	PURCHASE_GEM_6,

}

public class ShopModule : BaseModule, IGameEvent
{
	public static ShopModule Instance;
	public void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

	}
	
	public UserModuleData UserModuleData => ModuleManager.Instance.UserModuleData;
	public TMP_Text COIN;
	public TMP_Text GEM;
	public ModuleActionInfo Subscription1;
	public ModuleActionInfo NoAds1;
	public ModuleActionInfo HireCleaner1;
	public ModuleActionInfo Hiresecurity1;
	public ModuleActionInfo Cash1;
	public ModuleActionInfo Cash2;
	public ModuleActionInfo Cash3;
	public ModuleActionInfo Gem1;
	public ModuleActionInfo Gem2;
	public ModuleActionInfo Gem3;
	public ModuleActionInfo Gem4;
	public ModuleActionInfo Gem5;
	public ModuleActionInfo Gem6;

	public BaseModuleResponse ModuleResponse { get; private set; }
	public void Init(BaseModuleResponse mr)
	{
		ModuleResponse = mr;
		FetchAdditionProduct(ModuleResponse.SKU_List, (pl)=>
		{
			foreach (Product p in pl)
			{
				switch (p.definition.id)
				{
					case "subscription1"://how to do this hard code????
						Subscription1.Product = p;
						Subscription1.Text.text = p.metadata.localizedPriceString;
						break;
				}
			}
		});

		EventController.Instance.AddController(this);
		UpdateUI();
	}
	public override void OnButtonClicked(ModuleActionInfo bi)
	{
		base.OnButtonClicked(bi);
		ResponseClickObject rco = CreateResponsePurchaseObject(bi);
		switch (bi.ShopAction)
		{
			case ShopModuleActionType.NONE:
				break;
			case ShopModuleActionType.PURCHASE_SUBSCRIPTION_1:
			case ShopModuleActionType.PURCHASE_NO_ADS_1:
			case ShopModuleActionType.PURCHASE_CLEANER_1:
			case ShopModuleActionType.PURCHASE_SECURITY_1:
			case ShopModuleActionType.PURCHASE_GEM_1:
			case ShopModuleActionType.PURCHASE_GEM_2:
			case ShopModuleActionType.PURCHASE_GEM_3:
			case ShopModuleActionType.PURCHASE_GEM_4:
			case ShopModuleActionType.PURCHASE_GEM_5:
			case ShopModuleActionType.PURCHASE_GEM_6:
				Purchase(bi);
				break;
			case ShopModuleActionType.PURCHASE_CASH_1:
			case ShopModuleActionType.PURCHASE_CASH_2:
			case ShopModuleActionType.PURCHASE_CASH_3:
				if (ModuleResponse.CheckAvailable(rco) && ModuleResponse.Spend(rco))
					ModuleResponse.Earn(rco);
				break;
			
		}
	}
	public override void Purchase(ModuleActionInfo bi)
	{
		if (IsFetchingProduct)
		{
			Debug.Log("Still Fetching Product");
			return;
		}
		if (bi.Product != null)//GO TO IAP
		{
			SetPurchaseCallback(bi, OnPurchaseComplete, OnPurchaseFail);
			base.Purchase(bi);
		}
		else
		{
			Debug.LogWarning($"No product set for {bi.ShopAction}, you may need to update ShopModule.FetchAdditionProduct");
		}
	}

	private void OnPurchaseFail(Product arg1, PurchaseFailureReason arg2)
	{
		//throw new NotImplementedException();
	}

	private void OnPurchaseComplete(ModuleActionInfo mai)
	{
		switch (mai.ShopAction)
		{
			case ShopModuleActionType.PURCHASE_SUBSCRIPTION_1:
				UserModuleData.IsPurchasedSubscription = true;
				break;
			case ShopModuleActionType.PURCHASE_NO_ADS_1:
				UserModuleData.IsPurchasedNoAds = true;
				break;
			case ShopModuleActionType.PURCHASE_CLEANER_1:
				UserModuleData.IsPurchasedCleaner1 = true;
				break;
		}
		ModuleResponse.Earn(CreateResponsePurchaseObject(mai));
		ModuleManager.Instance.SaveUserModuleData();
	}

	protected override void UpdateUI()
	{
		base.UpdateUI();
		//update cash/gem;
		Subscription1?.gameObject.SetActive(!UserModuleData.IsPurchasedSubscription);
		NoAds1?.gameObject.SetActive(!UserModuleData.IsPurchasedSubscription);
		HireCleaner1?.gameObject.SetActive(!UserModuleData.IsPurchasedSubscription);
	}


	// Update is called once per frame
	void Update()
	{

	}

	public void ReachLevel(int level)
	{
		throw new System.NotImplementedException();
	}

	public void EarnReward(RewardData rd)
	{
		switch (rd.RewardType)
		{
			case RewardType.None:
				break;
			case RewardType.Cash:
				COIN.text = (int.Parse(COIN.text) + rd.Amount).ToString();
				break;
			case RewardType.Gem:
				GEM.text = (int.Parse(GEM.text) + rd.Amount).ToString();
				break;
		}
	}

	public void SpendReward(RewardData rd)
	{
		switch (rd.RewardType)
		{
			case RewardType.None:
				break;
			case RewardType.Cash:
				COIN.text = (int.Parse(COIN.text) - rd.Amount).ToString();
				break;
			case RewardType.Gem:
				GEM.text = (int.Parse(GEM.text) - rd.Amount).ToString();
				break;
		}
	}

}
