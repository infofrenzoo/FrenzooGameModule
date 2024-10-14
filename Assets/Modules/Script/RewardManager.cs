using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Globalization;
using TMPro;


[Serializable]
public class RewardInfo
{
	public RewardType Type;
	public int ContentId = 0;
	public Sprite Icon;
	public Sprite Icon_big;
	public Sprite Old_Icon;
	public Sprite[] IconDetails;
	/// <summary>
	/// assign directly if no other language
	/// </summary>
	public string Name;// { get; set; }
	public string Name_pl;// { get; set; }

}
public class RewardManager : MonoBehaviour
{
	public static RewardManager Instance;
	[SerializeField]

	public RewardInfoAsset RewardInfoAsset;
	private List<RewardInfo> RewardInfoList
	{
		get
		{
			return RewardInfoAsset.RewardInfoList;
		}
	}
	//private GameController GameManager => GameController.Instance;
	//private UserDataService UserDataService => GameManager.UserDataService;
	//private AppDataService AppDataService => ServiceManager.Instance.AppDataService;

	//private ServiceManager ServiceManagre => ServiceManager.Instance;
	//private Dictionary<RewardType, Reward> _RewardDic;
	/// <summary>
	/// no init Localization omg
	/// </summary>
	/// <returns></returns>
	//public void Init(LocalizationService localizationService)
	//{
	//	Dictionary<RewardType, Reward> _RewardDic = localizationService.GetReward().ToDictionary(x => (RewardType)Enum.Parse(typeof(RewardType), x.Tag), y => y);

	//	for (int i = 0; i < RewardInfoList.Count; i++)
	//	{
	//		if (_RewardDic.ContainsKey(RewardInfoList[i].Type))
	//		{
	//			Reward reward = _RewardDic[RewardInfoList[i].Type];
	//			RewardInfoList[i].Name = reward.Name;
	//			RewardInfoList[i].Name_pl = reward.Name_pl;

	//		}
	//		else
	//		{
	//			//GameManager.LogWarning($"No Reward Type {RewardInfoList[i].Type.ToString()} in language db");
	//		}
	//	}
	//}

	public List<RewardInfo> GetRewardInfoList()
	{
		return RewardInfoList;

	}
	
	public bool CanSpendCurrency(RewardType type, int amount, int ContentId = 0)
	{
		throw new NotImplementedException();
		//switch (type)
		//{
		//	case RewardType.Cash:
		//		return amount <= UserDataService.UserData.UserCoin;
		//	case RewardType.Gem:
		//		return amount <= UserDataService.UserData.UserGem;
		//	case RewardType.None:
		//		return false;

		//}
		//return false;
	}

	public bool IsAtlas(RewardType type, int contentId = 0)
	{
		throw new NotImplementedException();
		//switch (type)
		//{
		//	case RewardType.None:
		//		break;
		//	case RewardType.Product:
		//		return false;
		//	default:
		//		return true;
		//}
		//return true;
	}

	public bool IsCountable(RewardType type)
	{
		throw new NotImplementedException();
		//switch (type)
		//{
		//	case RewardType.None:
		//		break;
		//	case RewardType.Product:
		//		break;
		//	default:
		//		return true;
		//}
		//return false;
	}


	public RewardType GetRewardType(string enumString)
	{

		RewardType type = RewardType.None;
		if (string.IsNullOrEmpty(enumString))
		{
			Debug.LogWarning($"Empty Type");
			return type;
		}
		enumString = enumString.ToLower();
		if (Enum.TryParse<RewardType>(enumString, true, out type))
		{
			return type;
		}
		//just incase
		else if (enumString.Contains("cash"))
			type = RewardType.Cash;
		else if (enumString.Contains("gem"))
			type = RewardType.Gem;
		
		else
		{
			Debug.LogError($"CANNOT CONVERT RewardType: {enumString}");
		}
		return type;
	}

	public Sprite GetThumbnailSprite(RewardType type, int contentId = 0, int amount = 0, bool detail = false, bool bigIcon = false, bool oldIcon = false)
	{
		//GameManager.LogWarning($"get type: {type.ToString()}:{contentId}");
		RewardInfo ri = RewardInfoList.Where(x => x.Type == type && x.ContentId == contentId).FirstOrDefault();
		return GetThumbnailSprite(ri, amount, detail, bigIcon, oldIcon);
	}

	Sprite GetThumbnailSprite(RewardInfo ri, int amount = 0, bool detail = false, bool bigIcon = false, bool oldIcon = false)
	{
		//RewardInfo ri = RewardInfoList.Where(x => x.Type == rt).FirstOrDefault();
		if (ri == null)
		{
			Debug.LogWarning($"no sprite");
			return null;
		}
		Sprite sp = null;
		if (!detail || amount == 0)
		{
			//sp = bigIcon ? (ri.Icon_big != null ? ri.Icon_big : ri.Icon) : ri.Icon;

			sp = ri.Icon;

			if (bigIcon)
			{
				sp = (ri.Icon_big != null ? ri.Icon_big : ri.Icon);

			}
			else if (oldIcon)
			{
				sp = (ri.Old_Icon != null ? ri.Old_Icon : ri.Icon);
			}


			return sp;
		}
		switch (ri.Type)
		{
			case RewardType.Cash:
				if (amount <= 1000)
					sp = ri.IconDetails[0];
				else if (amount <= 10000)
					sp = ri.IconDetails[1];
				else
					sp = ri.IconDetails[2];
				break;
			case RewardType.Gem:
				if (amount <= 100)
					sp = ri.IconDetails[0];
				else if (amount <= 500)
					sp = ri.IconDetails[1];
				else if (amount <= 1200)
					sp = ri.IconDetails[2];
				else if (amount <= 2500)
					sp = ri.IconDetails[3];
				else if (amount <= 6500)
					sp = ri.IconDetails[4];
				else
					sp = ri.IconDetails[5];
				break;
			
		}
		return sp;
	}

