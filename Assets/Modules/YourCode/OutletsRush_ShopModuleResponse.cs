using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutletsRush_ShopModuleResponse : BaseModuleResponse
{
	public OutletsRush_ShopModuleResponse()
	{
		SKU_List = new List<string> { "subscription1" };
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

	public override void Earn(ResponseClickObject rco)
	{
		Debug.Log("Earn " + rco.EnumShopAction);
		switch (rco.EnumShopAction)
		{
			case ShopModuleActionType.NONE:
				break;
			case ShopModuleActionType.PURCHASE_SUBSCRIPTION_1:
				break;
			case ShopModuleActionType.PURCHASE_CLEANER_1:
				break;
		}
	}


}
