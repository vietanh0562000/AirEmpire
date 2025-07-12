using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public bool IS_HACK = false;
    public idleShipyard_Ecotable upgradeDatas;

    [HideInInspector]
    public int numLevel, statusLevelUp;

    [HideInInspector]
    public float currentExp;


// other data
    [HideInInspector]
    public bool isScene1;
    [HideInInspector]
    public int numUpgrade;


// setting data
    [HideInInspector]
    public int musicStatus, soundStatus, notiStatus;

// time data
    [HideInInspector]
    public string timeOnlineString;
    [HideInInspector]
    public float timeOnline;
    [HideInInspector]
    public bool isOnline;
    [HideInInspector]
    public int year, month, day;
    [HideInInspector]
    public string timeexit;

// session reward data
    [HideInInspector]
    public int numSessionRewardClaimed;
    [HideInInspector]
    public int isSessionRewardClaimed;
    [HideInInspector]
    public int[] listSessionClaim;

// Tutorial Data
    [HideInInspector]
    public int numTutorialStep;
    [HideInInspector]
    public int numStep1, numStep2;

// Map data
    [HideInInspector]
    public int totalMap = 2, currentMap;
    [HideInInspector]
    public int[] listLevelEarning, listLevelSpeed, listLevelWorker;
    [HideInInspector]
    public int[] listNumPartCompleted;
    [HideInInspector]
    public int[] listNumPartDecorTransported;
    [HideInInspector]
    public int[] listOperatorUnlock;
    [HideInInspector]
    public int[] listHireCaptain, listCaptainID;
    [HideInInspector]
    public int[] listParkingData;
    [HideInInspector]
    public int[] listZoneFinish;
    [HideInInspector]
    public int[] listZoneIsAvailable;
    [HideInInspector]
    public int numMapUnlocked;
    [HideInInspector]
    public int[] listMapIsWaitToUnlock;
    [HideInInspector]
    public float[] listTimeUnlockCounted;

// sale data
    [HideInInspector]
    public int levelSaleSlot, levelSaleSpeed, levelSalePrice;
    [HideInInspector]
    public int[] listLevelSaleSlot, listLevelSaleSpeed, listLevelSalePrice;
    [HideInInspector]
    public int isParkingAvailable;
    [HideInInspector]
    public int currentSaleZone;
    [HideInInspector]
    public float currentSaleTime;

