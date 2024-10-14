using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


[Serializable]
public class RewardGroup
{
	public List<RewardData> RewardDataList { get; private set; }

	public RewardGroup()
	{
		RewardDataList = new List<RewardData>();
	}

	public void SetReward(List<RewardData> rewardDataList)
	{
		RewardDataList = rewardDataList;
	}

	public void AddReward(RewardType type, int amount, int contentId)
	{
		RewardDataList.Add(RewardData.CreateData(type, amount, contentId));
	}

	public void AddReward(RewardData rewardData)
	{
		RewardDataList.Add(rewardData);
	}
}

public class ResponseClickObject
{
	public int ContentId;
	public ShopModuleActionType EnumShopAction;
	public RewardData[] CostDataAry;
	public RewardData[] RewardDataAry;
}

public abstract class BaseModuleResponse<T> where T : BaseModule
{
	public List<string> SKU_List;

	/// <summary>
	/// Check Currency mostly
	/// </summary>
	/// <param name="rpo"></param>
	/// <returns></returns>
	abstract public bool CheckAvailable(ResponseClickObject rpo);
	abstract public bool Spend(ResponseClickObject rpo);
	abstract public void Earn(ResponseClickObject rpo);
	/// <summary>
	/// Get the amount
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	abstract public int GetCurrency(RewardType type);
	/// <summary>
	/// Link IAP Product to UI
	/// </summary>
	/// <param name="module"></param>
	/// <param name="pl"></param>
	abstract public void OnIAPProductFetched(T module, IEnumerable<Product> pl);


}
