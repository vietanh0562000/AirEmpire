using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SaleManager : MonoBehaviour
{
    public static SaleManager Instance;
    public GameObject saleHouseObject;
    public GameObject prefabUpgrade;
    public GameObject panelUpgrade;
    public List<Sprite> listSpriteIcon;
    public List<Sprite> listSpritebackground;
    public List<string> listTitle;
    public List<int> listUpgradeLevel;
    public bool isOpen = false;
    public Button buttonClose;
    public List<GameObject> listUpgrade;

    public List<GameObject> listPosFirstZone;
    public List<GameObject> listPosParking;
    public List<GameObject> listPosSail;
    public List<GameObject> listPosTester;

    public int numParking = 0, totalParkingOpen = 1;

    public List<GameObject> listShipComPrefab;
    public GameObject shipParent;

    public List<GameObject> listLaunchShipObject;
    public List<Animator> listGateAnim;
    //public List<GameObject> listPushCarObject;

    public GameObject prefabPushShip;
    public GameObject panelEffect;

    public GameObject prefabWorkerCeleb;

    public GameObject prefabTester;
    public GameObject prefabCustomer;
    public Transform saleParent;
    public List<GameObject> listCustomerPathPos;

    [HideInInspector]
    public List<GameObject> listTester;

    public List<GameObject> listTestShipEffect;

    public List<Image> listBarrierProgress;

    public bool isParkingAvailable = true;

    public ParticleSystem effectUpgrade;

    public List<Tweener> listTestShipTweener;

    public List<ShipComController> listShipComplete;
    public bool[] listShipCompleteWait;
    public ShipComController currentShipComplete;
    public bool isCompleteTest = false;
    public float numSpeedBooster;

    // Start is called before the first frame update
    void Start()
    {
        numSpeedBooster = 1;
        Instance = this;
        InitSaleUpgrade();
        isParkingAvailable = true;
        listTestShipTweener = new List<Tweener>();
        listShipCompleteWait = new bool[] { false, false, false };

        if (DataManager.Instance.isParkingAvailable == 0)
        {
            Vector3 pos = listPosParking[0].transform.position;
            LoadShipCompletedTest(DataManager.Instance.currentSaleZone, pos);
        }

        for (int i = 0; i < 3; i++)
        {
            if (DataManager.Instance.listZoneIsAvailable[i + 3 * LoadingManager.Instance.currentMap] == 0)
            {
                Vector3 pos = listPosFirstZone[i].transform.position;
                LoadShipCompletedStop(i, pos);
            }
        }


        //InitShipCompleted(0);
        //InitShipCompleted(1);
        //InitShipCompleted(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCustomer(int numParking)
    {
        //Debug.Log("cus 1");
        //Debug.Log("cus 1" + prefabCustomer.name);
        //Debug.Log("cus 1" + saleParent.name);
        //Debug.Log("cus 1 ok");
        GameObject goCustomer = Instantiate(prefabCustomer, saleParent);
        //Debug.Log("cus 2");
        goCustomer.GetComponent<CustomerController>().numParking = numParking;
        //Debug.Log("cus 3");
    }

    public void InitTester(int numParking)
    {
        GameObject goTester = Instantiate(prefabTester, saleParent);
        goTester.transform.position = listPosTester[numParking].transform.position;
        listTester.Add(goTester);
    }

    public void InitShipCompleted(int zone)
    {
        GameObject goShip = Instantiate(listShipComPrefab[zone], shipParent.transform);
        goShip.transform.position = GameManager.Instance.listPosInstantiateShipWork[zone].position;
        goShip.GetComponent<ShipComController>().zone = zone;
        goShip.GetComponent<ShipComController>().SetShipGo(zone);
        listShipCompleteWait[zone] = true;
        //listShipComplete.Add(goShip.GetComponent<ShipComController>());
        //currentShipComplete = goShip.GetComponent<ShipComController>();
        StartCoroutine(CoWaitInitWorker(zone));
    }

    public void LoadShipCompletedTest(int zone, Vector3 pos)
    {
        GameObject goShip = Instantiate(listShipComPrefab[zone], shipParent.transform);
        goShip.transform.position = pos;
        goShip.GetComponent<ShipComController>().zone = zone;
        goShip.GetComponent<ShipComController>().SetProgressSale(0);
        goShip.transform.GetChild(1).gameObject.SetActive(false);
        goShip.GetComponent<ShipComController>().isStop = false;
        isCompleteTest = false;

        //listShipComplete.Add(goShip.GetComponent<ShipComController>());
        currentShipComplete = goShip.GetComponent<ShipComController>();
    }

    public void LoadShipCompletedStop(int zone, Vector3 pos)
    {
        GameObject goShip = Instantiate(listShipComPrefab[zone], shipParent.transform);
        goShip.transform.position = pos;
        goShip.GetComponent<ShipComController>().zone = zone;
        goShip.transform.GetChild(1).gameObject.SetActive(false);
        goShip.GetComponent<ShipComController>().isStop = true;
        goShip.GetComponent<ShipComController>().progressObject.SetActive(false);
        listShipCompleteWait[zone] = true;
    }

    IEnumerator CoWaitInitWorker(int zone)
    {
        for (int i = 0; i < 4; i++)
        {
            InitWorkerCeleb(zone, i);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }

    public void InitWorkerCeleb(int zone, int numWork)
    {
        GameObject goWorker = Instantiate(prefabWorkerCeleb, shipParent.transform);
        goWorker.GetComponent<WorkerCelebController>().workZone = zone;
        goWorker.GetComponent<WorkerCelebController>().numWork = numWork;
    }

    public void InitSaleUpgrade()
    {
        PopupUpgrade.Instance.textUpgradeSale.text = "x" + DataManager.Instance.numUpgrade;

        listUpgradeLevel.Add(DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap]);
        listUpgradeLevel.Add(DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap]);
        //listUpgradeLevel.Add(DataManager.Instance.levelSaleSlot);

        for (int i = 0; i < 2; i++)
        {
            GameObject goUpgrade = Instantiate(prefabUpgrade, panelUpgrade.transform);
            goUpgrade.transform.localPosition = new Vector3(0, -150 + 210 * i, 0);
            //goUpgrade.GetComponent<UpgradeController>().cost = (i + 1) * 10 * listUpgradeLevel[i];
            //goUpgrade.GetComponent<UpgradeController>().cost = (i + 1) * 10 * listUpgradeLevel[i];
            //goUpgrade.GetComponent<UpgradeController>().textCost.text = ((i + 1) * 1000 * listUpgradeLevel[i]).ToString();
            goUpgrade.GetComponent<UpgradeController>().icon.sprite = listSpriteIcon[i];
            goUpgrade.GetComponent<UpgradeController>().icon.SetNativeSize();
            goUpgrade.GetComponent<UpgradeController>().textTitle.text = listTitle[i];
            goUpgrade.GetComponent<UpgradeController>().textLevel.text = "Lv " + listUpgradeLevel[i].ToString();
            goUpgrade.GetComponent<UpgradeController>().idUpgrade = i;
            //if (i == 0)
            //{
            //    goUpgrade.GetComponent<UpgradeController>().cost = DataManager.Instance.upgradeDatas.Upgrade_Income_Test[DataManager.Instance.levelSalePrice].cost;
            //    goUpgrade.GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(goUpgrade.GetComponent<UpgradeController>().cost);
            //}
            //else if (i == 1)
            //{
            //    goUpgrade.GetComponent<UpgradeController>().cost = DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.levelSaleSpeed].cost;
            //    goUpgrade.GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(goUpgrade.GetComponent<UpgradeController>().cost);
            //}
            if (i == 2)
            {
                goUpgrade.GetComponent<UpgradeController>().progressObject.SetActive(true);
            }


            listUpgrade.Add(goUpgrade);
        }
        RefreshUI();
    }

    public void InitPushShip(GameObject goShip)
    {
        GameObject goPushShip = Instantiate(prefabPushShip, goShip.transform);
        goPushShip.transform.position = new Vector3(goShip.transform.position.x + 1f, 0, goShip.transform.position.z - 1);
    }

    public void RefreshUI()
    {
        for (int i = 0; i < listUpgrade.Count; i++)
        {
            listUpgrade[i].GetComponent<UpgradeController>().textLevel.text = "Lv " + listUpgradeLevel[i].ToString();
            listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.numUpgrade;
            listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + DataManager.Instance.numUpgrade + " lvl";

            if (i == 2)
            {
                listUpgrade[i].GetComponent<UpgradeController>().progressBar.fillAmount = (float)((listUpgradeLevel[i] - 1) % 5) / 5;
            }

            if (i == 0)
            {
                if (DataManager.Instance.numUpgrade > DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 - listUpgradeLevel[i])
                {
                    if (GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 - listUpgradeLevel[i], i) > 0)
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 - listUpgradeLevel[i], i);
                        listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 - listUpgradeLevel[i];
                        listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + (DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 - listUpgradeLevel[i]) + " lvl";
                    }
                    else
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i);
                    }
                }
                else
                {
                    if (GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i) > 0)
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i);
                    }
                    else
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i);
                    }
                }

                listUpgrade[i].GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(listUpgrade[i].GetComponent<UpgradeController>().cost);

                if (listUpgradeLevel[i] >= DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1)
                {
                    listUpgrade[i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(true);
                    listUpgrade[i].GetComponent<UpgradeController>().buttonUpgrade.gameObject.SetActive(false);
                }
                else
                {
                    listUpgrade[i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(listUpgrade[i].GetComponent<UpgradeController>().cost > GameUIManager.Instance.moneyTotal);
                    listUpgrade[i].GetComponent<UpgradeController>().buttonUpgrade.interactable = listUpgrade[i].GetComponent<UpgradeController>().cost <= GameUIManager.Instance.moneyTotal;
                }
            }
            else if (i == 1)
            {
                if (DataManager.Instance.numUpgrade > DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 - listUpgradeLevel[i])
                {
                    if (GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 - listUpgradeLevel[i], i) > 0)
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 - listUpgradeLevel[i], i);
                        listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 - listUpgradeLevel[i];
                        listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + (DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 - listUpgradeLevel[i]) + " lvl";
                    }
                    else
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i);
                    }
                }
                else
                {
                    if (GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i) > 0)
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetMaxUpgradeCost(DataManager.Instance.numUpgrade, i);
                    }
                    else
                    {
                        listUpgrade[i].GetComponent<UpgradeController>().cost = GetUpgradeCost(1, i);
                    }
                }

                listUpgrade[i].GetComponent<UpgradeController>().textCost.text = UnitConverter.Convert(listUpgrade[i].GetComponent<UpgradeController>().cost);

                if (listUpgradeLevel[i] >= DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1)
                {
                    listUpgrade[i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(true);
                    listUpgrade[i].GetComponent<UpgradeController>().buttonUpgrade.gameObject.SetActive(false);
                }
                else
                {
                    listUpgrade[i].GetComponent<UpgradeController>().imageDisabled.gameObject.SetActive(listUpgrade[i].GetComponent<UpgradeController>().cost > GameUIManager.Instance.moneyTotal);
                    listUpgrade[i].GetComponent<UpgradeController>().buttonUpgrade.interactable = listUpgrade[i].GetComponent<UpgradeController>().cost <= GameUIManager.Instance.moneyTotal;
                }
            } 
        }
    }

    public float GetUpgradeCost(int number, int i)
    {
        float total = 0;
        for (int k = 0; k < number; k++)
        {
            if (i == 0)
            {
                //total += DataManager.Instance.upgradeDatas.Upgrade_Income_Test[DataManager.Instance.levelSalePrice + k].cost;
                total += DataManager.Instance.listSalePriceCost[DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
            }
            else if (i == 1)
            {
                //total += DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.levelSalePrice + k].cost;
                total += DataManager.Instance.listSaleSpeedCost[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
            }
            else if (i == 2)
            {
                total += DataManager.Instance.upgradeDatas.Upgrade_Slot[DataManager.Instance.levelSaleSlot + k].cost;
            }
        }
        listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = number;
        listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + number + " lvl";
        return total;
    }

    public float GetMaxUpgradeCost(int number, int i)
    {
        float total = 0;
        for (int k = 0; k < number; k++)
        {
            if (i == 0)
            {
                if (k > 0)
                {
                    total += DataManager.Instance.listSalePriceCost[DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
                    if (total > GameUIManager.Instance.moneyTotal)
                    {
                        total -= DataManager.Instance.listSalePriceCost[DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
                        listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = k;
                        listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                        break;
                    }
                }
            }
            else if (i == 1)
            {
                total += DataManager.Instance.listSaleSpeedCost[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
                if (total > GameUIManager.Instance.moneyTotal)
                {
                    total -= DataManager.Instance.listSaleSpeedCost[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap] + k][LoadingManager.Instance.currentMap];
                    listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = k;
                    listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                    break;
                }
            }
            else if (i == 2)
            {
                total += DataManager.Instance.upgradeDatas.Upgrade_Slot[listUpgradeLevel[i] + k].cost;
                if (total > GameUIManager.Instance.moneyTotal)
                {
                    total -= DataManager.Instance.upgradeDatas.Upgrade_Slot[listUpgradeLevel[i] + k].cost;
                    listUpgrade[i].GetComponent<UpgradeController>().numUpgrade = k;
                    listUpgrade[i].GetComponent<UpgradeController>().textNumUpgrade.text = "x" + k + " lvl";
                    break;
                }
            }
        }

        return total;
    }

    public void UpgradeSpeedTest()
    {
        for (int i = 0; i < listTestShipTweener.Count; i++)
        {
            listTestShipTweener[i].DOTimeScale((DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap]].speed + 
                OperatorManager.Instance.listCaptains[3].boostSpeed) * numSpeedBooster, 0.1f).SetEase(Ease.Linear);
            Debug.Log(DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap]].speed);
        }
    }
}
