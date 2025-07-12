using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int levelIndex;
    public Image icon;
    public Image iconLock;
    public GameObject currentLocated;
    public Text textName;
    public Text textMoneyPerSec;
    public Button buttonOpen;
    public Button buttonUnlock;
    public Text textUnlockCost;
    public Image imageDisable;
    public GameObject imageLockMap;
    public GameObject objComingSoon;
    public GameObject imageLock;

    public GameObject progressUnlockObject;
    public Image progressUnlock;
    public Text textTimeUnlock;
    public bool isWaitToUnlock;
    public Text textTimeAds;
    public GameObject mBgObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
       // if (levelIndex == 1)
        {
            int isFirstTime = PlayerPrefs.GetInt("FirstMapOpen", 0);
            if (isFirstTime == 0 && levelIndex == 1)
            {
                mBgObject.gameObject.GetComponent<Animator>().enabled = true;
                PlayerPrefs.SetInt("FirstMapOpen", 1);
            }
            else
            {
                mBgObject.gameObject.GetComponent<Animator>().enabled = false;
                mBgObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickOpenLevel()
    {
        SceneManager.LoadScene(levelIndex + 1);
        DataManager.Instance.SaveData();
        LoadingManager.Instance.currentMap = levelIndex;
        LoadingManager.Instance.SaveData();
    }

    public void OnClickButtonUnlock()
    {
        //DataManager.Instance.numMapUnlocked++;
        //buttonOpen.gameObject.SetActive(true);
        DataManager.Instance.listMapIsWaitToUnlock[levelIndex] = 1;
        progressUnlockObject.SetActive(true);
        buttonUnlock.gameObject.SetActive(false);
        textTimeAds.text =  "-" +  UnitConverter.ConvertTime(DataManager.Instance.upgradeDatas.Unlock_Map[levelIndex].time_ads);
    }

    public void WatchSucessfully()
    {
        DataManager.Instance.listTimeUnlockCounted[levelIndex] += DataManager.Instance.upgradeDatas.Unlock_Map[levelIndex].time_ads;
    }
    
    public void FailToLoadAds()
    {
        Invoke("AutoHidenPopup", 3f);
    }

    public void AutoHidenPopup()
    {
        GameUIManager.Instance.popupNoads.SetActive(false);
    }
}
