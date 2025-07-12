using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public LevelDatas levelDatas;
    public GameObject prefabLevel;
    public GameObject panelLevel;
    public ScrollRect scrollRect;
    public List<LevelController> listLevel;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitAllMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameUIManager.Instance.popupMap.activeSelf)
        {
            RefreshLevelMap();
        }
    }

    public void InitAllMap()
    {
        scrollRect.verticalNormalizedPosition = 1;

        for (int i = 0; i < levelDatas.listLevelObject.Count; i++)
        {
            GameObject goLevel = Instantiate(prefabLevel, panelLevel.transform);
            goLevel.GetComponent<LevelController>().textName.text = levelDatas.listLevelObject[i].name;
            goLevel.GetComponent<LevelController>().levelIndex = i;
            goLevel.GetComponent<LevelController>().icon.sprite = levelDatas.listLevelObject[i].icon;
            goLevel.GetComponent<LevelController>().icon.SetNativeSize();
            goLevel.GetComponent<LevelController>().iconLock.sprite = levelDatas.listLevelObject[i].iconLock;
            goLevel.GetComponent<LevelController>().iconLock.SetNativeSize();
            if(i < 2)
            {
                goLevel.GetComponent<LevelController>().buttonOpen.interactable = true;
                goLevel.GetComponent<LevelController>().iconLock.gameObject.SetActive(false);
                goLevel.GetComponent<LevelController>().buttonUnlock.gameObject.SetActive(false);
                goLevel.GetComponent<LevelController>().objComingSoon.SetActive(false);
                goLevel.GetComponent<LevelController>().textUnlockCost.text = UnitConverter.Convert(DataManager.Instance.upgradeDatas.Unlock_Map[i].cost);
                //goLevel.GetComponent<LevelController>().imageLock.SetActive(false);
                //goLevel.GetComponent<LevelController>().currentLocated.SetActive(true);

            }
            else
            {
                goLevel.GetComponent<LevelController>().buttonOpen.interactable = false;
                goLevel.GetComponent<LevelController>().buttonUnlock.gameObject.SetActive(false);
                goLevel.GetComponent<LevelController>().iconLock.gameObject.SetActive(true);
                goLevel.GetComponent<LevelController>().objComingSoon.SetActive(true);
                //goLevel.GetComponent<LevelController>().imageLock.SetActive(true);
                //goLevel.GetComponent<LevelController>().currentLocated.SetActive(false);
            }
            listLevel.Add(goLevel.GetComponent<LevelController>());

        }
    }

    public void RefreshLevelMap()
    {
        for (int i = 0; i < listLevel.Count - 1; i++)
        {
            listLevel[i].buttonUnlock.interactable = GameUIManager.Instance.moneyTotal >= DataManager.Instance.upgradeDatas.Unlock_Map[i].cost;
            listLevel[i].imageDisable.gameObject.SetActive(GameUIManager.Instance.moneyTotal < DataManager.Instance.upgradeDatas.Unlock_Map[i].cost);
            listLevel[i].textMoneyPerSec.text = UnitConverter.Convert(DataManager.Instance.GetIncomeForMap(i)) + "/s";
            if (DataManager.Instance.listMapIsWaitToUnlock[i] == 1)
            {
                DataManager.Instance.listTimeUnlockCounted[i] += Time.deltaTime;
                listLevel[i].textTimeUnlock.text = UnitConverter.ConvertTime2(DataManager.Instance.upgradeDatas.Unlock_Map[i].time_unlock - (int)DataManager.Instance.listTimeUnlockCounted[i]);
                listLevel[i].progressUnlock.fillAmount = DataManager.Instance.listTimeUnlockCounted[i] / DataManager.Instance.upgradeDatas.Unlock_Map[i].time_unlock;
                if (DataManager.Instance.listTimeUnlockCounted[i] >= DataManager.Instance.upgradeDatas.Unlock_Map[i].time_unlock)
                {
                    DataManager.Instance.numMapUnlocked++;
                    listLevel[i].buttonOpen.gameObject.SetActive(true);
                    listLevel[i].progressUnlockObject.SetActive(false);
                    DataManager.Instance.listMapIsWaitToUnlock[i] = 0;
                }
            }
        }
    }

    public void LoadMapStatus()
    {
        for (int i = 0; i < listLevel.Count - 1; i++)
        {

            if (i == LoadingManager.Instance.currentMap)
            {
                listLevel[i].currentLocated.SetActive(true);
                listLevel[i].buttonOpen.gameObject.SetActive(false);
            }
            else
            {
                listLevel[i].currentLocated.SetActive(false);
                listLevel[i].buttonOpen.gameObject.SetActive(true);
            }
            listLevel[i].textMoneyPerSec.gameObject.SetActive(i < DataManager.Instance.numMapUnlocked);
            if (i > 0)
            {
                listLevel[i].buttonOpen.gameObject.SetActive(i < DataManager.Instance.numMapUnlocked && i != LoadingManager.Instance.currentMap);
                if (i >= DataManager.Instance.numMapUnlocked)
                {
                    listLevel[i].imageLockMap.SetActive(DataManager.Instance.listOperatorUnlock[2 + (i - 1) * 3] == 0);
                    listLevel[i].buttonUnlock.gameObject.SetActive(DataManager.Instance.listOperatorUnlock[2 + (i - 1) * 3] == 1);
                }
                else
                {
                    listLevel[i].imageLockMap.SetActive(false);
                    listLevel[i].buttonUnlock.gameObject.SetActive(false);
                }
                if (DataManager.Instance.listMapIsWaitToUnlock[i] == 1)
                {
                    DataManager.Instance.listTimeUnlockCounted[i] += UnitConverter.Offline(PlayerPrefs.GetString("exittime"));

                    if ((int)DataManager.Instance.listTimeUnlockCounted[i] >= DataManager.Instance.upgradeDatas.Unlock_Map[i].time_unlock)
                    {
                        DataManager.Instance.numMapUnlocked++;
                        listLevel[i].buttonOpen.gameObject.SetActive(true);
                        listLevel[i].progressUnlockObject.SetActive(false);
                        DataManager.Instance.listMapIsWaitToUnlock[i] = 0;
                    }
                    else
                    {
                        listLevel[i].progressUnlockObject.SetActive(true);
                        listLevel[i].buttonUnlock.gameObject.SetActive(false);
                        listLevel[i].textTimeAds.text = "-" + UnitConverter.ConvertTime(DataManager.Instance.upgradeDatas.Unlock_Map[i].time_ads);
                    }
                }
            }
            else if (i == 0)
            {
                listLevel[i].imageLockMap.SetActive(false);
                listLevel[i].buttonUnlock.gameObject.SetActive(false);
            }
        }
    }
}
