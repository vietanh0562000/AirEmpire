using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Jackal;
using UnityEngine.EventSystems;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    public GameObject cameraObject;
    public DataManager dataManager;
    public FormulaData formulaData;
    public GameObject canvasUIObject;
    public Text textMoneyTotal;
    public Text timex2goldcount;
    public Text timex3goldcount;
    public Text timex2speedcount;
    [HideInInspector]
    public double moneyTotal, moneyPerPart;

    [HideInInspector]
    public List<double> listIncome;

    private bool isSpeedUp = false;

    public List<GameObject> listUpgradeZone1, listUpgradeZone2, listUpgradeZone3;
    public List<List<GameObject>> listListUpgrade;
    public GameObject panelUpgrade;
    public GameObject prefabUpgrade;
    public GameObject bttx2speed;
    public GameObject bttx3gold;
    public GameObject bttUnlockNewShip;
    public GameObject canvasUI;
    public Button bttx2gold5min;
    public GameObject objcouttimex3;
    public GameObject objcouttimex2, effTimex2Active;
    public GameObject objcoutspeedx2;
    public List<Sprite> listSpriteIcon;
    public List<Sprite> listSpritebackground;

    public List<string> listTitle;
    public List<int> listUpgradeLevel;
    public List<List<int>> listListUpgradeLevel;

    public GameObject popupSetting, popupLevelUp, popupNoads, popupEarning, popupCaptain, popupUpgrade,
        popupSessionReward, popupExit, popupMap, popupShop, popupSaleUpgrade, popupManager, popupAds;

    public float timeCounter;
    public float timeLevelUp = 20;
    public Image imageLevelBar;
    public GameObject buttonLevelUp;
    public GameObject effectmoney;
    public Text textLevel;
    public Text textLevelReward;
    public Text offlineIncome;

    public List<GameObject> listMenuChosenObject, listMenuIconChosen;

    public GameObject panelEffect;
    public List<ParticleSystem> listUpgradeEffects;

    public List<GameObject> listCameraZonePos;

    public GameObject effCoinUI, effMoneyUI, effGiftBlastUI;

    public int numTap = 0;
    public bool isHideUI = false, isTap = false;
    public float timeTap;

    public GameObject buttonMapLock, textMapUnlocked;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        listTitle = new List<string> { "Earning", "Speed", "Workers" };
        listListUpgrade = new List<List<GameObject>>(3);
        listListUpgradeLevel = new List<List<int>>(3);
        listIncome = new List<double> { 0, 0, 0 };
        PoolObjectManager.Instance.StartPooling();
        DataManager.Instance.LoadData();
        InitAllUpgrade();
        ChosenMenu(2);

        if(DataManager.Instance.isMenuMapUnlocked == 1)
        {
            buttonMapLock.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePassiveIncome();

        moneyTotal += DataManager.Instance.GetIncomeFromOtherMap(LoadingManager.Instance.currentMap);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            popupExit.SetActive(true);
        }

        //if (isTap)
        //{
        //    timeTap += Time.deltaTime;
        //    if (timeTap > 0.2f)
        //    {
        //        timeTap = 0;
        //        numTap = 0;
        //        isTap = false;
        //    }
        //}

        //if (!IsMouseOverUI())
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (isTap)
        //        {
        //            if (timeTap < 0.2f)
        //            {
        //                if (isHideUI)
        //                {
        //                    canvasUIObject.SetActive(true);
        //                    isHideUI = false;
        //                }
        //                else
        //                {
        //                    canvasUIObject.SetActive(false);
        //                    isHideUI = true;
        //                }
        //            }
        //        }
        //        isTap = true;
        //    }
        //}
        
    }

    public List<float> listUpgradeSpeedCost, listUpgradeIncomeCost, listUpgradeWorkerCost;
    public void InitAllUpgrade()
    {
        PopupUpgrade.Instance.textUpgrade.text = "x" + DataManager.Instance.numUpgrade;
        textLevel.text = "Level " + DataManager.Instance.numLevel.ToString();
        textMoneyTotal.text = UnitConverter.Convert(moneyTotal);

        for (int i = 0; i < 3; i++)
        {
            listListUpgrade.Add(new List<GameObject>());
            listListUpgradeLevel.Add(new List<int>());
        }

        for (int j = 0; j < listListUpgrade.Count; j++)
        {
            listListUpgradeLevel[j].Add(DataManager.Instance.listLevelEarning[j + 3 * LoadingManager.Instance.currentMap]);
            listListUpgradeLevel[j].Add(DataManager.Instance.listLevelSpeed[j + 3 * LoadingManager.Instance.currentMap]);
            listListUpgradeLevel[j].Add(DataManager.Instance.listLevelWorker[j + 3 * LoadingManager.Instance.currentMap]);

            for (int i = 0; i < 3; i++)
            {
                GameObject goUpgrade = Instantiate(prefabUpgrade, PopupUpgrade.Instance.listUpgradePanel[j].transform);
                //goUpgrade.transform.localPosition = new Vector3(-350 + 350 * i, 0, 0);
                goUpgrade.transform.localPosition = new Vector3(0, -240 + 195 * i, 0);
                goUpgrade.GetComponent<UpgradeController>().zone = j;
                goUpgrade.GetComponent<UpgradeController>().idUpgrade = i;
                goUpgrade.GetComponent<UpgradeController>().icon.sprite = listSpriteIcon[i];
                //goUpgrade.GetComponent<UpgradeController>().imageBg.sprite = listSpritebackground[i];
                goUpgrade.GetComponent<UpgradeController>().icon.SetNativeSize();
                //goUpgrade.GetComponent<UpgradeController>().imageBg.SetNativeSize();
                goUpgrade.GetComponent<UpgradeController>().textTitle.text = listTitle[i];
                goUpgrade.GetComponent<UpgradeController>().textLevel.text = "Lv " + listListUpgradeLevel[j][i].ToString();
                if (i == 2) // worker
                {
                    goUpgrade.GetComponent<UpgradeController>().progressObject.SetActive(true);
                }
                listListUpgrade[j].Add(goUpgrade);
            }

            listIncome[j] = DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[j + 3 * LoadingManager.Instance.currentMap]][j + 3 * LoadingManager.Instance.currentMap];

        }

        //listIncome[0] = DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[0][0]].income1;
        //listIncome[1] = DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[1][0]].income2;
        //listIncome[2] = DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[2][0]].income3;

        RefreshUI();

        if (DataManager.Instance.statusLevelUp == 0)
        {
            buttonLevelUp.SetActive(false);
            ProgressLevelUp();
        }
        else
        {
            buttonLevelUp.SetActive(true);
        }
    }

    public void RefreshUI() {
        
        for (int j = 0; j < listListUpgrade.Count; j++)
        {
            for (int i = 0; i < listListUpgrade[j].Count; i++)
            {
                listListUpgrade[j][i].GetComponent<UpgradeController>().textLevel.text = "Lv " + listListUpgradeLevel[j][i].ToString();
                listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.numUpgrade;
                listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + DataManager.Instance.numUpgrade + " lvl";
                if (i == 2) // worker
                {
                    listListUpgrade[j][i].GetComponent<UpgradeController>().progressBar.fillAmount =
                        Mathf.Repeat(DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i]].worker, 1);

                    if (DataManager.Instance.numUpgrade > DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1 - listListUpgradeLevel[j][i])
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1 - listListUpgradeLevel[j][i], i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1 - listListUpgradeLevel[j][i], i, j);
                            listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1 - listListUpgradeLevel[j][i];
                            listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + (DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1 - listListUpgradeLevel[j][i]) + " lvl";
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }
                    else
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j);
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }

                    listListUpgrade[j][i].GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(listListUpgrade[j][i].GetComponent<UpgradeController>().cost);

                    if (listListUpgradeLevel[j][i] >= DataManager.Instance.upgradeDatas.Upgrade_Worker.Count - 1)
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(true);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageUp.gameObject.SetActive(false);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.gameObject.SetActive(false);
                    }
                    else
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(listListUpgrade[j][i].GetComponent<UpgradeController>().cost > moneyTotal);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.interactable = listListUpgrade[j][i].GetComponent<UpgradeController>().cost <= moneyTotal;
                    }
                }
                if (i == 0) // income
                {
                    if (DataManager.Instance.numUpgrade > DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1 - listListUpgradeLevel[j][i])
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1 - listListUpgradeLevel[j][i], i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1 - listListUpgradeLevel[j][i], i, j);
                            listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1 - listListUpgradeLevel[j][i];
                            listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + (DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1 - listListUpgradeLevel[j][i]) + " lvl";
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }
                    else
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j);
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }

                    listListUpgrade[j][i].GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(listListUpgrade[j][i].GetComponent<UpgradeController>().cost);

                    if (listListUpgradeLevel[j][i] >= DataManager.Instance.upgradeDatas.Upgrade_Income.Count - 1)
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(true);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageUp.gameObject.SetActive(false);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.gameObject.SetActive(false);
                        //listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.interactable = false;
                        //listListUpgrade[j][i].GetComponent<UpgradeController>().textCost.text = "MAXED";
                    }
                    else
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(listListUpgrade[j][i].GetComponent<UpgradeController>().cost > moneyTotal);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.interactable = listListUpgrade[j][i].GetComponent<UpgradeController>().cost <= moneyTotal;
                    }
                }
                if (i == 1) // speed
                {
                    if(DataManager.Instance.numUpgrade > DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1 - listListUpgradeLevel[j][i])
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1 - listListUpgradeLevel[j][i], i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1 - listListUpgradeLevel[j][i], i, j);
                            listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1 - listListUpgradeLevel[j][i];
                            listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + (DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1 - listListUpgradeLevel[j][i]) + " lvl";
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }
                    else
                    {
                        if (GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j) > 0)
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i, j);
                        }
                        else
                        {
                            listListUpgrade[j][i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i, j);
                        }
                    }

                    listListUpgrade[j][i].GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(listListUpgrade[j][i].GetComponent<UpgradeController>().cost);

                    if (listListUpgradeLevel[j][i] >= DataManager.Instance.upgradeDatas.Upgrade_Speed.Count - 1)
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(true);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageUp.gameObject.SetActive(false);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.gameObject.SetActive(false);
                        //listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.interactable = false;
                        //listListUpgrade[j][i].GetComponent<UpgradeController>().textCost.text = "MAXED";
                    }
                    else
                    {
                        listListUpgrade[j][i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(listListUpgrade[j][i].GetComponent<UpgradeController>().cost > moneyTotal);
                        listListUpgrade[j][i].GetComponent<UpgradeController>().buttonUpgrade.interactable = listListUpgrade[j][i].GetComponent<UpgradeController>().cost <= moneyTotal;
                    }
                }
            }
        }        
    }

    public void UpdatePassiveIncome()
    {
        //if (LoadingManager.Instance.currentMap == 0)
        //{          
        //    moneyTotal += DataManager.Instance.GetIncomeForMap(1) * Time.deltaTime;
        //}
        //else
        //{
        //    moneyTotal += DataManager.Instance.GetIncomeForMap(0) * Time.deltaTime;
        //}

        moneyTotal += GetTotalIncomeInAllMap();
    }

    public float GetTotalIncomeInAllMap()
    {
        float total = 0;
        for (int i = 0; i < DataManager.Instance.numMapUnlocked; i++)
        {
            if (i != LoadingManager.Instance.currentMap)
            {
                total += DataManager.Instance.GetIncomeForMap(i);
            }
        }
        return total;
    }

    public float GetUpgradeCost(int number, int i, int j)
    {
        float total = 0;
        for (int k = 0; k < number; k++)
        {
            if (i == 0)
            {
                //if (j == 0)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost1;
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost2;
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost3;
                //}
                total += DataManager.Instance.listUpgradeIncomeCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
            }
            else if (i == 1)
            {
                //if (j == 0)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost1;
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost2;
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost3;
                //}
                total += DataManager.Instance.listUpgradeSpeedCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
            }
            else if (i == 2)
            {
                //if (j == 0)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost1;
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost2;
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost3;
                //}
                total += DataManager.Instance.listUpgradeWorkerCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
            }
        }
        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = number;
        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + number + " lvl";
        return total;
    }

    public float GetMaxUpgradeCost(int number, int i, int j)
    {
        float total = 0;
        for (int k = 0; k < number; k++)
        {
            if (i == 0)
            {
                total += DataManager.Instance.listUpgradeIncomeCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                if (total > moneyTotal)
                {
                    total -= DataManager.Instance.listUpgradeIncomeCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                    listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                    listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                    break;
                }

                //if (j == 0)
                //{
                //    if(k > 0)
                //    {
                //        total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost1;
                //        if (total > moneyTotal)
                //        {
                //            total -= DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost1;
                //            listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //            listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";                            
                //            break;
                //        }
                //    }
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost2;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost2;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost3;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Income[listListUpgradeLevel[j][i] + k].cost3;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
            }
            else if (i == 1)
            {
                total += DataManager.Instance.listUpgradeSpeedCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                if (total > moneyTotal)
                {
                    total -= DataManager.Instance.listUpgradeSpeedCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                    listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                    listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                    break;
                }

                //if (j == 0)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost1;
                //    if (total > moneyTotal)
                //    {                       
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost1;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost2;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost2;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost3;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Speed[listListUpgradeLevel[j][i] + k].cost3;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
            }
            else if (i == 2)
            {
                total += DataManager.Instance.listUpgradeWorkerCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                if (total > moneyTotal)
                {
                    total -= DataManager.Instance.listUpgradeWorkerCost[listListUpgradeLevel[j][i] + k][j + 3 * LoadingManager.Instance.currentMap];
                    listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                    listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                    break;
                }

                //if (j == 0)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost1;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost1;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
                //else if (j == 1)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost2;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost2;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
                //else if (j == 2)
                //{
                //    total += DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost3;
                //    if (total > moneyTotal)
                //    {
                //        total -= DataManager.Instance.upgradeDatas.Upgrade_Worker[listListUpgradeLevel[j][i] + k].cost3;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().numUpgrade = k;
                //        listListUpgrade[j][i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                //        break;
                //    }
                //}
            }
        }

        return total;
    }

    public void AddMoney(double money)
    {
        moneyTotal += money;
        moneyTotal = Mathf.Clamp((float)moneyTotal, 0, float.MaxValue);
        //textMoneyTotal.text = UnitConverter.Convert(moneyTotal);
        if(moneyTotal < 1000)
        {
            textMoneyTotal.DOText(UnitConverter.Convert(moneyTotal), 0.5f, true, ScrambleMode.Numerals).SetEase(Ease.Linear);
        }
        else
        {
            textMoneyTotal.DOText(UnitConverter.Convert(moneyTotal), 0.5f).SetEase(Ease.Linear);
        }

        RefreshUI();
        SaleManager.Instance.RefreshUI();

        if (popupUpgrade.activeSelf)
        {
            PopupUpgrade.Instance.buttonUnlock.interactable = moneyTotal >= DataManager.Instance.upgradeDatas.Unlock_Data[PopupUpgrade.Instance.currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost;
            PopupUpgrade.Instance.imageLock.gameObject.SetActive(moneyTotal < DataManager.Instance.upgradeDatas.Unlock_Data[PopupUpgrade.Instance.currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost);

            for (int i = 1; i < 3; i++)
            {
                if (moneyTotal >= DataManager.Instance.upgradeDatas.Unlock_Data[i + 3 * LoadingManager.Instance.currentMap].cost)
                {
                    OperatorManager.Instance.listZone[i].isReadyUnlock = true;
                    OperatorManager.Instance.listZone[i].lockObject.GetComponent<Animator>().Play("lock_ready");
                }
            }
        }
    }

    public void SpendMoney(double money)
    {
        moneyTotal -= money;
        moneyTotal = Mathf.Clamp((float)moneyTotal, 0, float.MaxValue);
        //textMoneyTotal.text = UnitConverter.Convert(moneyTotal);
        if (moneyTotal < 1000)
        {
            textMoneyTotal.DOText(UnitConverter.Convert(moneyTotal), 0.5f, true, ScrambleMode.Numerals).SetEase(Ease.Linear);
        }
        else
        {
            textMoneyTotal.DOText(UnitConverter.Convert(moneyTotal), 0.5f).SetEase(Ease.Linear);
        }
        for (int i = 1; i < 3; i++)
        {
            if (moneyTotal < DataManager.Instance.upgradeDatas.Unlock_Data[i + 3 * LoadingManager.Instance.currentMap].cost)
            {
                OperatorManager.Instance.listZone[i].isReadyUnlock = false;
                OperatorManager.Instance.listZone[i].lockObject.GetComponent<Animator>().Play("idle");
            }
        }
    }

    public void InitMoney(GameObject target, int zone)
    {
        GameObject go = PoolObjectManager.Instance.GetNextObjectPooling("money");
        go.transform.localScale = Vector3.one;
        go.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        go.transform.GetChild(1).GetComponent<Text>().color = new Color(1, 1, 1, 1);

        //go.transform.GetChild(1).GetComponent<Text>().text = UnitConverter.Convert(moneyPerPart * GameManager.Instance.koeffmoney);
        go.transform.GetChild(1).GetComponent<Text>().text = UnitConverter.Convert(listIncome[zone] * (GameManager.Instance.koeffmoney + OperatorManager.Instance.listCaptains[zone].boostIncome));

        go.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        go.SetActive(true);
        go.transform.DOMove(go.transform.position + new Vector3(0, 150, 0), 0.7f).SetEase(Ease.Linear);
        go.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.Linear);
        go.transform.GetChild(1).GetComponent<Text>().DOColor(new Color(1, 1, 1, 0), 1).SetEase(Ease.Linear);
        StartCoroutine(CoWaitObject(go));

        //AddMoney(moneyPerPart * GameManager.Instance.koeffmoney);
        AddMoney(listIncome[zone] * (GameManager.Instance.koeffmoney + OperatorManager.Instance.listCaptains[zone].boostIncome));
        SoundManager.Instance.PlayCollectClick();
    }

    IEnumerator CoWaitObject(GameObject go)
    {
        yield return new WaitForSeconds(1);
        go.SetActive(false);
    }
    
    public void OnClickUpgradeEarning()
    {
        moneyPerPart += 10;
    }

    public void OnClickUpgradeSpeed()
    {
        if (!isSpeedUp)
        {
            Time.timeScale = 2;
            isSpeedUp = true;
        }
        else
        {
            Time.timeScale = 1;
            isSpeedUp = false;
        }
    }

    public float FindForHighestCost()
    {
        float highestCost = (float)listListUpgrade[0][0].GetComponent<UpgradeController>().cost;
        for (int i = 0; i < listListUpgrade.Count; i++)
        {
            for (int j = 0; j < listListUpgrade[i].Count; j++)
            {
                if (listListUpgrade[i][j].GetComponent<UpgradeController>().cost > highestCost)
                {
                    highestCost = (float)listListUpgrade[i][j].GetComponent<UpgradeController>().cost;
                }
            }
        }
        return highestCost;
    }

    public void OnClickUpgradeWorkers()
    {
        //GameManager.Instance.SetUpWorker();
    }

    public void OnClickUnlockNewShip()
    {
        //for (int i = 0; i < GameManager.Instance.listShipObject.Count; i++)
        //{
        //    GameManager.Instance.listShipObject[i].SetActive(false);
        //}
        //PlayerPrefsController.shipSelect++;
        //GameManager.Instance.numPartCompleted = 0;
        //GameManager.Instance.numPartTransported = 0;
        //GameManager.Instance.indexDecor = 0;
        //dataManager.SaveData();
        //SceneManager.LoadSceneAsync(1);
        //GameManager.Instance.effectSmokeUnlock.GetComponent<ParticleSystem>().Play();
        //GameManager.Instance.effConfettiFinish.SetActive(true);

        SceneManager.LoadSceneAsync(2);
    }

    public void OnClickSetting()
    {
        popupSetting.SetActive(true);
    }

    public void ProgressLevelUp()
    {
        float time = (1 - DataManager.Instance.currentExp) * (2 * DataManager.Instance.numLevel - 1);
        imageLevelBar.fillAmount = DataManager.Instance.currentExp;
        var tweener = imageLevelBar.DOFillAmount(1, time).SetEase(Ease.Linear);
        StartCoroutine(CoWaitLevelUp(tweener));
    }

    IEnumerator CoWaitLevelUp(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        buttonLevelUp.SetActive(true);
        DataManager.Instance.statusLevelUp = 1;
    }

    public void OnClickLevelUp()
    {
        int level = DataManager.Instance.numLevel + 1;
        textLevel.text = "Level " + level.ToString();
        textLevelReward.text = UnitConverter.Convert(1000 * Mathf.Pow(DataManager.Instance.numLevel - 1, 1.5f) + 500);
        DataManager.Instance.currentExp = 0;
        imageLevelBar.fillAmount = 0;
        buttonLevelUp.SetActive(false);
        popupLevelUp.SetActive(true);
        //SdkNextsolManager.Instance.fbManager.LogLevel_achievedEvent();
    }

    public void ChosenMenu(int index)
    {
        for (int i = 0; i < listMenuChosenObject.Count; i++)
        {
            listMenuChosenObject[i].SetActive(false);
            listMenuIconChosen[i].transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
        listMenuChosenObject[index].SetActive(true);
        listMenuIconChosen[index].transform.localScale = Vector3.one;
        InactiveAllMenu();
    }

    public void OnClickShop()
    {
        if (DataManager.Instance.numTutorialStep > 2)
        {
            ChosenMenu(0);
            popupShop.SetActive(true);
        }
    }

    public void OnClickManager()
    {
        if(DataManager.Instance.numTutorialStep > 2)
        {
            ChosenMenu(1);
            popupManager.SetActive(true);
        }
    }

    public void OnClickHome()
    {
        ChosenMenu(2);
    }

    public void OnClickMap()
    {
        if (DataManager.Instance.numTutorialStep > 2)
        {
            ChosenMenu(3);
            popupMap.SetActive(true);
            MapManager.Instance.scrollRect.verticalNormalizedPosition = 0;
            MapManager.Instance.LoadMapStatus();
        }
    }

    public void OnClickMenu()
    {
        if (DataManager.Instance.numTutorialStep > 2)
        {
            ChosenMenu(4);
            popupSetting.SetActive(true);
        }
    }

    public void InactiveAllMenu()
    {
        popupSetting.SetActive(false);
        popupMap.SetActive(false);
        popupShop.SetActive(false);
        popupManager.SetActive(false);
    }

    public void OnClickSessionReward()
    {
        popupSessionReward.SetActive(true);
        PopupSessionReward.Instance.SetAnimIcon();
    }

    public void OnClickCheatMoney(float money)
    {
        AddMoney(money);
    }

    public void OnClickCheatMoneyZero()
    {
        moneyTotal = 0;
        textMoneyTotal.text = UnitConverter.Convert(moneyTotal);
        RefreshUI();
        SaleManager.Instance.RefreshUI();
    }

    public void OnClickHideUI()
    {
        canvasUIObject.SetActive(false);
    }

    public void ChangeCameraZonePos(int zone)
    {
        cameraObject.transform.DOLocalMove(listCameraZonePos[zone].transform.localPosition, 0.3f).SetEase(Ease.Linear);
        cameraObject.GetComponent<Camera>().DOOrthoSize(7, 0.3f).SetEase(Ease.Linear);
    }


    #region Ads
    public void Adsx2Speed()
    {
        popupAds.SetActive(false);
        x2SpeedUpMissle();
        //SendEventFirebase.ClickOnAdsEvent("ads_reward_double_speed", "Main Screen");
    }
    public void Adsx2Gold()
    {
        popupAds.SetActive(false);
        x2Gold();
        //SendEventFirebase.ClickOnAdsEvent("ads_reward_double_income", "Main Screen");
    }
    public void Adsx3Gold()
    {
        popupAds.SetActive(false);
        x3Gold();
    }
    public void FailToLoadAds()
    {
        popupNoads.SetActive(true);
        Invoke("AutoHidenPopup", 3f);
    }
    public void AutoHidenPopup()
    {
        GameUIManager.Instance.popupNoads.SetActive(false);
    }
    public void x2SpeedUpMissle()
    {
        //GameManager.Instance.isbosterspeedup = true;
        //GameManager.Instance.timebosterspeedup += 60;
        //bttx2speed.SetActive(false);
        //bttx2speed.transform.localPosition = bttx2speed.transform.localPosition - new Vector3(300, 0, 0);
        //GameManager.Instance.timeshownextspeedup = Time.time + 310;


        
        AdsManager.Instance.ActivateBoosterSpeed();
    }
    public void x2Gold()
    {
        //GameManager.Instance.isx2goldbosterup = true;
        //GameManager.Instance.timex2goldspeedup += 300;
        //bttx2gold5min.interactable = false;
        //StartCoroutine(ReactiveX2Gold5Min(300, bttx2gold5min));

        AdsManager.Instance.ActivateBoosterIncome();
    }
    public void x3Gold()
    {
        //GameManager.Instance.isx3goldbosterup = true;
        //GameManager.Instance.timex3goldspeedup += 300;
        //bttx3gold.SetActive(false);
        //bttx3gold.transform.localPosition = bttx3gold.transform.localPosition + new Vector3(300, 0, 0);
        ////bttx3gold.transform.DOMove(bttx3gold.transform.position + new Vector3(300, 0, 0), 0.5f).SetEase(Ease.Linear).OnComplete(() => bttx3gold.SetActive(false));
        //GameManager.Instance.timeshownextx3 = Time.time + 310;
    }


    IEnumerator ReactiveX2Gold5Min(float time,Button button)
    {
        yield return new WaitForSeconds(time);
        button.interactable = true;
    }
    #endregion
}
