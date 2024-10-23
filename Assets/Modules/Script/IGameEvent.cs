using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    public void OnEarnXP(int xp);

    public void OnReachLevel(int level);

    public void OnEarnReward(RewardData rd);

    public void OnSpendReward(RewardData rd);


}
