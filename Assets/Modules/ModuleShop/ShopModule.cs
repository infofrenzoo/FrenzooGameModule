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
	public bool IsPurchasedSecurity1 = false;

	public int NextdailyRewardIndex;
	public DateTime NextDailyReward = DateTime.MinValue;
	public DateTime LastDailyReward = DateTime.MinValue;
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
	DAILY_REWARD_1,
	DAILY_REWARD_2,
	DAILY_REWARD_3,
	DAILY_REWARD_4,
	SKIP_DAILY_REWARD,

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
	public ModuleActionInfo HireSecurity1;
	public ModuleActionInfo Cash1;
	public ModuleActionInfo Cash2;
	public ModuleActionInfo Cash3;
	public ModuleActionInfo Gem1;
	public ModuleActionInfo Gem2;
	public ModuleActionInfo Gem3;
	public ModuleActionInfo Gem4;
	public ModuleActionInfo Gem5;
	public ModuleActionInfo Gem6;
	public ModuleActionInfo DailyReward1;
	public ModuleActionInfo DailyReward2;
	public ModuleActionInfo DailyReward3;
	public ModuleActionInfo DailyReward4;
	public ModuleActionInfo SkipDailyReward;
	public TMP_Text DailyRewardTimer;
	public BaseModuleResponse<ShopModule> ModuleResponse { get; private set; }
	public void Init(BaseModuleResponse<ShopModule> mr)
	{
		base.Init();
		ModuleResponse = mr;
		FetchAdditionProduct(ModuleResponse.SKU_List, (pl) =>
		{
			ModuleResponse.OnIAPProductFetched(this, pl);

		});

		EventController.Instance.AddController(this);
		CheckDailyReward();
		UpdateUI();
	}

	public override void OnGameTick()
	{
		base.OnGameTick();
		if (!CheckDailyReward())
		{
			var a = UserModuleData.NextDailyReward - ModuleManager.Instance.CurrentTime;
			//Debug.Log(a);
			DailyRewardTimer.text = ModuleManager.Instance.GetDisplayTime(UserModuleData.NextDailyReward - ModuleManager.Instance.CurrentTime);

		}
		else
		{
			if (DailyRewardTimer.transform.parent.gameObject.activeSelf)
			{
				UpdateUI();
			}
		}

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
			case ShopModuleActionType.DAILY_REWARD_1:
				ClaimDailyReward(bi);
				break;
			case ShopModuleActionType.DAILY_REWARD_2:
				ClaimDailyReward(bi);
				break;
			case ShopModuleActionType.DAILY_REWARD_3:
				ClaimDailyReward(bi);
				break;
			case ShopModuleActionType.DAILY_REWARD_4:
				ClaimDailyReward(bi);
				break;
			case ShopModuleActionType.SKIP_DAILY_REWARD:
				if (ModuleResponse.CheckAvailable(rco) && ModuleResponse.Spend(rco))
				{
					if (UserModuleData.NextdailyRewardIndex >= 4)
						UserModuleData.NextDailyReward = DateTime.MinValue;
					else
						UserModuleData.NextDailyReward = ModuleManager.Instance.CurrentTime;
					CheckDailyReward();
					UpdateUI();
				}
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
			case ShopModuleActionType.PURCHASE_SECURITY_1:
				UserModuleData.IsPurchasedSecurity1 = true;
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
		DateTime now = ModuleManager.Instance.CurrentTime.AddSeconds(1);
		DailyRewardTimer.transform.parent.gameObject.SetActive(UserModuleData.NextDailyReward >= now);
		SkipDailyReward.gameObject.SetActive(UserModuleData.NextDailyReward >= now);

		bool canClaim = now >= UserModuleData.NextDailyReward;

		GetChildGameObject(DailyReward1.gameObject, "Tick").SetActive(UserModuleData.NextdailyRewardIndex > 0);
		GetChildGameObject(DailyReward2.gameObject, "Tick").SetActive(UserModuleData.NextdailyRewardIndex > 1);
		GetChildGameObject(DailyReward3.gameObject, "Tick").SetActive(UserModuleData.NextdailyRewardIndex > 2);
		GetChildGameObject(DailyReward4.gameObject, "Tick").SetActive(UserModuleData.NextdailyRewardIndex > 3);
		GetChildGameObject(DailyReward1.gameObject, "Get").SetActive(UserModuleData.NextdailyRewardIndex == 0 && canClaim);
		GetChildGameObject(DailyReward2.gameObject, "Get").SetActive(UserModuleData.NextdailyRewardIndex == 1 && canClaim);
		GetChildGameObject(DailyReward3.gameObject, "Get").SetActive(UserModuleData.NextdailyRewardIndex == 2 && canClaim);
		GetChildGameObject(DailyReward4.gameObject, "Get").SetActive(UserModuleData.NextdailyRewardIndex == 3 && canClaim);

	}

	/// <summary>
	/// return canClaim
	/// </summary>
	/// <returns></returns>
	public bool CheckDailyReward()
	{
		if (ModuleManager.Instance.CurrentTime.Date > UserModuleData.LastDailyReward.Date) //next day
			UserModuleData.NextDailyReward = DateTime.MinValue;
		if (UserModuleData.NextDailyReward == DateTime.MinValue)
		{
			UserModuleData.NextdailyRewardIndex = 0;
			ModuleManager.Instance.SaveUserModuleData();
		}

		return ModuleManager.Instance.CurrentTime.AddSeconds(1) >= UserModuleData.NextDailyReward;
	}

	public void ClaimDailyReward(ModuleActionInfo bi)
	{
		ResponseClickObject rco = CreateResponsePurchaseObject(bi);
		int sec = 0;
		switch (bi.ShopAction)
		{
			case ShopModuleActionType.NONE:
				break;
			case ShopModuleActionType.DAILY_REWARD_1:
				sec = 5;
				break;
			case ShopModuleActionType.DAILY_REWARD_2:
				sec = 10;
				break;
			case ShopModuleActionType.DAILY_REWARD_3:
				sec = 20;
				break;
			case ShopModuleActionType.DAILY_REWARD_4:
				sec = (int)(ModuleManager.Instance.CurrentTime.AddDays(1).Date - ModuleManager.Instance.CurrentTime).TotalSeconds;
				break;
		}
		UserModuleData.NextdailyRewardIndex = UserModuleData.NextdailyRewardIndex + 1;
		UserModuleData.LastDailyReward = ModuleManager.Instance.CurrentTime;
		UserModuleData.NextDailyReward = ModuleManager.Instance.CurrentTime.AddSeconds(sec);
		ModuleResponse.Earn(rco);
		OnGameTick();
		UpdateUI();
		ModuleManager.Instance.SaveUserModuleData();
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
