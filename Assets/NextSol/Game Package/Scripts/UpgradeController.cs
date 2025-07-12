using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeController : MonoBehaviour
{
    // id 0 = earning, 1 = speed, 2 = worker

    public int zone;
    public int idUpgrade;
    public int level;
    public double cost;
    public Button buttonUpgrade;
    public Image icon;
    public Text textTitle;
    public Text textLevel;
    public Text textCost;
    public Image imageDisabled;
    public Image imageBg;
    public GameObject imageUp;
    public GameObject progressObject;
    public Image progressBar;
    public int numUpgrade;
    public Text textNumUpgrade;

    public delegate void EarningUpgradeCallback();

    private List<EarningUpgradeCallback> mlistEarningCallback = new List<EarningUpgradeCallback>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterEarningUpgrade(EarningUpgradeCallback cb)
    {
        mlistEarningCallback.Add(cb);
    }

    public void OnClickUpgrade()
    {
        if (DataManager.Instance.numTutorialStep == 2)
        {

            if (DataManager.Instance.numStep2 == 0 && idUpgrade == 2)
            {
                InitButtonEffect2();
                GameUIManager.Instance.listUpgradeEffects[zone].Play();
                GameUIManager.Instance.SpendMoney(cost);
                GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade] += numUpgrade;

                DataManager.Instance.listLevelWorker[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;
                StartCoroutine(CoWaitNewWorker(numUpgrade));

                SendEventFirebase.SendEvent_worker_update();
                DataManager.Instance.numStep2++;
                TutorialManager.Instance.ActivePanelUpgrade(2);
                TutorialManager.Instance.effPointerUI.SetActive(true);
                TutorialManager.Instance.effPointerUI.transform.position = GameUIManager.Instance.listListUpgrade[0][1].GetComponent<UpgradeController>().buttonUpgrade.transform.position;

            }
            else if (DataManager.Instance.numStep2 == 1 && idUpgrade == 1)
            {
                InitButtonEffect2();
                GameUIManager.Instance.listUpgradeEffects[zone].Play();
                GameUIManager.Instance.SpendMoney(cost);
                GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade] += numUpgrade;

                DataManager.Instance.listLevelSpeed[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;
                GameManager.Instance.UpgradeSpeedForZone(zone);
                SendEventFirebase.SendEvent_speed_update();
                DataManager.Instance.numStep2++;
                TutorialManager.Instance.ActivePanelUpgrade(3);
                TutorialManager.Instance.effPointerUI.SetActive(true);
                TutorialManager.Instance.effPointerUI.transform.position = GameUIManager.Instance.listListUpgrade[0][0].GetComponent<UpgradeController>().buttonUpgrade.transform.position;
            }
            else if (DataManager.Instance.numStep2 == 2 && idUpgrade == 0)
            {
                InitButtonEffect2();
                GameUIManager.Instance.listUpgradeEffects[zone].Play();
                GameUIManager.Instance.SpendMoney(cost);
                GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade] += numUpgrade;

                DataManager.Instance.listLevelEarning[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;
                GameUIManager.Instance.listIncome[zone] = DataManager.Instance.upgradeDatas.Upgrade_Income[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]].income1;
                SendEventFirebase.SendEvent_earning_update();
                DataManager.Instance.numStep2++;
                DataManager.Instance.numTutorialStep = 3;
                TutorialManager.Instance.ActiveTutorialCaptainStep1();
            }
            GameUIManager.Instance.RefreshUI();
        }
        else
        {
            InitButtonEffect2();
            GameUIManager.Instance.listUpgradeEffects[zone].Play();
            GameUIManager.Instance.SpendMoney(cost);
            GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade] += numUpgrade;

            if (idUpgrade == 0)
            {
                DataManager.Instance.listLevelEarning[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;
                GameUIManager.Instance.listIncome[zone] = DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[zone + 3 * LoadingManager.Instance.currentMap]][zone + 3 * LoadingManager.Instance.currentMap];

                GameObject.Find("Map").transform.Find("Ingame").transform.Find("Xuong_1").transform.Find("Unlocked")
                    .transform.Find("NhaXuong").GetComponent<OperatorController>().OnEarningLvlUp();

                GameObject.Find("Map").transform.Find("Ingame").transform.Find("Xuong_2").transform.Find("Unlocked")
                    .transform.Find("NhaXuong").GetComponent<OperatorController>().OnEarningLvlUp();

                GameObject.Find("Map").transform.Find("Ingame").transform.Find("Xuong_3").transform.Find("Unlocked")
                    .transform.Find("NhaXuong").GetComponent<OperatorController>().OnEarningLvlUp();

                foreach (EarningUpgradeCallback cb in mlistEarningCallback)
                {
                    cb();
                }

                //if (zone == 0)
                //{
                //    GameUIManager.Instance.listIncome[zone] = DataManager.Instance.upgradeDatas.Upgrade_Income[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]].income1;
                //}
                //else if (zone == 1)
                //{
                //    GameUIManager.Instance.listIncome[zone] = DataManager.Instance.upgradeDatas.Upgrade_Income[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]].income2;
                //}
                //else if (zone == 2)
                //{
                //    GameUIManager.Instance.listIncome[zone] = DataManager.Instance.upgradeDatas.Upgrade_Income[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]].income3;
                //}

                SendEventFirebase.SendEvent_earning_update();
            }
            else if (idUpgrade == 1)
            {
                DataManager.Instance.listLevelSpeed[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;

                GameManager.Instance.UpgradeSpeedForZone(zone);
                SendEventFirebase.SendEvent_speed_update();
            }
            else if (idUpgrade == 2)
            {
                DataManager.Instance.listLevelWorker[zone + 3 * LoadingManager.Instance.currentMap] += numUpgrade;
                if (DataManager.Instance.listZoneFinish[zone + 3 * LoadingManager.Instance.currentMap] == 0)
                {
                    StartCoroutine(CoWaitNewWorker(numUpgrade));
                }

                SendEventFirebase.SendEvent_worker_update();
            }
        }

        SendEventFirebase.UpgradeDocksEvent(0, "Workshop", zone, GameUIManager.Instance.listTitle[idUpgrade], GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]);
        GameUIManager.Instance.RefreshUI();
    }

    IEnumerator CoWaitNewWorker(int numUpgrade)
    {
        int numWorker = (int)(Mathf.Floor(DataManager.Instance.upgradeDatas.Upgrade_Worker[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade]].worker)
            - Mathf.Floor(DataManager.Instance.upgradeDatas.Upgrade_Worker[GameUIManager.Instance.listListUpgradeLevel[zone][idUpgrade] - numUpgrade].worker));
        for (int i = 0; i < numWorker; i++)
        {
            GameManager.Instance.SetUpNewWorker(zone);
            yield return new WaitForSeconds(1f);
        }
    }

    // id 0 = price, 1 = speed, 2 = slot
    public void OnClickUpgradeSale()
    {
        InitButtonEffect();
        SaleManager.Instance.effectUpgrade.Play();
        GameUIManager.Instance.SpendMoney(cost);
        SaleManager.Instance.listUpgradeLevel[idUpgrade] += numUpgrade;

        if (idUpgrade == 0)
        {
            DataManager.Instance.listLevelSalePrice[LoadingManager.Instance.currentMap] += numUpgrade;
        }
        else if (idUpgrade == 1)
        {
            DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap] += numUpgrade;
            //Debug.Log(DataManager.Instance.upgradeDatas.Upgrade_Speed_Test[DataManager.Instance.listLevelSaleSpeed[LoadingManager.Instance.currentMap]].speed);
            SaleManager.Instance.UpgradeSpeedTest();
        }
        else if (idUpgrade == 2)
        {
            DataManager.Instance.levelSaleSlot++;
            progressBar.DOFillAmount((float)((DataManager.Instance.levelSaleSlot - 1) % 5) / 5, 0.2f).SetEase(Ease.Linear);
        }

        SendEventFirebase.UpgradeDocksEvent(0, "Trading House", 0, SaleManager.Instance.listTitle[idUpgrade], SaleManager.Instance.listUpgradeLevel[idUpgrade]);
        SaleManager.Instance.RefreshUI();
    }

    public void InitButtonEffect()
    {
        buttonUpgrade.transform.DOScale(Vector3.one * 1.5f, 0.15f).SetEase(Ease.Linear);
        buttonUpgrade.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear).SetDelay(0.15f);

        //GameObject goMoney = Instantiate(textCost.transform.parent.gameObject, textCost.transform.parent.parent);
        //goMoney.transform.position = textCost.transform.parent.gameObject.transform.position;
        //goMoney.transform.DOMove(goMoney.transform.position + new Vector3(0, 100, 0), 0.3f).SetEase(Ease.Linear);
        //Destroy(goMoney, 0.4f);

        GameObject goIcon = Instantiate(icon.gameObject, SaleManager.Instance.panelEffect.transform);
        goIcon.transform.position = icon.transform.position;
        goIcon.transform.DOMove(goIcon.transform.position + new Vector3(0, 100, 0), 0.3f).SetEase(Ease.Linear);
        Destroy(goIcon, 0.3f);
    }
    public void InitButtonEffect2()
    {
        buttonUpgrade.transform.DOScale(Vector3.one * 1.5f, 0.15f).SetEase(Ease.Linear);
        buttonUpgrade.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear).SetDelay(0.15f);

        //GameObject goMoney = Instantiate(textCost.transform.parent.gameObject, textCost.transform.parent.parent);
        //goMoney.transform.position = textCost.transform.parent.gameObject.transform.position;
        //goMoney.transform.DOMove(goMoney.transform.position + new Vector3(0, 100, 0), 0.3f).SetEase(Ease.Linear);
        //Destroy(goMoney, 0.4f);

        GameObject goIcon = Instantiate(icon.gameObject, GameUIManager.Instance.panelEffect.transform);
        goIcon.transform.position = icon.transform.position;
        goIcon.transform.DOMove(goIcon.transform.position + new Vector3(0, 100, 0), 0.3f).SetEase(Ease.Linear);
        Destroy(goIcon, 0.3f);
    }

}
