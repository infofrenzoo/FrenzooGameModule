using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public abstract class BaseModule : MonoBehaviour
{
	public GameObject Panel;
	public bool IsFetchingProduct = false;
	private Action<Product, PurchaseFailureReason> OnPurchaseFailAction;
	private Action<ModuleActionInfo> OnPurchaseCompleteAction;
	private ModuleActionInfo BuyInfo;

	public virtual void OnButtonClicked(ModuleActionInfo bi)
	{

	}

	protected void FetchAdditionProduct(List<string> skuList, Action<IEnumerable<Product>> successAction = null)
	{
		IsFetchingProduct = true;
		IAPManager.Instance.FetchAdditionProduct(skuList, (pl) =>
		{
			successAction?.Invoke(pl);
			IsFetchingProduct = false;
		});
	}

	public virtual void Purchase(ModuleActionInfo bi)
	{
		BuyInfo = bi;
		IAPManager.Instance.Purchase(bi.Product.definition.id, OnPurchaseComplete, OnPurchaseFail);
	}

	public virtual void SetPurchaseCallback(ModuleActionInfo bi, Action<ModuleActionInfo> onPurchaseComplete, Action<Product, PurchaseFailureReason> onPurchaseFail)
	{
		BuyInfo = bi;
		OnPurchaseCompleteAction = onPurchaseComplete;
		OnPurchaseFailAction = onPurchaseFail;
	}
	//make it virtual??
	private void OnPurchaseFail(Product arg1, PurchaseFailureReason arg2)
	{
		OnPurchaseFailAction?.Invoke(arg1, arg2);
		OnPurchaseFailAction = null;
	}

	//make it virtual??
	private void OnPurchaseComplete(Product obj)
	{
		OnPurchaseCompleteAction?.Invoke(BuyInfo);
		OnPurchaseCompleteAction = null;
		UpdateUI();
	}

	protected virtual void UpdateUI()
	{

	}

	protected ResponseClickObject CreateResponsePurchaseObject(ModuleActionInfo mai)
	{
		return new ResponseClickObject() { EnumShopAction = mai.ShopAction, ContentId = mai.ContentId, RewardGroup = mai.RewardGroup};
	}
}
