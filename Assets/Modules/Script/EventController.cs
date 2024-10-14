using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class EventController : MonoBehaviour, IGameEvent
{
    public static EventController Instance;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private List<IGameEvent> IEventList = new List<IGameEvent>();
   public void AddController(IGameEvent controller)
    {
        IEventList.Add(controller);
    }

	public void ReachLevel(int level)
	{
		System.Threading.Tasks.Task.Run(() =>
		   {
			   for (int i = 0; i < IEventList.Count; i++)
			   {
				   IEventList[i].ReachLevel(level);
			   }
		   });
	}

	public void EarnReward(RewardData rd)
	{
		System.Threading.Tasks.Task.Run(() =>
		{
			for (int i = 0; i < IEventList.Count; i++)
			{
				IEventList[i].EarnReward(rd);
			}
		});
	}

	public void SpendReward(RewardData rd)
	{
		System.Threading.Tasks.Task.Run(() =>
		{
			for (int i = 0; i < IEventList.Count; i++)
			{
				IEventList[i].SpendReward(rd);
			}
		});
	}
}
