using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public List<GameObject> listPanelTutorial;
    public List<GameObject> listPanelTutorialUpgrade, listPanelTutorialCaptain;
    public GameObject panelBooster;
    public List<GameObject> listTutorialCameraPos;

    public GameObject effPointerHouse, effPointerCaptain;
    public GameObject effPointerUI, effPointerUI2, effHandPointer;

    public GameObject adaptiveMess1;
    public float timeAdaptiveMessAppear = 0;
    public bool isMoveCamera = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        LoadTutorialStep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTutorialStep()
    {
        effPointerCaptain.SetActive(false);
        effPointerHouse.SetActive(false);
        effPointerUI.SetActive(false);

        for (int i = 0; i < listPanelTutorial.Count; i++)
        {
            listPanelTutorial[i].SetActive(false);
        }

        if (DataManager.Instance.numTutorialStep < 4)
        {
            panelBooster.SetActive(false);
            OperatorManager.Instance.listZone[1].lockObject.GetComponent<Animator>().Play("pause");
            OperatorManager.Instance.listZone[2].lockObject.GetComponent<Animator>().Play("pause");
        }

        if (DataManager.Instance.numTutorialStep == 0)
        {
            GameUIManager.Instance.cameraObject.transform.DOLocalMove(listTutorialCameraPos[0].transform.localPosition, 0.3f).SetEase(Ease.Linear);
            ActivePanelTutorial(0);
            //effPointerUI2.SetActive(true);
            effHandPointer.SetActive(true);
        }

    }

    public void ActivePanelTutorial(int index)
    {
        for (int i = 0; i < listPanelTutorial.Count; i++)
        {
            listPanelTutorial[i].SetActive(false);
        }
        listPanelTutorial[index].SetActive(true);
    }

    public void ActivePanelUpgrade(int index)
    {
        for (int i = 0; i < listPanelTutorialUpgrade.Count; i++)
        {
            listPanelTutorialUpgrade[i].SetActive(false);
        }
        listPanelTutorialUpgrade[index].SetActive(true);
    }

    public void ActiveTutorialCaptainStep1()
    {
        effPointerUI.SetActive(false);
        ActivePanelTutorial(3);
        PopupUpgrade.Instance.OnClose();
        OperatorManager.Instance.listCaptains[0].unhiredObject.SetActive(true);
        effHandPointer.transform.localPosition = new Vector3(72, -186, 0);
        GameUIManager.Instance.cameraObject.transform.DOLocalMove(listTutorialCameraPos[2].transform.localPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() => effHandPointer.SetActive(true));
        ActivePanelCaptain(0);
    }

    public void ActiveTutorialCaptainStep2()
    {
        ActivePanelCaptain(1);
        effPointerUI.SetActive(true);
        effPointerUI.transform.position = PopupCaptain.Instance.buttonHire.transform.position;
    }

    public void ActiveTutorialCaptainStep3()
    {
        ActivePanelCaptain(2);
        effPointerUI.SetActive(false);
    }

    public void ActivePanelCaptain(int index)
    {
        for (int i = 0; i < listPanelTutorialCaptain.Count; i++)
        {
            listPanelTutorialCaptain[i].SetActive(false);
        }
        listPanelTutorialCaptain[index].SetActive(true);
    }

    public void OnClickTutorial0()
    {
        DataManager.Instance.numTutorialStep = 1;
        listPanelTutorial[0].GetComponent<TextEffect>().OnDisappear();
        
        GameManager.Instance.SetUpShip(0);
        GameManager.Instance.SetUpWorkerForZone(0);
        //effPointerUI2.SetActive(false);
        effHandPointer.SetActive(false);
    }

    public void OnClickTutorial1()
    {
        listPanelTutorial[1].GetComponent<TextEffect>().OnDisappear();
        effHandPointer.SetActive(false);
        GameManager.Instance.SetUpWorkerForZone(0);
        DataManager.Instance.numStep1++;
        if(DataManager.Instance.numStep1 > 1)
        {
            ActivePanelTutorial(2);
            ActivePanelUpgrade(0);
            effPointerHouse.SetActive(true);
            GameUIManager.Instance.cameraObject.transform.DOLocalMove(listTutorialCameraPos[1].transform.localPosition, 0.3f).SetEase(Ease.Linear);
            DataManager.Instance.numTutorialStep = 2;
            effHandPointer.transform.localPosition = new Vector3(139, -11, 0);
            effHandPointer.SetActive(true);
        }
    }

    public void OnClickTutorialCaptain()
    {
        DataManager.Instance.numTutorialStep = 4;
        listPanelTutorial[3].SetActive(false);
        panelBooster.SetActive(true);
        OperatorManager.Instance.listZone[1].lockObject.GetComponent<Animator>().Play("idle");
        OperatorManager.Instance.listZone[2].lockObject.GetComponent<Animator>().Play("idle");
    }

    public void ShowAdaptiveMess1()
    {
        //StartCoroutine(CoWaitShowAdaptiveMess1());
    }

    IEnumerator CoWaitShowAdaptiveMess1()
    {
        adaptiveMess1.SetActive(true);
        if (!isMoveCamera)
        {
            //GameUIManager.Instance.ChangeCameraZonePos(3);
            GameUIManager.Instance.cameraObject.transform.DOLocalMove(GameUIManager.Instance.listCameraZonePos[3].transform.localPosition, 0.3f).SetEase(Ease.Linear);
            isMoveCamera = true;
        }
        yield return new WaitForSeconds(3);
        adaptiveMess1.SetActive(false);
    }
}
