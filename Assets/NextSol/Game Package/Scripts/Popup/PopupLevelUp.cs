using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLevelUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCollectReward()
    {
        DataManager.Instance.numLevel++;
        GameUIManager.Instance.popupLevelUp.SetActive(false);
        GameUIManager.Instance.AddMoney(1000 * Mathf.Pow(DataManager.Instance.numLevel - 1, 1.5f) + 500);
        GameUIManager.Instance.ProgressLevelUp();
        DataManager.Instance.statusLevelUp = 0;

        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void Adsx2Reward()
    {
        Collectx2Reward();
        GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();

    }
    public void Adsx5Reward()
    {
        Collectx5Reward();
        GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();
    }
    public void Adsx2OfflineReward()
    {
        Collectx2OfflineIncome();
        //GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();
    }
  
    public void Collectx2Reward()
    {
        DataManager.Instance.numLevel++;
        GameUIManager.Instance.popupLevelUp.SetActive(false);
        GameUIManager.Instance.AddMoney((1000 * Mathf.Pow(DataManager.Instance.numLevel - 1, 1.5f) + 500) * 2);
        GameUIManager.Instance.ProgressLevelUp();
        DataManager.Instance.statusLevelUp = 0;
        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void Collectx5Reward()
    {
        DataManager.Instance.numLevel++;
        GameUIManager.Instance.popupLevelUp.SetActive(false);
        GameUIManager.Instance.AddMoney((1000 * Mathf.Pow(DataManager.Instance.numLevel - 1, 1.5f) + 500) * 5);
        GameUIManager.Instance.ProgressLevelUp();
        DataManager.Instance.statusLevelUp = 0;

        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void CollectOfflineIncome()
    {
        GameUIManager.Instance.AddMoney(GameManager.Instance.moneyOffline);
        GameUIManager.Instance.popupEarning.SetActive(false);
        GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();
        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void Collectx2OfflineIncome()
    {
        GameUIManager.Instance.AddMoney(GameManager.Instance.moneyOffline * 2);
        GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();
        GameUIManager.Instance.popupEarning.SetActive(false);
        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void Collectx5OfflineIncome()
    {
        GameUIManager.Instance.AddMoney(GameManager.Instance.moneyOffline * 5);
        GameUIManager.Instance.popupEarning.SetActive(false);
        //SdkNextsolManager.Instance.fbManager.LogRewarded_shownEvent();
    }
    public void FailToLoadAds()
    {
        GameUIManager.Instance.popupNoads.SetActive(true);
        Invoke("AutoHidenPopup", 3f);
    }
    public void AutoHidenPopup()
    {
        GameUIManager.Instance.popupNoads.SetActive(false);
    }
}
