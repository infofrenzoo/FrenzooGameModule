using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    public void ReachLevel(int level);

    public void EarnReward(RewardData rd);

    public void SpendReward(RewardData rd);


}
