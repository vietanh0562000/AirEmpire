using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionRewardController : MonoBehaviour
{
    public int idSession;
    public Image imageBGCurrent;
    public Text textTime;
    public Text textMoneyReward;
    public Button buttonClaim;
    public Image imageIconReady;
    public GameObject icon;
    public Image imageShadow;
    public Image imageReadyEff;
    public Image imageClaimed;
    public bool isClaim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClaim()
    {
        Debug.Log("claim");
        icon.gameObject.SetActive(false);
        imageIconReady.gameObject.SetActive(false);
        imageClaimed.gameObject.SetActive(true);
        buttonClaim.interactable = false;
        DataManager.Instance.listSessionClaim[idSession] = 1;
        GameUIManager.Instance.AddMoney(DataManager.Instance.upgradeDatas.Session_Reward[idSession].reward);
        PopupSessionReward.Instance.imageNew.gameObject.SetActive(false);
        DataManager.Instance.timeOnline = 0;

        GameUIManager.Instance.effMoneyUI.GetComponent<ParticleSystem>().Play();
        GameUIManager.Instance.effMoneyUI.transform.localPosition = Vector3.zero;
        GameUIManager.Instance.effGiftBlastUI.GetComponent<ParticleSystem>().Play();
        GameUIManager.Instance.effGiftBlastUI.transform.localPosition = gameObject.transform.localPosition;

        if (idSession < 2)
        {
            DataManager.Instance.isSessionRewardClaimed = 0;
        }
        else
        {
            PopupSessionReward.Instance.buttonGiftbox.SetActive(false);
        }
        DataManager.Instance.numSessionRewardClaimed++;

        SendEventFirebase.SessionRewardReceiveEvent();
    }
}