// excel data
    [HideInInspector]
    public List<List<float>> listUpgradeIncomeCost, listUpgradeSpeedCost, listUpgradeWorkerCost;
    [HideInInspector]
    public List<List<float>> listUpgradeIncome;
    [HideInInspector]
    public List<List<float>> listSaleSpeedCost, listSalePriceCost, listSalePriceIncome;

    [HideInInspector]
    public int isMenuMapUnlocked;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        GetAllExcelData();

        totalMap = 2;
        listSessionClaim = new int[3];

        if (DateTime.Now.Year > GetYear() || DateTime.Now.Month > GetMonth() || DateTime.Now.Day > GetDay())
        {
            Debug.Log("New Day");
            timeOnline = 0;
            isSessionRewardClaimed = 0;
            numSessionRewardClaimed = 0;
            for (int i = 0; i < listSessionClaim.Length; i++)
            {
                listSessionClaim[i] = 0;
            }
            SetDay(DateTime.Now.Day);
            SetMonth(DateTime.Now.Month);
            SetYear(DateTime.Now.Year);
        }
        else
        {
            timeOnline = PlayerPrefs.GetFloat("timeOnline", 0);
            numSessionRewardClaimed = PlayerPrefs.GetInt("numSRClaimed", 0);
            isSessionRewardClaimed = PlayerPrefs.GetInt("isSRClaimed", 0);
            for (int i = 0; i < 3; i++)
            {
                listSessionClaim[i] = PlayerPrefs.GetInt("sessionClaim" + i, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isSessionRewardClaimed == 0)
        {
            timeOnline += Time.deltaTime;
        }
    }

    public void LoadData()
    {
        int totalShip = 3 * totalMap;
        listNumPartCompleted = new int[totalShip];
        listNumPartDecorTransported = new int[totalShip];
        listLevelEarning = new int[totalShip];
        listLevelSpeed = new int[totalShip];
        listLevelWorker = new int[totalShip];
        listOperatorUnlock = new int[totalShip];
        listParkingData = new int[totalShip];
        listHireCaptain = new int[4 * totalMap];
        listCaptainID = new int[4 * totalMap];
        listLevelSalePrice = new int[totalMap];
        listLevelSaleSpeed = new int[totalMap];
        listZoneFinish = new int[totalShip];
        listZoneIsAvailable = new int[totalShip];
        listMapIsWaitToUnlock = new int[totalMap];
        listTimeUnlockCounted = new float[totalMap];

        GameUIManager.Instance.moneyTotal = PlayerPrefs.GetFloat("moneyTotal", 0);
        //GameUIManager.Instance.moneyTotal = 100000000000000;

        GameUIManager.Instance.moneyPerPart = PlayerPrefs.GetFloat("moneyPerPart", 500);
        for (int i = 0; i < listNumPartCompleted.Length; i++)
        {
            listNumPartCompleted[i] = PlayerPrefs.GetInt("numPartCompleted" + i, 0);
        }
        for (int i = 0; i < listNumPartDecorTransported.Length; i++)
        {
            listNumPartDecorTransported[i] = PlayerPrefs.GetInt("numPartDecorCompleted" + i, 0);
        }

        for (int i = 0; i < listLevelEarning.Length; i++)
        {
            listLevelEarning[i] = PlayerPrefs.GetInt("levelEarning" + i, 1);
        }
        for (int i = 0; i < listLevelSpeed.Length; i++)
        {
            listLevelSpeed[i] = PlayerPrefs.GetInt("levelSpeed" + i, 1);
        }
        for (int i = 0; i < listLevelWorker.Length; i++)
        {
            listLevelWorker[i] = PlayerPrefs.GetInt("levelWorker" + i, 1);
        }

        for (int i = 1; i < listOperatorUnlock.Length; i++)
        {
            listOperatorUnlock[i] = PlayerPrefs.GetInt("operatorUnlock" + i, 0);
        }
        for (int i = 0; i < totalMap; i++)
        {
            listOperatorUnlock[0 + i * 3] = PlayerPrefs.GetInt("operatorUnlock" + (0 + i * 3), 1);
        }

        for (int i = 0; i < listHireCaptain.Length; i++)
        {
            listHireCaptain[i] = PlayerPrefs.GetInt("hireCaptain" + i, 0);
        }

        for (int i = 0; i < listCaptainID.Length; i++)
        {
            listCaptainID[i] = PlayerPrefs.GetInt("captainID" + i, 0);
        }
        //numPartCompleted = PlayerPrefs.GetInt("numPartCompleted", 0);
        //numPartDecorCompleted = PlayerPrefs.GetInt("numPartDecorTransported", 0);
        //levelEarning = PlayerPrefs.GetInt("levelEarning", 1);
        //levelSpeed = PlayerPrefs.GetInt("levelSpeed", 1);
        //levelWorker = PlayerPrefs.GetInt("levelWorker", 1);
        musicStatus = PlayerPrefs.GetInt("musicStatus", 1);
        soundStatus = PlayerPrefs.GetInt("soundStatus", 1);
        notiStatus = PlayerPrefs.GetInt("notiStatus", 1);
        numLevel = PlayerPrefs.GetInt("numLevel", 1);
        statusLevelUp = PlayerPrefs.GetInt("statusLevelUp", 0);
        currentExp = PlayerPrefs.GetFloat("currentExp", 0);

        levelSaleSlot = PlayerPrefs.GetInt("levelSaleSlot", 1);
        levelSaleSpeed = PlayerPrefs.GetInt("levelSaleSpeed", 1);
        levelSalePrice = PlayerPrefs.GetInt("levelSalePrice", 1);

        for (int i = 0; i < listLevelSaleSpeed.Length; i++)
        {
            listLevelSaleSpeed[i] = PlayerPrefs.GetInt("levelSaleSpeed" + i, 1);
        }

        for (int i = 0; i < listLevelSalePrice.Length; i++)
        {
            listLevelSalePrice[i] = PlayerPrefs.GetInt("levelSalePrice" + i, 1);
        }

        for (int i = 0; i < 3; i++)
        {
            listParkingData[i] = PlayerPrefs.GetInt("parkingSlot" + i, 0);
        }

        numUpgrade = PlayerPrefs.GetInt("numUpgrade", 1);

        numTutorialStep = PlayerPrefs.GetInt("tutorialStep", 0);
        numStep1 = PlayerPrefs.GetInt("numStep1", 0);
        numStep2 = PlayerPrefs.GetInt("numStep2", 0);

        isParkingAvailable = PlayerPrefs.GetInt("isParkingAvailable", 1);
        currentSaleZone = PlayerPrefs.GetInt("currentSaleZone", 0);
        currentSaleTime = PlayerPrefs.GetFloat("currentSaleTime", 0);
        for (int i = 0; i < listZoneFinish.Length; i++)
        {
            listZoneFinish[i] = PlayerPrefs.GetInt("zoneFinish" + i, 0);
        }
        for (int i = 0; i < listZoneIsAvailable.Length; i++)
        {
            listZoneIsAvailable[i] = PlayerPrefs.GetInt("zoneIsAvailable" + i, 1);
        }

        isMenuMapUnlocked = PlayerPrefs.GetInt("isMapUnlocked", 0);
        numMapUnlocked = PlayerPrefs.GetInt("numMapUnlocked", 1);

        for (int i = 0; i < listMapIsWaitToUnlock.Length; i++)
        {
            listMapIsWaitToUnlock[i] = PlayerPrefs.GetInt("isWaitToUnlock" + i, 0);
        }

        for (int i = 0; i < listTimeUnlockCounted.Length; i++)
        {
            listTimeUnlockCounted[i] = PlayerPrefs.GetFloat("timeUnlockCounted" + i, 0);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("moneyTotal", (float)GameUIManager.Instance.moneyTotal);
        PlayerPrefs.SetFloat("moneyPerPart", (float)GameUIManager.Instance.moneyPerPart);
        for (int i = 0; i < GameManager.Instance.listShipWorking.Length; i++)
        {
            if(GameManager.Instance.listShipWorking[i] != null)
            {
                PlayerPrefs.SetInt("numPartCompleted" + (i + 3 * LoadingManager.Instance.currentMap), GameManager.Instance.listShipWorking[i].GetComponent<ShipController>().numPartCompleted);
            }
        }
        for (int i = 0; i < GameManager.Instance.listNumPartDecorCompleted.Length; i++)
        {
            PlayerPrefs.SetInt("numPartDecorCompleted" + (i + 3 * LoadingManager.Instance.currentMap), GameManager.Instance.listNumPartDecorCompleted[i]);
        }

        for (int i = 0; i < listLevelEarning.Length; i++)
        {
            PlayerPrefs.SetInt("levelEarning" + i, listLevelEarning[i]);
        }
        for (int i = 0; i < listLevelSpeed.Length; i++)
        {
            PlayerPrefs.SetInt("levelSpeed" + i, listLevelSpeed[i]);
        }
        for (int i = 0; i < listLevelWorker.Length; i++)
        {
            PlayerPrefs.SetInt("levelWorker" + i, listLevelWorker[i]);
        }

        for (int i = 0; i < listOperatorUnlock.Length; i++)
        {
            PlayerPrefs.SetInt("operatorUnlock" + i, listOperatorUnlock[i]);
        }

        for (int i = 0; i < listSessionClaim.Length; i++)
        {
            PlayerPrefs.SetInt("sessionClaim" + i, listSessionClaim[i]);
        }

        for (int i = 0; i < listHireCaptain.Length; i++)
        {
            PlayerPrefs.SetInt("hireCaptain" + i, listHireCaptain[i]);
        }

        for (int i = 0; i < listCaptainID.Length; i++)
        {
            PlayerPrefs.SetInt("captainID" + i, listCaptainID[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("parkingSlot" + i, listParkingData[i]);
        }

        PlayerPrefs.SetInt("musicStatus", musicStatus);
        PlayerPrefs.SetInt("soundStatus", soundStatus);
        PlayerPrefs.SetInt("notiStatus", notiStatus);
        PlayerPrefs.SetInt("numLevel", numLevel);
        PlayerPrefs.SetInt("statusLevelUp", statusLevelUp);
        timeexit = DateTime.Now.ToString();
        PlayerPrefs.SetString("exittime", timeexit);
        currentExp = GameUIManager.Instance.imageLevelBar.fillAmount;
        PlayerPrefs.SetFloat("currentExp", currentExp);

        PlayerPrefs.SetInt("levelSaleSlot", levelSaleSlot);
        PlayerPrefs.SetInt("levelSaleSpeed", levelSaleSpeed);
        PlayerPrefs.SetInt("levelSalePrice", levelSalePrice);

        for (int i = 0; i < listLevelSaleSpeed.Length; i++)
        {
            PlayerPrefs.SetInt("levelSaleSpeed" + i, listLevelSaleSpeed[i]);
        }

        for (int i = 0; i < listLevelSalePrice.Length; i++)
        {
            PlayerPrefs.SetInt("levelSalePrice" + i, listLevelSalePrice[i]);
        }

        PlayerPrefs.SetFloat("timeOnline", timeOnline);
        PlayerPrefs.SetInt("numSRClaimed", numSessionRewardClaimed);
        PlayerPrefs.SetInt("isSRClaimed", isSessionRewardClaimed);

        PlayerPrefs.SetInt("numUpgrade", numUpgrade);
        PlayerPrefs.SetInt("tutorialStep", numTutorialStep);

        PlayerPrefs.SetInt("numStep1", numStep1);
        PlayerPrefs.SetInt("numStep2", numStep2);

        PlayerPrefs.SetInt("isParkingAvailable", isParkingAvailable);
        PlayerPrefs.SetInt("currentSaleZone", currentSaleZone);
        PlayerPrefs.SetFloat("currentSaleTime", currentSaleTime);

        for (int i = 0; i < listZoneFinish.Length; i++)
        {
            PlayerPrefs.SetInt("zoneFinish" + i, listZoneFinish[i]);
        }
        for (int i = 0; i < listZoneIsAvailable.Length; i++)
        {
            PlayerPrefs.SetInt("zoneIsAvailable" + i, listZoneIsAvailable[i]);
        }

        PlayerPrefs.SetInt("isMapUnlocked", isMenuMapUnlocked);
        PlayerPrefs.SetInt("numMapUnlocked", numMapUnlocked);

        for (int i = 0; i < listMapIsWaitToUnlock.Length; i++)
        {
            PlayerPrefs.SetInt("isWaitToUnlock" + i, listMapIsWaitToUnlock[i]);
        }

        for (int i = 0; i < listTimeUnlockCounted.Length; i++)
        {
            PlayerPrefs.SetFloat("timeUnlockCounted" + i, listTimeUnlockCounted[i]);
        }
    }

    public static void SetDay(int day)
    {
        PlayerPrefs.SetInt("day", day);
        //PlayerPrefs.Save();
    }

    public static int GetDay()
    {
        return PlayerPrefs.GetInt("day", 0);
    }

    public static void SetMonth(int month)
    {
        PlayerPrefs.SetInt("month", month);
        //PlayerPrefs.Save();
    }

    public static int GetMonth()
    {
        return PlayerPrefs.GetInt("month", 0);
    }

    public static void SetYear(int year)
    {
        PlayerPrefs.SetInt("year", year);
        //PlayerPrefs.Save();
    }

    public static int GetYear()
    {
        return PlayerPrefs.GetInt("year", 0);
    }

    public void GetAllExcelData()
    {
        listUpgradeIncomeCost = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Income.Count; i++)
        {
            listUpgradeIncomeCost.Add(new List<float>());
        }

        listUpgradeSpeedCost = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Speed.Count; i++)
        {
            listUpgradeSpeedCost.Add(new List<float>());
        }

        listUpgradeWorkerCost = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Worker.Count; i++)
        {
            listUpgradeWorkerCost.Add(new List<float>());
        }

        listUpgradeIncome = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Income.Count; i++)
        {
            listUpgradeIncome.Add(new List<float>());
        }

        listSaleSpeedCost = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Speed_Test.Count; i++)
        {
            listSaleSpeedCost.Add(new List<float>());
        }

        listSalePriceCost = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Income_Test.Count; i++)
        {
            listSalePriceCost.Add(new List<float>());
        }

        listSalePriceIncome = new List<List<float>>();
        for (int i = 0; i < upgradeDatas.Upgrade_Income_Test.Count; i++)
        {
            listSalePriceIncome.Add(new List<float>());
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Income.Count; i++)
        {
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost1);
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost2);
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost3);
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost4);
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost5);
            listUpgradeIncomeCost[i].Add(upgradeDatas.Upgrade_Income[i].cost6);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Speed.Count; i++)
        {
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost1);
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost2);
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost3);
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost4);
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost5);
            listUpgradeSpeedCost[i].Add(upgradeDatas.Upgrade_Speed[i].cost6);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Worker.Count; i++)
        {
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost1);
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost2);
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost3);
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost4);
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost5);
            listUpgradeWorkerCost[i].Add(upgradeDatas.Upgrade_Worker[i].cost6);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Income.Count; i++)
        {
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income1);
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income2);
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income3);
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income4);
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income5);
            listUpgradeIncome[i].Add(upgradeDatas.Upgrade_Income[i].income6);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Speed_Test.Count; i++)
        {
            listSaleSpeedCost[i].Add(upgradeDatas.Upgrade_Speed_Test[i].cost1);
            listSaleSpeedCost[i].Add(upgradeDatas.Upgrade_Speed_Test[i].cost2);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Income_Test.Count; i++)
        {
            listSalePriceCost[i].Add(upgradeDatas.Upgrade_Income_Test[i].cost1);
            listSalePriceCost[i].Add(upgradeDatas.Upgrade_Income_Test[i].cost2);
        }

        for (int i = 0; i < upgradeDatas.Upgrade_Income_Test.Count; i++)
        {
            listSalePriceIncome[i].Add(upgradeDatas.Upgrade_Income_Test[i].income1);
            listSalePriceIncome[i].Add(upgradeDatas.Upgrade_Income_Test[i].income2);
        }
    }

    public float GetIncomeForMap(int map)
    {
        float income = 0;
        income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[3 * map]][3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[3 * map]].speed);
        if (listOperatorUnlock[1 + 3 * map] == 1)
        {
            income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[1 + 3 * map]][1 + 3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[1 + 3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[1 + 3 * map]].speed);
        }
        if (listOperatorUnlock[2 + 3 * map] == 1)
        {
            income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[2 + 3 * map]][2 + 3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[2 + 3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[2 + 3 * map]].speed);
        }
        return income;
    }

    public float GetIncomeFromOtherMap(int map)
    {
        float income = 0;
        for (int i = 0; i < numMapUnlocked; i++)
        {
            income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[3 * i]][3 * i],
            upgradeDatas.Upgrade_Worker[listLevelWorker[3 * i]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[3 * i]].speed);
            if (listOperatorUnlock[1 + 3 * i] == 1)
            {
                income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[1 + 3 * i]][1 + 3 * i],
                upgradeDatas.Upgrade_Worker[listLevelWorker[1 + 3 * i]].worker,
                upgradeDatas.Upgrade_Speed[listLevelSpeed[1 + 3 * i]].speed);
            }
            if (listOperatorUnlock[2 + 3 * i] == 1)
            {
                income += GetFormulaIncome(listUpgradeIncome[listLevelEarning[2 + 3 * i]][2 + 3 * i],
                upgradeDatas.Upgrade_Worker[listLevelWorker[2 + 3 * i]].worker,
                upgradeDatas.Upgrade_Speed[listLevelSpeed[2 + 3 * i]].speed);
            }
        }

        income -= GetFormulaIncome(listUpgradeIncome[listLevelEarning[3 * map]][3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[3 * map]].speed);
        if (listOperatorUnlock[1 + 3 * map] == 1)
        {
            income -= GetFormulaIncome(listUpgradeIncome[listLevelEarning[1 + 3 * map]][1 + 3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[1 + 3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[1 + 3 * map]].speed);
        }
        if (listOperatorUnlock[2 + 3 * map] == 1)
        {
            income -= GetFormulaIncome(listUpgradeIncome[listLevelEarning[2 + 3 * map]][2 + 3 * map],
            upgradeDatas.Upgrade_Worker[listLevelWorker[2 + 3 * map]].worker,
            upgradeDatas.Upgrade_Speed[listLevelSpeed[2 + 3 * map]].speed);
        }
        return income;
    }

    public float GetFormulaIncome(float income, float worker, float speed)
    {
        return (income * (int)Mathf.Floor(worker) * speed) / 16;
    }

    private void OnApplicationQuit()
    {
        SaveData();
        LoadingManager.Instance.SaveData();
        SendEventFirebase.GameExit();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
            LoadingManager.Instance.SaveData();
        }
    }
}
