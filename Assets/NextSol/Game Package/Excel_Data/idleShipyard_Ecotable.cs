using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class idleShipyard_Ecotable : ScriptableObject
{
	public List<UpgradeSpeed> Upgrade_Speed; // Replace 'EntityType' to an actual type that is serializable.
	public List<UpgradeIncome> Upgrade_Income; // Replace 'EntityType' to an actual type that is serializable.
	public List<UpgradeWorker> Upgrade_Worker; // Replace 'EntityType' to an actual type that is serializable.
    public List<UpgradeSlot> Upgrade_Slot;
    public List<UpgradeSpeedTest> Upgrade_Speed_Test;
    public List<UpgradeIncomeTest> Upgrade_Income_Test;
    public List<CaptainData> Captain_Data;
    public List<UnlockData> Unlock_Data;
    public List<UnlockMap> Unlock_Map;
    public List<SessionRewardData> Session_Reward;
    public List<Constant> Constant;
}
