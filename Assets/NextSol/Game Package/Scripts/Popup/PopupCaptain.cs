using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupCaptain : MonoBehaviour
{
    public static PopupCaptain Instance;
    public GameObject panelCaptain;
    public int zone;
    public Button buttonHire;
    public Text textCost;
    public Image imageHired;
    public int numWorkerDone = 0;
    public Button buttonClose;
    public Text textTitle;
    public Text textCaptainName;
    public Text textCaptainBoost;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickHireCaptain()
    {
        if (OperatorManager.Instance.listCaptains[zone].isOpen)
        {
            OperatorManager.Instance.listCaptains[zone].anim.Play("work");
            DataManager.Instance.listHireCaptain[zone + 3 * LoadingManager.Instance.currentMap] = 1;
            OperatorManager.Instance.listCaptains[zone].LoadCaptainData();
            OperatorManager.Instance.listCaptainIDHired.Add(DataManager.Instance.listCaptainID[zone]);
            OperatorManager.Instance.listCaptains[zone].hiredObject.SetActive(true);
            OperatorManager.Instance.listCaptains[zone].unhiredObject.SetActive(false);
            //OperatorManager.Instance.listCaptains[zone].isHired = true;

            //GameManager.Instance.SetUpWorkerForZone(zone);
            var tweener = panelCaptain.transform.DOLocalMove(panelCaptain.transform.localPosition - new Vector3(0, 1000, 0), 0.5f).SetEase(Ease.Linear);
            buttonClose.gameObject.SetActive(false);
            StartCoroutine(CoWaitClose(tweener));

            if(DataManager.Instance.numTutorialStep == 3)
            {
                TutorialManager.Instance.ActiveTutorialCaptainStep3();
                GameManager.Instance.SetUpWorkerForZone(0);
            }
        }       
    }

    public void OnClose()
    {
        if(DataManager.Instance.numTutorialStep > 3)
        {
            if (OperatorManager.Instance.listCaptains[zone].isOpen)
            {
                var tweener = panelCaptain.transform.DOLocalMove(panelCaptain.transform.localPosition - new Vector3(0, 1000, 0), 0.5f).SetEase(Ease.Linear);
                buttonClose.gameObject.SetActive(false);
                StartCoroutine(CoWaitClose(tweener));
            }
        }
    }

    IEnumerator CoWaitClose(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        OperatorManager.Instance.listCaptains[zone].isOpen = false;
    }
}
