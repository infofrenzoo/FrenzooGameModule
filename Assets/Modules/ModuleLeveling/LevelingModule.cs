using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Purchasing;

public partial class UserModuleData
{
	public int UserLevel = 1;
	public int UserXP = 0;
}

public class LevelingModule : BaseModule
{
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
			EarnXP(10);
	}

	public static LevelingModule Instance;
	public void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

	}

	public LevelAsset LevelAsset;
	public TMP_Text PanelLevelText;
	public List<RewardGameObject> PanelRewardGameObjectList;
	public TMP_Text BarLevelText;
	public TMP_Text BarXPText;
	public Slider BarSlider;

	public LevelInfo GetLevel(int levelId)
	{
		return LevelAsset.LevelInfoList.Where(x => x.LevelId == levelId).FirstOrDefault();
	}


	public int UserLevel
	{
		get
		{
			return UserModuleData.UserLevel;
		}
	}

	public int UserXP
	{
		get { return UserModuleData.UserXP; }
	}

	private LevelInfo _CurrentLevel;
	public LevelInfo CurrentLevel
	{
		get { return _CurrentLevel == null ? _CurrentLevel = GetLevel(UserLevel) : _CurrentLevel; }
	}

	public override void Init(BaseModuleResponse mr)
	{
		base.Init(mr);
		UpdateUI();
	}

	public override void OnButtonClicked(ModuleActionInfo bi)
	{
		base.OnButtonClicked(bi);
	}

	public override void OnGameTick()
	{
		base.OnGameTick();
	}

	public override void Purchase(ModuleActionInfo bi)
	{
		base.Purchase(bi);
	}

	public bool CheckLevelUp()
	{
		if (UserXP >= (CurrentLevel.RequireXp + CurrentLevel.TotalXp))
		{

			if (IsLastLevel())
			{
				UserModuleData.UserXP = CurrentLevel.RequireXp + CurrentLevel.TotalXp;
				return false;
			}
			//Debug.Log("XP: " + UserXP);
			//Debug.Log("CurrentLevel: " + CurrentLevel.LevelId);
			//Debug.Log("RequireXp: " + CurrentLevel.RequireXp);
			ModuleResponse.Earn(CurrentLevel.Rewards.ToArray());
			UserModuleData.UserLevel += 1;

			UserModuleData.Save();
			EventController.Instance.OnReachLevel(UserModuleData.UserLevel);
			//Debug.Log("add level items: "+ string.Join(",",CurrentLevel.Products.Select(x => x.ProductId).ToArray()));
			//UserDataService.AddToOwnedItem(CurrentLevel.NextLevelProducts.Select(x => x.ProductId).ToArray());
			_CurrentLevel = null;
			//UserDataService.UserData.OnPropertyChanged(() => UserDataService.UserData.UserLevel);
			//OnPropertyChanged(() => UserLevel);
			CheckLevelUp();
			return true;
		}
		return false;
	}

	public bool IsLastLevel()
	{
		return UserLevel >= LevelAsset.LevelInfoList.Last().LevelId;
	}

	public void EarnXP(int xp)
	{
		if (IsLastLevel())
			return;
		UserModuleData.UserXP += xp;
		EventController.Instance.OnEarnXP(UserModuleData.UserLevel);
		if (CheckLevelUp())
		{
			UpdateUI();
			ModuleResponse.Earn();
			Panel.SetActive(true);
		}
		else
			UpdateUI();

		UserModuleData.Save();
	}


	protected override void UpdateUI()
	{
		base.UpdateUI();
		Debug.Log("XP: " + UserModuleData.UserXP);
		bool isLastLevel = IsLastLevel();
		int x = UserModuleData.UserXP - CurrentLevel.TotalXp;
		int reqXP = CurrentLevel.RequireXp;
		BarSlider.maxValue = reqXP;
		BarSlider.value = isLastLevel ? reqXP : x;
		BarXPText.text = isLastLevel ? "MAX" : string.Format("{0} / {1}", x, reqXP);
		PanelLevelText.text = BarLevelText.text = CurrentLevel.LevelId.ToString();

		foreach (RewardGameObject go in PanelRewardGameObjectList)
		{
			RewardData rd = CurrentLevel.Rewards.Where(x => x.RewardType == go.Type).FirstOrDefault();
			if (rd != null)
			{
				go.SetRewardData(rd);
				go.gameObject.SetActive(true);
			}
			else
				go.gameObject.SetActive(false);
		}
	}


}
