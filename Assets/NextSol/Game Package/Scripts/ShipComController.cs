using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShipComController : MonoBehaviour
{
    public float speed = 1.5f;
    public int zone;
    public GameObject progressObject;
    public Image progressBar;
    //public float timeToTest;
    public List<Vector3> listSailPath;
    public bool isStop = false;
    public bool isTest = false;
    public bool isMessage = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(3).gameObject.SetActive(false);       
    }

    // Update is called once per frame
    void Update()
    {
        if (isTest)
        {
            DataManager.Instance.currentSaleTime = DataManager.Instance.upgradeDatas.Constant[0].testing_time * (1 - progressBar.fillAmount);         
        }

        if (isStop)
        {
            if (DataManager.Instance.isParkingAvailable == 1)
            {
                if(zone > 0)
                {
                    if(!SaleManager.Instance.listShipCompleteWait[zone - 1])
                    {
                        Debug.Log("test ship");
                        StartCoroutine(CoWaitShipSale(zone));
                        DataManager.Instance.isParkingAvailable = 0;
                        isStop = false;
                        SaleManager.Instance.listShipCompleteWait[zone] = false;
                        TutorialManager.Instance.timeAdaptiveMessAppear = 0;
                    }
                }
                else
                {
                    Debug.Log("test ship");
                    StartCoroutine(CoWaitShipSale(zone));
                    DataManager.Instance.isParkingAvailable = 0;
                    isStop = false;
                    SaleManager.Instance.listShipCompleteWait[zone] = false;
                    TutorialManager.Instance.timeAdaptiveMessAppear = 0;
                }

            }
            else
            {
                //Debug.Log(DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] + " " +
                //    (DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1));

                if (!SaleManager.Instance.isCompleteTest)
                {
                    if (AdsManager.Instance.capBoosterCompleteTest < 10)
                    {
                        AdsManager.Instance.ShowBoosterCompleteTest();
                    }
                }

                AdsManager.Instance.ShowBoosterX2SpeedTest();

                //if (DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap] <
                //    DataManager.Instance.upgradeDatas.Upgrade_Speed_Test.Count - 1 &&
                //    DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] <
                //    DataManager.Instance.upgradeDatas.Upgrade_Income_Test.Count - 1 && !isMessage)
                //{
                //    TutorialManager.Instance.timeAdaptiveMessAppear += Time.deltaTime;
                //    if (TutorialManager.Instance.timeAdaptiveMessAppear >= 15)
                //    {
                //        TutorialManager.Instance.ShowAdaptiveMess1();
                //        TutorialManager.Instance.timeAdaptiveMessAppear = 0;
                //        isMessage = true;
                //    }
                //}
            }          
        }
    }

    public float GetTimeTransport2Point(Vector3 start, Vector3 des)
    {
        float distance = Vector3.Distance(start, des);
        float time = distance / speed;
        return time;
    }

    public void SetShipGo(int zone)
    {
        progressObject.SetActive(false);
        StartCoroutine(CoWaitShipGo(zone));
    }

    IEnumerator CoWaitShipGo(int zone)
    {
        Debug.Log("ship go");
        gameObject.SetActive(true);
        float time1 = GetTimeTransport2Point(gameObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position);
        gameObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position, time1).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time1);
        float time2 = GetTimeTransport2Point(gameObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position);
        transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log("stop");
        isStop = true;

        //StartCoroutine(CoWaitBarrierProgress(zone, 4f));

        //yield return new WaitForSeconds(1f);
        //gameObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.43f, 0), 2).SetEase(Ease.Linear);
        //SaleManager.Instance.listLaunchShipObject[zone].transform.DOMove(SaleManager.Instance.listLaunchShipObject[zone].transform.position - new Vector3(0, 0.45f, 0), 2).SetEase(Ease.Linear);

        //yield return new WaitForSeconds(1.5f);
        //SaleManager.Instance.listGateAnim[zone].Play("up");

        //yield return new WaitForSeconds(1.5f);
        //Vector3 pos1 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, SaleManager.Instance.listPosParking[numPark].transform.position.z);
        //float time3 = GetTimeTransport2Point(gameObject.transform.position, pos1);
        //gameObject.transform.DOMove(pos1, time3).SetEase(Ease.Linear);

        //yield return new WaitForSeconds(time3 + 1);
        //SaleManager.Instance.listGateAnim[zone].Play("down");
        //SaleManager.Instance.listLaunchShipObject[zone].transform.DOMove(SaleManager.Instance.listLaunchShipObject[zone].transform.position + new Vector3(0, 0.45f, 0), 2).SetEase(Ease.Linear);
        //SaleManager.Instance.InitPushShip(gameObject);

        //yield return new WaitForSeconds(1);
        //Vector3 pos4 = new Vector3(SaleManager.Instance.listPosParking[numPark].transform.position.x, gameObject.transform.position.y, SaleManager.Instance.listPosParking[numPark].transform.position.z);
        //float time4 = GetTimeTransport2Point(gameObject.transform.position, pos4);
        //gameObject.transform.DOMove(pos4, time4).SetEase(Ease.Linear);

        //yield return new WaitForSeconds(time4);
        //transform.GetChild(4).gameObject.SetActive(false);
        //StartCoroutine(SetProgressBar(numPark, 350));
    }

    IEnumerator CoWaitShipSale(int zone)
    {
        Debug.Log("ship test");
        isStop = false;
        DataManager.Instance.currentSaleZone = zone;
        int numPark = 0;

        StartCoroutine(CoWaitBarrierProgress(zone, 4f));

        yield return new WaitForSeconds(1f);
        transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.43f, 0), 2).SetEase(Ease.Linear);
        SaleManager.Instance.listLaunchShipObject[zone].transform.DOMove(SaleManager.Instance.listLaunchShipObject[zone].transform.position - new Vector3(0, 0.45f, 0), 2).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.5f);
        SaleManager.Instance.listGateAnim[zone].Play("up");

        yield return new WaitForSeconds(1.5f);
        Vector3 pos1 = new Vector3(transform.position.x, transform.position.y, SaleManager.Instance.listPosParking[numPark].transform.position.z);
        float time3 = GetTimeTransport2Point(transform.position, pos1);
        transform.DOMove(pos1, time3).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time3 + 1);
        SaleManager.Instance.listGateAnim[zone].Play("down");
        SaleManager.Instance.listLaunchShipObject[zone].transform.DOMove(SaleManager.Instance.listLaunchShipObject[zone].transform.position + new Vector3(0, 0.45f, 0), 2).SetEase(Ease.Linear);
        SaleManager.Instance.InitPushShip(gameObject);

        yield return new WaitForSeconds(1);
        DataManager.Instance.listZoneIsAvailable[zone + 3 * LoadingManager.Instance.currentMap] = 1;

        Vector3 pos4 = new Vector3(SaleManager.Instance.listPosParking[numPark].transform.position.x, transform.position.y, SaleManager.Instance.listPosParking[numPark].transform.position.z);
        float time4 = GetTimeTransport2Point(transform.position, pos4);
        transform.DOMove(pos4, time4).SetEase(Ease.Linear);

        yield return new WaitForSeconds(time4);
        transform.GetChild(4).gameObject.SetActive(false);
        StartCoroutine(SetProgressBar(numPark, DataManager.Instance.upgradeDatas.Constant[0].testing_time));
    }

    IEnumerator CoWaitBarrierProgress(int zone, float delay)
    {
        SaleManager.Instance.listBarrierProgress[zone].transform.parent.gameObject.SetActive(true);
        SaleManager.Instance.listBarrierProgress[zone].fillAmount = 0;
        SaleManager.Instance.listBarrierProgress[zone].DOFillAmount(1, delay).SetEase(Ease.Linear);
        yield return new WaitForSeconds(delay);
        SaleManager.Instance.listBarrierProgress[zone].transform.parent.gameObject.SetActive(false);
    }

    public void SetProgressSale(int numParking)
    {
        StartCoroutine(SetProgressBar(numParking, DataManager.Instance.currentSaleTime));
    }

    IEnumerator SetProgressBar(int numParking, float timeToTest)
    {
        SaleManager.Instance.currentShipComplete = gameObject.GetComponent<ShipComController>();
        SaleManager.Instance.isCompleteTest = false;
        isTest = true;
        SaleManager.Instance.InitTester(numParking);
        SaleManager.Instance.listTestShipEffect[numParking].SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        progressObject.gameObject.SetActive(true);

        //timeToTest = DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.levelSaleSpeed].speed -
        //    DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.levelSaleSpeed].speed * OperatorManager.Instance.listCaptains[3].boostSpeed;
        //timeToTest = 350;

        progressBar.fillAmount = (DataManager.Instance.upgradeDatas.Constant[0].testing_time - timeToTest) / DataManager.Instance.upgradeDatas.Constant[0].testing_time;
        var tweener = progressBar.DOFillAmount(1, timeToTest).SetEase(Ease.Linear);
        SaleManager.Instance.listTestShipTweener.Add(tweener);
        Debug.Log("test time " + timeToTest);
        SaleManager.Instance.UpgradeSpeedTest();
        yield return tweener.WaitForCompletion();

        
        GoSail(numParking);
    }

    public void GoSail(int numParking)
    {
        StartCoroutine(CoWaitGoSail(numParking));
    }

    IEnumerator CoWaitGoSail(int numParking)
    {
        //Debug.Log("sail");
        SaleManager.Instance.isCompleteTest = true;
        //Debug.Log("sail 0.1");
        progressObject.gameObject.SetActive(false);
        //Debug.Log("sail 0.2");
        transform.GetChild(3).gameObject.SetActive(false);
        //Debug.Log("sail 0.3");
        SaleManager.Instance.listTestShipEffect[numParking].SetActive(false);
        //Debug.Log("sail 0.4");
        SaleManager.Instance.InitCustomer(numParking);
        //Debug.Log("sail 1");
        yield return new WaitForSeconds(GetTimeTransport2Point(SaleManager.Instance.listCustomerPathPos[0].transform.position,
            SaleManager.Instance.listCustomerPathPos[numParking + 1].transform.position));
        yield return new WaitForSeconds(2);
        Destroy(SaleManager.Instance.listTester[0]);

        isTest = false;
        listSailPath.Add(transform.position);
        for (int i = 0; i < SaleManager.Instance.listPosSail.Count; i++)
        {
            listSailPath.Add(new Vector3(SaleManager.Instance.listPosSail[i].transform.position.x, transform.position.y,
                SaleManager.Instance.listPosSail[i].transform.position.z));
        }

        var tweener = transform.DOPath(listSailPath.ToArray(), GetTimeTransport(listSailPath)).SetEase(Ease.Linear).SetLookAt(0.1f);
        InitMoney(gameObject);
        SaleManager.Instance.isParkingAvailable = true;
        SaleManager.Instance.numParking--;
        DataManager.Instance.isParkingAvailable = 1;
        yield return tweener.WaitForCompletion();

        Destroy(gameObject);
    }

    public void InitMoney(GameObject target)
    {
        GameObject go = PoolObjectManager.Instance.GetNextObjectPooling("money");
        go.transform.localScale = Vector3.one * 2;
        go.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        go.transform.GetChild(1).GetComponent<Text>().color = new Color(1, 1, 1, 1);
        //go.transform.GetChild(1).GetComponent<Text>().text = UnitConverter.Convert(DataManager.Instance.upgradeDatas.Upgrade_Income_Test[DataManager.Instance.levelSalePrice].income * (1 + OperatorManager.Instance.listCaptains[3].boostIncome));
        go.transform.GetChild(1).GetComponent<Text>().text = UnitConverter.Convert(DataManager.Instance.listSalePriceIncome[DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap]][LoadingManager.Instance.currentMap] * (1 + OperatorManager.Instance.listCaptains[3].boostIncome));
        go.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        go.SetActive(true);
        go.transform.DOMove(go.transform.position + new Vector3(0, 150, 0), 0.7f).SetEase(Ease.Linear);
        go.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.Linear);
        go.transform.GetChild(1).GetComponent<Text>().DOColor(new Color(1, 1, 1, 0), 1).SetEase(Ease.Linear);
        StartCoroutine(CoWaitObject(go));

        GameUIManager.Instance.AddMoney(DataManager.Instance.listSalePriceIncome[DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap]][LoadingManager.Instance.currentMap] * (1 + OperatorManager.Instance.listCaptains[3].boostIncome));

        SoundManager.Instance.PlayCollectClick();
    }

    IEnumerator CoWaitObject(GameObject go)
    {
        yield return new WaitForSeconds(1);
        go.SetActive(false);
    }

    public float GetDistancePath(List<Vector3> listPos)
    {
        float totalDistance = 0;
        for (int i = 0; i < listPos.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(listPos[i], listPos[i + 1]);
        }
        return totalDistance;
    }

    public float GetTimeTransport(List<Vector3> listPos)
    {
        return GetDistancePath(listPos) / speed;
    }
}
