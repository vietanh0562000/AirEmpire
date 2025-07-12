using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    [Header("---Booster Speed---")]
    public GameObject speedBoosterObject;
    public bool isSpeedAppear = false;
    public float timeSpeedBoosterWait, timeSpeedBoosterApeear, timeSpeedActivated;
    public bool isBoosterSpeedActivated = false;
    public GameObject timeSpeedActivatedObject;
    public Text textTimeSpeedCount;

    [Header("---Booster Income---")]
    public GameObject incomeBoosterObject;
    public Text textIncomeTime;
    public bool isIncomeAppear = false;
    public float timeIncomeBoosterWait, timeIncomeBoosterAppear, timeIncomeActivated;
    public bool isBoosterIncome5mActivated = false, isBoosterIncome15mActivated = false, isBoosterIncome60mActivated = false, isBoosterIncomeActivated = false;
    public GameObject timeIncomeActivatedObject;
    public Text textTimeIncomeCount;
    public int numBoosterIncomeActive;

    [Header("---Angel Investor---")]
    public GameObject investorAdsObject;
    public List<GameObject> listPos;
    public List<Vector3> listPath;
    public bool isInvestorAppear = false;
    public float timeInvestorWait;

    [Header("---Booster Complete Test---")]
    public GameObject completeTestBoosterObject;
    public bool isCompleteTestAppear = false;
    public float timeBoosterCompleteTestWait, timeBoosterCompleteTestAppear;
    public bool isBoosterCompleteTestActivated = false;
    public int capBoosterCompleteTest = 0;
    public float timeCapBoosterCompleteTest;

    [Header("---Booster Speed Test")]
    public GameObject speedTestBoosterObject;
    public bool isSpeedTestAppear = false;
    public float timeBoosterSpeedTestWait, timeBoosterSpeedTestAppear, timeSpeedTestActivated;
    public bool isBoosterSpeedTestActivated = false;
    public GameObject timeSpeedTestActivatedObject;
    public Text textTimeSpeedTestCount;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        incomeBoosterObject.GetComponent<Animator>().enabled = false;
        speedBoosterObject.GetComponent<Animator>().enabled = false;
        completeTestBoosterObject.GetComponent<Animator>().enabled = false;
        speedTestBoosterObject.GetComponent<Animator>().enabled = false;
        timeSpeedBoosterWait = 5;
        numBoosterIncomeActive = 5;
        timeInvestorWait = 15;

        for (int i = 0; i < listPos.Count; i++)
        {
            listPath.Add(listPos[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpeedAppear)
        {
            ShowBoosterIncome();
        }
        if (!isIncomeAppear)
        {
            ShowBoosterSpeed();
        }
        ShowAngelInsvestor();

        if (isBoosterSpeedActivated)
        {
            timeSpeedActivated -= Time.deltaTime;
            if (timeSpeedActivated > 0)
            {
                float minutes = Mathf.FloorToInt(timeSpeedActivated / 60);
                float seconds = Mathf.FloorToInt(timeSpeedActivated % 60);
                textTimeSpeedCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                isBoosterSpeedActivated = false;
                foreach (GameObject workerA in GameManager.Instance.listWorker)
                {
                    workerA.GetComponent<WorkerController>().trail.SetActive(false);
                }
                GameManager.Instance.numSpeedBooster = 0;
                timeSpeedActivatedObject.SetActive(false);
                GameManager.Instance.UpgradeSpeedForZone(0);
                GameManager.Instance.UpgradeSpeedForZone(1);
                GameManager.Instance.UpgradeSpeedForZone(2);
            }
        }

        if (isBoosterIncomeActivated)
        {
            timeIncomeActivated -= Time.deltaTime;
            if (timeIncomeActivated > 0)
            {
                float minutes = Mathf.FloorToInt(timeIncomeActivated / 60);
                float seconds = Mathf.FloorToInt(timeIncomeActivated % 60);
                textTimeIncomeCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                if(timeIncomeActivated < 5 * 60)
                {
                    isBoosterIncome15mActivated = false;
                }
                if(timeIncomeActivated > 5 * 60)
                {
                    isBoosterIncome60mActivated = false;
                }
            }
            else
            {
                isBoosterIncomeActivated = false;
                isBoosterIncome5mActivated = false;
                isBoosterIncome15mActivated = false;
                isBoosterIncome60mActivated = false;
                GameManager.Instance.koeffmoney = 1;
                timeIncomeActivatedObject.SetActive(false);
            }
        }

        if (isBoosterSpeedTestActivated)
        {
            timeSpeedTestActivated -= Time.deltaTime;
            if (timeSpeedTestActivated > 0)
            {
                float minutes = Mathf.FloorToInt(timeSpeedTestActivated / 60);
                float seconds = Mathf.FloorToInt(timeSpeedTestActivated % 60);
                textTimeSpeedTestCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                isBoosterSpeedActivated = false;
                timeSpeedTestActivatedObject.SetActive(false);
                SaleManager.Instance.numSpeedBooster = 1;
                SaleManager.Instance.UpgradeSpeedTest();
                timeBoosterSpeedTestWait = -15 * 60;
            }
        }

        timeCapBoosterCompleteTest += Time.deltaTime;
        if(timeCapBoosterCompleteTest > 3600)
        {
            capBoosterCompleteTest = 0;
            timeCapBoosterCompleteTest = 0;
        }

       
        
    }

    public void ActivateBoosterSpeed()
    {
        isBoosterSpeedActivated = true;
        timeSpeedActivated = 60;
        isSpeedAppear = false;

        foreach (GameObject workerA in GameManager.Instance.listWorker)
        {
            workerA.GetComponent<WorkerController>().trail.SetActive(true);
        }
        timeSpeedActivatedObject.SetActive(true);
        GameManager.Instance.numSpeedBooster = 1;
        GameManager.Instance.UpgradeSpeedForZone(0);
        GameManager.Instance.UpgradeSpeedForZone(1);
        GameManager.Instance.UpgradeSpeedForZone(2);
        BoosterReturn(speedBoosterObject);
    }

    public void ActivateBoosterIncome()
    {
        isBoosterIncomeActivated = true;
        timeIncomeActivatedObject.SetActive(true);

        if (!isBoosterIncome5mActivated)
        {
            timeIncomeActivated = 5 * 60;
            isBoosterIncome5mActivated = true;
        }
        else
        {
            if (!isBoosterIncome15mActivated)
            {
                timeIncomeActivated = 15 * 60;
                isBoosterIncome15mActivated = true;
            }
            else
            {
                timeIncomeActivated = 60 * 60;
                isBoosterIncome60mActivated = true;
            }
        }
        isIncomeAppear = false;
        GameManager.Instance.koeffmoney = 2;
        BoosterReturn(incomeBoosterObject);
    }

    public void ShowBoosterSpeed()
    {
        if (!isBoosterSpeedActivated)
        {
            if (!isSpeedAppear)
            {
                timeSpeedBoosterWait += Time.deltaTime;
                if (timeSpeedBoosterWait > 10)
                {
                    BoosterAppear(speedBoosterObject);
                    timeSpeedBoosterWait = 0;
                    isSpeedAppear = true;
                }
            }
            else
            {
                timeSpeedBoosterApeear += Time.deltaTime;
                if (timeSpeedBoosterApeear > 15)
                {
                    BoosterReturn(speedBoosterObject);
                    timeSpeedBoosterApeear = 0;
                    isSpeedAppear = false;
                }
            }
        }
    }

    public void ShowBoosterIncome()
    {
        if (!isBoosterIncome60mActivated)
        {
            if (!isIncomeAppear)
            {
                timeIncomeBoosterWait += Time.deltaTime;
                if (timeIncomeBoosterWait > Random.Range(5,10))
                {
                    BoosterAppear(incomeBoosterObject);
                    if (!isBoosterIncome5mActivated)
                    {
                        numBoosterIncomeActive = 5;
                    }
                    else
                    {
                        if (!isBoosterIncome15mActivated)
                        {
                            numBoosterIncomeActive = 15;
                        }
                        else
                        {
                            numBoosterIncomeActive = 60;
                        }
                    }
                    textIncomeTime.text = numBoosterIncomeActive + "m";
                    timeIncomeBoosterWait = 0;
                    isIncomeAppear = true;
                }
            }
            else
            {
                timeIncomeBoosterAppear += Time.deltaTime;
                if (timeIncomeBoosterAppear > 15)
                {
                    BoosterReturn(incomeBoosterObject);
                    timeIncomeBoosterAppear = 0;
                    isIncomeAppear = false;
                }
            }
        }
    }

    public void ShowAngelInsvestor()
    {
        if (!isInvestorAppear)
        {
            timeInvestorWait += Time.deltaTime;
            if (timeInvestorWait > 120)
            {
                investorAdsObject.SetActive(true);
                investorAdsObject.transform.position = listPath[0];
                investorAdsObject.transform.DOPath(listPath.ToArray(), 15, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => InvestorDisappear());
                timeInvestorWait = 0;
                isInvestorAppear = true;
            }
        }
    }

    public void InvestorDisappear()
    {
        investorAdsObject.SetActive(false);
        isInvestorAppear = false;
    }

    public void BoosterAppear(GameObject goBooster)
    {
        goBooster.transform.DOLocalMove(goBooster.transform.localPosition + new Vector3(350, 0, 0), 0.5f)
            .SetEase(Ease.Linear).OnComplete(() => goBooster.GetComponent<Animator>().enabled = true);
    }

    public void BoosterReturn(GameObject goBooster)
    {
        goBooster.GetComponent<Animator>().enabled = false;
        goBooster.transform.GetChild(0).transform.localPosition = Vector3.zero;
        goBooster.transform.DOLocalMove(goBooster.transform.localPosition - new Vector3(350, 0, 0), 0.5f).SetEase(Ease.Linear);
    }

    public void ActivateBoosterCompleteShipTest()
    {
        //timeBoosterCompleteTestWait = 0;
        //isCompleteTestAppear = false;
        ////isBoosterCompleteTestActivated = true;
        //capBoosterCompleteTest++;
        ////SaleManager.Instance.listShipComplete[0].GoSail(0);
        //SaleManager.Instance.currentShipComplete.GoSail(0);
        //BoosterReturn(completeTestBoosterObject);

        BoosterReturn(completeTestBoosterObject);
        StartCoroutine(CoWaitActivateBoosterCompleteShipTest(2));
    }

    IEnumerator CoWaitActivateBoosterCompleteShipTest(float delay)
    {
        yield return new WaitForSeconds(delay);
        timeBoosterCompleteTestWait = 0;
        isCompleteTestAppear = false;
        capBoosterCompleteTest++;
        SaleManager.Instance.currentShipComplete.GoSail(0);
    }

    public void ShowBoosterCompleteTest()
    {
        if (!isCompleteTestAppear)
        {
            timeBoosterCompleteTestWait += Time.deltaTime;
            if (timeBoosterCompleteTestWait > 15)
            {
                BoosterAppear(completeTestBoosterObject);
                timeBoosterCompleteTestWait = 0;
                isCompleteTestAppear = true;
            }
        }
        else
        {
            timeBoosterCompleteTestAppear += Time.deltaTime;
            if (timeBoosterCompleteTestAppear > 15)
            {
                BoosterReturn(completeTestBoosterObject);
                timeBoosterCompleteTestAppear = 0;
                isCompleteTestAppear = false;
                timeBoosterCompleteTestWait = 0;
            }
        }
    }

    public void ShowBoosterX2SpeedTest()
    {
        timeBoosterSpeedTestWait += Time.deltaTime;
        if (!isBoosterSpeedTestActivated)
        {
            if (!isSpeedTestAppear)
            {
                if (timeBoosterSpeedTestWait > 10)
                {
                    BoosterAppear(speedTestBoosterObject);
                    timeBoosterSpeedTestWait = 0;
                    isSpeedTestAppear = true;
                }               
            }
            else
            {
                timeBoosterSpeedTestAppear += Time.deltaTime;
                if (timeBoosterSpeedTestAppear > 15)
                {
                    BoosterReturn(speedTestBoosterObject);
                    timeBoosterSpeedTestAppear = 0;
                    isSpeedTestAppear = false;
                    timeBoosterSpeedTestWait = 0;
                }
            }
        }
    }

    public void ActivateBoosterSpeedTest()
    {
        isBoosterSpeedTestActivated = true;
        timeSpeedTestActivatedObject.SetActive(true);
        timeSpeedTestActivated = 5 * 60;
        SaleManager.Instance.numSpeedBooster = 2;
        SaleManager.Instance.UpgradeSpeedTest();
        BoosterReturn(speedTestBoosterObject);
    }
}
