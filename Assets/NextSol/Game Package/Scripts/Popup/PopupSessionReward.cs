using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSessionReward : MonoBehaviour
{
    public static PopupSessionReward Instance;
    public SessionRewardDatas sessionDatas;
    public GameObject prefabSessionReward;
    public GameObject panelSessionReward;
    public Image imageNew;
    public bool isNew;
    public List<SessionRewardController> listSessionReward;
    public Text textTimeOnline, textSessionRewardTime;
    public GameObject buttonGiftbox;
    public GameObject imageGiftbox;
    public List<Sprite> listRewardReady;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitSessionReward();
        if (DataManager.Instance.numSessionRewardClaimed == 3)
        {
            buttonGiftbox.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonGiftbox.activeSelf)
        {
            RefreshUI();
        }
    }

    public void InitSessionReward()
    {
        for (int i = 0; i < DataManager.Instance.upgradeDatas.Session_Reward.Count; i++)
        {
            GameObject goReward = Instantiate(prefabSessionReward, panelSessionReward.transform);
            goReward.transform.localPosition = new Vector3(-330 + i * 330, -165, 0);
            goReward.GetComponent<SessionRewardController>().idSession = i;
            goReward.GetComponent<SessionRewardController>().imageIconReady.sprite = listRewardReady[i];
            goReward.GetComponent<SessionRewardController>().imageIconReady.SetNativeSize();
            goReward.GetComponent<SessionRewardController>().textTime.text = UnitConverter.ConvertTime(DataManager.Instance.upgradeDatas.Session_Reward[i].time);
            goReward.GetComponent<SessionRewardController>().textMoneyReward.text = UnitConverter.Convert(DataManager.Instance.upgradeDatas.Session_Reward[i].reward);
            listSessionReward.Add(goReward.GetComponent<SessionRewardController>());
        }
    }

    public void SetAnimIcon()
    {
        StartCoroutine(CoSetAnimIcon());
    }

    IEnumerator CoSetAnimIcon()
    {
        for (int i = 0; i < listSessionReward.Count; i++)
        {
            listSessionReward[i].icon.gameObject.SetActive(false);
        }

        for (int i = 0; i < listSessionReward.Count; i++)
        {
            listSessionReward[i].icon.gameObject.SetActive(DataManager.Instance.listSessionClaim[i] == 0);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < listSessionReward.Count; i++)
        {
            listSessionReward[i].imageBGCurrent.gameObject.SetActive(i == DataManager.Instance.numSessionRewardClaimed);
            listSessionReward[i].imageShadow.gameObject.SetActive(DataManager.Instance.listSessionClaim[i] == 0);
            //listSessionReward[i].icon.gameObject.SetActive(DataManager.Instance.listSessionClaim[i] == 0);
            if (DataManager.Instance.timeOnline >= DataManager.Instance.upgradeDatas.Session_Reward[i].time)
            {
                listSessionReward[i].imageReadyEff.gameObject.SetActive(true);
                listSessionReward[i].buttonClaim.interactable = true;
            }
            else
            {
                listSessionReward[i].imageReadyEff.gameObject.SetActive(false);
                listSessionReward[i].buttonClaim.interactable = false;
            }

            if (DataManager.Instance.listSessionClaim[i] == 1)
            {
                listSessionReward[i].imageIconReady.gameObject.SetActive(false);
                listSessionReward[i].imageClaimed.gameObject.SetActive(true);
                listSessionReward[i].imageReadyEff.gameObject.SetActive(false);
                listSessionReward[i].buttonClaim.interactable = false;
            }
            else
            {
                listSessionReward[i].imageClaimed.gameObject.SetActive(false);
                listSessionReward[i].imageIconReady.gameObject.SetActive(true);
            }
        }

        if (DataManager.Instance.upgradeDatas.Session_Reward[DataManager.Instance.numSessionRewardClaimed].time > (int)DataManager.Instance.timeOnline){
            textTimeOnline.text = UnitConverter.ConvertTime2(DataManager.Instance.upgradeDatas.Session_Reward[DataManager.Instance.numSessionRewardClaimed].time - (int)DataManager.Instance.timeOnline);
            textSessionRewardTime.text = UnitConverter.ConvertTime2(DataManager.Instance.upgradeDatas.Session_Reward[DataManager.Instance.numSessionRewardClaimed].time - (int)DataManager.Instance.timeOnline);
            imageGiftbox.GetComponent<Animator>().Play("idle");
        }
        else
        {
            textTimeOnline.text = "Ready!";
            //textTimeOnline.text = UnitConverter.ConvertTime2(0);
            //textSessionRewardTime.text = UnitConverter.ConvertTime2(0);
            textSessionRewardTime.text = "Ready!";
            //imageGiftbox.GetComponent<Animator>().Play("reward_alarm");     
            DataManager.Instance.isSessionRewardClaimed = 1;
        }
    }

    public void OnClose()
    {
        GameUIManager.Instance.popupSessionReward.SetActive(false);
    }
}
