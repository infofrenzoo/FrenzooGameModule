using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class OutletsRush_LevelingModuleResponse : BaseModuleResponse
{
	public override bool CheckAvailable(ResponseClickObject rpo)
	{
		//throw new System.NotImplementedException();
		return false;
	}

	public override void Earn(params RewardData[] rewards)
	{
		Debug.Log("Earn reward!!!");
		//throw new System.NotImplementedException();
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

	public override void OnIAPProductFetched(BaseModule module, IEnumerable<Product> pl)
	{
		//throw new System.NotImplementedException();
	}

	public override bool Spend(ResponseClickObject rpo)
	{
		return false;
	}
}
