using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class OutletsRush_ShopModuleResponse : BaseModuleResponse
{
	public OutletsRush_ShopModuleResponse()
	{
		SKU_List = new List<string> { "subscription1", "noAds1", "HireCleaner1", "HireSecurity1", "Gem1", "Gem2", "Gem3", "Gem4", "Gem5", "Gem6" };
	}

	public override void OnIAPProductFetched(BaseModule bmodule, IEnumerable<Product> pl)
	{
		ShopModule module = (ShopModule)bmodule;
		foreach (Product p in pl)
		{
			switch (p.definition.id)
			{
				case "subscription1":
					module.Subscription1.SetProduct(p);
					break;
				case "noAds1":
					module.NoAds1.SetProduct(p);
					break;
				case "HireCleaner1":
					module.HireCleaner1.SetProduct(p);
					break;
				case "HireSecurity1":
					module.HireSecurity1.SetProduct(p);
					break;
				case "Gem1":
					module.Gem1.SetProduct(p);
					break;
				case "Gem2":
					module.Gem2.SetProduct(p);
					break;
				case "Gem3":
					module.Gem3.SetProduct(p);
					break;
				case "Gem4":
					module.Gem4.SetProduct(p);
					break;
				case "Gem5":
					module.Gem5.SetProduct(p);
					break;
				case "Gem6":
					module.Gem6.SetProduct(p);
					break;

			}
		}
	}

	public override bool CheckAvailable(ResponseClickObject rco)
	{
		Debug.Log("CheckAvailable");
		bool available = true;
		switch (rco.EnumShopAction)
		{
			case ShopModuleActionType.NONE:
				break;
			case ShopModuleActionType.PURCHASE_CASH_1:
				break;
			case ShopModuleActionType.PURCHASE_CASH_2:
				break;
			case ShopModuleActionType.PURCHASE_CASH_3:
				break;
			case ShopModuleActionType.SKIP_DAILY_REWARD:
				break;
		}
		return available;
	}

	public override bool Spend(ResponseClickObject rco)
	{
		Debug.Log("Spend");
		bool spent = true;
		switch (rco.EnumShopAction)
		{
			case ShopModuleActionType.NONE:
				break;
			case ShopModuleActionType.PURCHASE_SUBSCRIPTION_1:
				break;
			case ShopModuleActionType.PURCHASE_CLEANER_1:
				break;
		}
		return spent;
	}

	public override void Earn(params RewardData[] rewards)
	{
		//Debug.Log("Earn " + rco.EnumShopAction);
		foreach (var reward in rewards)
		{
			switch (reward.RewardType)
			{
				case RewardType.None:
					break;
				case RewardType.Cash:
					break;
				case RewardType.Gem:
					break;
				case RewardType.MoveSpeed:
					break;
				case RewardType.CashMultiplier:
					break;
			}
		}
	}

	public override int GetCurrency(RewardType type)
	{
		int amount = 0;
		switch (type)
		{
			case RewardType.Cash:
				break;
			case RewardType.Gem:
				break;
			case RewardType.MoveSpeed:
				break;
			case RewardType.CashMultiplier:
				break;
		}
		return amount;
	}
}
