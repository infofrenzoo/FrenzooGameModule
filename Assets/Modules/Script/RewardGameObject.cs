using UnityEngine;

public class RewardGameObject : MonoBehaviour
{
	public RewardType Type;
	public TMPro.TMP_Text Amount;

	public void SetRewardData(RewardData rd)
	{
		Amount.text = rd.Amount.ToString();
	}
}