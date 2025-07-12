using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PopupSetting : MonoBehaviour
{
    public static PopupSetting Instance;

    public Image imageMusicOn, imageMusicOff;
    public Image imageSoundOn, imageSoundOff;
    public Image imageNotiOn, imageNotiOff;
    public Image imageMusicControl;
    public Image imageSoundControl;
    public Image imageNotiControl;
    public Text textComingSoon;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        LoadSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSetting()
    {
        imageMusicOn.gameObject.SetActive(DataManager.Instance.musicStatus == 1);
        imageMusicOff.gameObject.SetActive(DataManager.Instance.musicStatus == 0);
        
        imageSoundOn.gameObject.SetActive(DataManager.Instance.soundStatus == 1);
        imageSoundOff.gameObject.SetActive(DataManager.Instance.soundStatus == 0);
        
        imageNotiOn.gameObject.SetActive(DataManager.Instance.notiStatus == 1);
        imageNotiOff.gameObject.SetActive(DataManager.Instance.notiStatus == 0);
    }

    public void OnClickMusic()
    {
        Debug.Log("music");
        if (DataManager.Instance.musicStatus == 0)
        {
            imageMusicOn.gameObject.SetActive(true);
            imageMusicOff.gameObject.SetActive(false);
            DataManager.Instance.musicStatus = 1;
        }
        else
        {
            imageMusicOn.gameObject.SetActive(false);
            imageMusicOff.gameObject.SetActive(true);
            DataManager.Instance.musicStatus = 0;
        }
        SoundManager.Instance.LoadStatusSetting();
    }

    public void OnClickSound()
    {
        if (DataManager.Instance.soundStatus == 0)
        {
            imageSoundOn.gameObject.SetActive(true);
            imageSoundOff.gameObject.SetActive(false);
            DataManager.Instance.soundStatus = 1;
        }
        else
        {
            imageSoundOn.gameObject.SetActive(false);
            imageSoundOff.gameObject.SetActive(true);
            DataManager.Instance.soundStatus = 0;
        }
        SoundManager.Instance.LoadStatusSetting();
    }

    public void OnClickNoti()
    {
        if (DataManager.Instance.notiStatus == 0)
        {
            imageNotiOn.gameObject.SetActive(true);
            imageNotiOff.gameObject.SetActive(false);
            DataManager.Instance.notiStatus = 1;
        }
        else
        {
            imageNotiOn.gameObject.SetActive(false);
            imageNotiOff.gameObject.SetActive(true);
            DataManager.Instance.notiStatus = 0;
        }
    }

    public void OnClickClose()
    {
        GameUIManager.Instance.popupSetting.SetActive(false);
    }
    public void OnClickQuit()
    {
        DataManager.Instance.SaveData();
        LoadingManager.Instance.SaveData();
        SceneManager.LoadScene(2);
    }

    public void OnClickOther()
    {
        textComingSoon.DOKill();
        textComingSoon.gameObject.SetActive(true);
        textComingSoon.transform.localPosition = new Vector3(0, -100, 0);
        textComingSoon.color = new Color(1, 1, 1, 1);
        textComingSoon.transform.DOLocalMove(new Vector3(0, 100, 0), 1).SetEase(Ease.Linear);
        textComingSoon.DOColor(new Color(1, 1, 1, 0), 1).SetEase(Ease.Linear);
    }

    public void OnClickExit()
    {
        GameUIManager.Instance.popupExit.SetActive(true);
    }
}
