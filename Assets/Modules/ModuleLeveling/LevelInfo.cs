using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelInfo 
{
	public int LevelId;
	public int RequireXp;
	public int TotalXp;
	public List<RewardData> Rewards;
}