	/// <summary>
	/// Handle memory leak yourself
	/// </summary>
	/// <param name="type"></param>
	/// <param name="contentId"></param>
	/// <param name="callBack"></param>

	public string GetDisplayName(RewardType rt, int content = 0, bool isPlural = true)
	{
		string name = string.Empty;
		switch (rt)
		{

			//case RewardType.Product:
			//	try
			//	{
			//		name = UserDataService.GetProduct(content).GetName();
			//	}
			//	catch (Exception ex)
			//	{
			//		GameManager.LogError($"content id: {content}");
			//		UnityEngine.Debug.LogError(ex);
			//	}
			//	break;
			//case RewardType.AssetPainterTool:
			//	name = GameController.Instance.AssetPainterService.GetTool(content).Name;
			//	break;
			default:
				RewardInfo ri = RewardInfoList.Where(x => x.Type == rt).FirstOrDefault();
				name = isPlural ? ri.Name_pl : ri.Name;
				break;
		}
		//Debug.Log($"name: {name}");
		return name;
	}

	//public ImageBindAction SetRewardImage(RewardData rd, bool detail = false, Image image = null, Text text = null, bool bigIcon = false, Action onFinish = null)
	//{
	//	SetRewardImage(rd, detail, image, text, null, bigIcon, onFinish);
	//}

	public void SetRewardImage(RewardData rd, bool detail = false, Image image = null, Text text = null, TMP_Text tmp_text = null, bool bigIcon = false, Action onFinish = null)
	{
		RewardInfo ri = RewardInfoList.Where(x => x.Type == rd.RewardType && x.ContentId == rd.ContentId).FirstOrDefault();
		if (ri != null)
		{
			if (tmp_text != null)
			{
				if (!IsCountable(rd.RewardType))
					tmp_text.gameObject.SetActive(false);
				tmp_text.text = $"{rd.Amount}";
			}
			if (text != null)
			{
				if (!IsCountable(rd.RewardType))
					text.gameObject.SetActive(false);
				text.text = $"{rd.Amount}";// {ri.Name_pl}";
			}
			if (image != null)
				image.sprite = GetThumbnailSprite(ri, rd.ContentId, detail, bigIcon);
			onFinish?.Invoke();
		}
		else
		{
			//if (tmp_text != null)
			//{
			//	if (!IsCountable(rd.RewardType))
			//		tmp_text.gameObject.SetActive(false);
			//	tmp_text.text = GetDisplayName(rd.RewardType, rd.ContentId);
			//}
			//if (text != null)
			//{
			//	if (!IsCountable(rd.RewardType))
			//		text.gameObject.SetActive(false);
			//	text.text = GetDisplayName(rd.RewardType, rd.ContentId);
			//}
			//if (image != null)
			//{
			//	imageBind = new ImageBindAction();
			//	int contentId = rd.ContentId == 0 ? rd.Amount : rd.ContentId;
			//	imageBind.OnValueChanged(image, GetThumbnailPath(rd.RewardType, contentId), (img, path) => { onFinish?.Invoke(); });
			//}
		}
	}
	/// <summary>
	/// Call OnUnRegister urself to free memory
	/// </summary>
	/// <param name="rt"></param>
	/// <param name="contentId"></param>
	/// <param name="amount"></param>
	/// <param name="detail"></param>
	/// <param name="image"></param>
	/// <param name="text"></param>
	/// <param name="onFinish"></param>
	/// <returns></returns>
	public void SetRewardImage(RewardType rt, int contentId = 0, int amount = 0, bool detail = false, Image image = null, Text text = null, bool bigIcon = false, Action onFinish = null)
	{
		throw new NotImplementedException();
		//return SetRewardImage(RewardData.CreateData(rt, amount, contentId), detail, image, text, null, bigIcon, onFinish);
	}

	/// <summary>
	/// this method doesn't handle non atlas images, use SetRewardImage if you want to handle automatically
	/// </summary>
	/// <param name="rt"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public RewardInfo GetReward(RewardType rt, int id = 0)
	{
		RewardInfo ri = RewardInfoList.Where(x => x.Type == rt).FirstOrDefault();
		if (ri == null)
		{
			Debug.LogWarning($"non support type {rt.ToString()}");
		}
		return ri;
	}

	public void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

		//if (!Application.isEditor)
		//    DontDestroyOnLoad(gameObject);

	}
}
