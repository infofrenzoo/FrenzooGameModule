using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RewardType
{
	None = 0,
	Cash = 1,
	Gem = 2,
	Speed = 3,
	DoubleCash = 4,
	//Booster = 3,
	//Ticket = 4,
	//Product = 5,
	//Furniture = 6,
	//Wallpaper = 7,
	//FloorTile = 8,
	//Look = 9,
	//Pose = 10,
	//Expression = 11,
	//Prop = 12,
	//Occasion = 13,
	//NailCoin = 14,
	//AssetPainterTool = 15,
	//AssetPainterSlot = 16,
	//GoldNailCoin = 17,
}

[Serializable]

public class RewardData
{

	public RewardType RewardType;
	
	public int Amount;
	public int ContentId = 0;

	public static RewardData CreateData(RewardType type, int amount = 1, int contentId = 0)
	{
		return new RewardData() { RewardType = type, Amount = amount, ContentId = contentId };
	}

	public override string ToString()
	{
		return $"{RewardType.ToString()}, amount:{Amount}, contentId:{ContentId}";
	}
}
