using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class UserModuleData
{
	public DateTime LastLoginDate = DateTime.MinValue;
	public int LogInDay = 0;

	[Newtonsoft.Json.JsonIgnore]
	public bool IsNewDay = false;
}

public class ModuleManager : MonoBehaviour
{
	public static ModuleManager Instance;

	public void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

	}

	internal UserModuleData UserModuleData;
	internal DateTime CurrentTime => DateTime.Now;
	private Coroutine SkipFrameCorutine;
	float GameTime = 0;
	Action OnGameTimeTick;
	float interval = 1f;
	void Start()
	{
		Debug.Log(Application.persistentDataPath);
		//RemoveCustomActivityView();
		SkipFrameCorutine = StartCoroutine(SkipFrameIEnumerator());
		UserModuleData = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModuleData>(PlayerPrefs.GetString("UserModuleData")) ?? new UserModuleData();
		Debug.Log(UserModuleData.LastLoginDate);
		UpdateUserModuleData();
		Debug.Log(UserModuleData.LastLoginDate);
		IAPManager.Instance.Init();
		ShopModule.Instance.Init(new OutletsRush_ShopModuleResponse());
	}

	void UpdateUserModuleData()
	{
		if (UserModuleData.LastLoginDate == DateTime.MinValue)//new player?
		{

		}
		else
		{
			if (CurrentTime.Date > UserModuleData.LastLoginDate.Date)
			{
				UserModuleData.LogInDay++;
				UserModuleData.IsNewDay = true;
			}
		}
		UserModuleData.LastLoginDate = CurrentTime;
		SaveUserModuleData();

	}

	public async void SaveUserModuleData()
	{

		string dString = await Task<string>.Run(() => Newtonsoft.Json.JsonConvert.SerializeObject(UserModuleData));
		PlayerPrefs.SetString("UserModuleData", dString);

	}

	class SkipFrameValues
	{
		public int frame;
		public Action callBack;
	}

	private List<SkipFrameValues> SkipFrameAction = new List<SkipFrameValues>();
	public void DelayFrame(int frame, Action callBack)
	{
		SkipFrameValues values = new SkipFrameValues();
		values.frame = frame;
		values.callBack = callBack;
		SkipFrameAction.Add(values);

	}
	private IEnumerator SkipFrameIEnumerator()
	{
		while (true)
		{
			var tempSkipFrameAction = new List<SkipFrameValues>(SkipFrameAction);
			for (int i = 0; i < tempSkipFrameAction.Count; i++)
			{
				SkipFrameValues action = tempSkipFrameAction[i];
				action.frame--;
				if (action.frame <= 0)
				{
					try
					{
						action.callBack();
					}
					catch (Exception ex)
					{
						Debug.LogError(ex.StackTrace);
					}
					SkipFrameAction.Remove(action);
					//Log("removed: " + SkipFrameAction.Count);
				}
			}

			yield return 0;

		}
	}

	/// <summary>
	/// Remember to call RemoveOnGameTick if finish
	/// </summary>
	/// <param name="callBack"></param>
	public void AddOnGameTick(Action callBack)
	{
		callBack();
		OnGameTimeTick += callBack;
	}

	public void RemoveOnGameTick(Action callBack)
	{
		OnGameTimeTick -= callBack;
	}

	void Update()
	{
		if (Time.realtimeSinceStartup >= GameTime)
		{
			if (OnGameTimeTick != null)
				OnGameTimeTick();
			GameTime += interval;

		}
	}

	public string GetDisplayTime(TimeSpan ts)
	{
		if (ts.TotalSeconds <= 0)
			return string.Empty;

		return string.Concat(
					ts.Days > 0 ? string.Format("{0}{1} ", ts.Days, "D") : "",
					ts.Hours > 0 ? string.Format("{0}{1} ", ts.Hours, "H") : "",
					ts.Days == 0 && ts.Minutes > 0 ? string.Format("{0}{1} ", ts.Minutes, "M") : "",
					ts.Days == 0 && ts.Hours == 0 && ts.Seconds > 0 ? string.Format("{0}{1}", ts.Seconds, "S") : "");
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Tools/ClearModuleData")]
#endif
	public static void ClearSave()
	{
		PlayerPrefs.DeleteKey("UserModuleData");
	}

}
