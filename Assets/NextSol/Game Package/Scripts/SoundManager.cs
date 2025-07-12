using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource musicBGSource;
    public AudioSource soundSource;

    public AudioClip collectClick;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        LoadStatusSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPopupActive())
        {
            soundSource.volume = 0;
        }
        else
        {
            soundSource.volume = DataManager.Instance.soundStatus;
        }
    }

    public bool IsPopupActive()
    {
        if (GameUIManager.Instance.popupAds.activeSelf || GameUIManager.Instance.popupEarning.activeSelf
            || GameUIManager.Instance.popupExit.activeSelf || GameUIManager.Instance.popupEarning.activeSelf
            || GameUIManager.Instance.popupManager.activeSelf || GameUIManager.Instance.popupMap.activeSelf
            || GameUIManager.Instance.popupNoads.activeSelf || GameUIManager.Instance.popupSessionReward.activeSelf
            || GameUIManager.Instance.popupSetting.activeSelf || GameUIManager.Instance.popupShop.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadStatusSetting()
    {
        musicBGSource.volume = DataManager.Instance.musicStatus;
        soundSource.volume = DataManager.Instance.soundStatus;
    }

    public void PlayCollectClick()
    {
        soundSource.clip = collectClick;
        soundSource.Play();
    }
}
