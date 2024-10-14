using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ModuleButton : MonoBehaviour
{
	public BaseModule TargetModuleManager;
	public TMP_Text TimerText;
	public GameObject Badge;
	DateTime EndTime = DateTime.MinValue;
	Action TimerCallback;
	bool IsTicking = false;

	public void Start()
	{
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(() => { if (Badge != null) Badge.SetActive(false); TargetModuleManager.Panel.SetActive(true); });
		//SetTimer(TimeSpan.FromSeconds(10), ()=> { this.gameObject.SetActive(false); });
	}

	public void SetTimer(TimeSpan ts, Action callback = null)
	{
		EndTime = ModuleManager.Instance.CurrentTime.Add(ts);
		IsTicking = true;
		TimerCallback = callback;
		ModuleManager.Instance.AddOnGameTick(OnTick);
	}

	public void UpdateBadge(int count)
	{
		if (Badge != null) Badge.SetActive(count > 0);
	}

	void OnTick()
	{
		if (!IsTicking)
			return;
		TimeSpan ts = EndTime - ModuleManager.Instance.CurrentTime;
		
		TimerText.text = string.Format("{0}", ModuleManager.Instance.GetDisplayTime(ts));

		if (ts.TotalSeconds <= 0)
		{
			IsTicking = false;
			TimerCallback?.Invoke();
		}
	}


	private void OnDestroy()
	{
		ModuleManager.Instance.RemoveOnGameTick(OnTick);
	}
}
