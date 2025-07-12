using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    public Image imageProgress;
    public int currentMap;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        Instance = this;
        StartCoroutine(CoWaitLoading(3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CoWaitLoading(float delay)
    {
        imageProgress.fillAmount = 0;
        imageProgress.DOFillAmount(1, delay).SetEase(Ease.Linear);
        yield return new WaitForSeconds(delay);
        SendEventFirebase.GameStart();
        SceneManager.LoadScene(currentMap + 1);
    }

    public void LoadData()
    {
        currentMap = PlayerPrefs.GetInt("currentMap", 0);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("currentMap", currentMap);
    }
}
