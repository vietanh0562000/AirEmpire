using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionReward", menuName = "SessionReward", order = 1)]
public class SessionRewardDatas : ScriptableObject
{
    public List<SessionRewardObject> listSessionReward;
}
