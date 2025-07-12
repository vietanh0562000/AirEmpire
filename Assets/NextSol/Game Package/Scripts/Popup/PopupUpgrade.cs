using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupUpgrade : MonoBehaviour
{
    public static PopupUpgrade Instance;
    public List<Text> listTextZone;
    public Text textZone;
    public Button buttonClose;
    public GameObject panelUpgrade;
    public GameObject panelUnlock;
    public GameObject bgUnlock;
    public Text textUnlockZone;
    public Text textUnlockCost;
    public Button buttonUnlock;
    public Image imageLock;
    public int currentZone;
    public bool isOpen = false, isOpenDone = false;

    public List<GameObject> listUpgradePanel;
    public int numUpgrade;
    public Text textUpgrade;
    public Text textUpgradeSale;
    public GameObject mask, mMapUnlockItem;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameUIManager.Instance.popupUpgrade.activeSelf)
        {
            buttonUnlock.interactable = GameUIManager.Instance.moneyTotal >= DataManager.Instance.upgradeDatas.Unlock_Data[currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost;
            imageLock.gameObject.SetActive(GameUIManager.Instance.moneyTotal < DataManager.Instance.upgradeDatas.Unlock_Data[currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost);
        }
    }

    public void ShowUpgradeZone(int zone)
    {
        isOpen = true;
        isOpenDone = true;
        currentZone = zone;
        textZone.text = "Workshop " + zone;
        GameUIManager.Instance.ChangeCameraZonePos(zone - 1);

        for (int i = 0; i < listUpgradePanel.Count; i++)
        {
            listUpgradePanel[i].SetActive(false);
        }

        if(OperatorManager.Instance.listOperators[zone - 1].isUnlock)
        {
            listUpgradePanel[zone - 1].SetActive(true);
            panelUnlock.SetActive(false);
            bgUnlock.SetActive(false);
            textUpgrade.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            panelUnlock.SetActive(true);
            bgUnlock.SetActive(true);
            textUnlockZone.text = "Unlock The Workshop No " + zone;
            textUnlockCost.text = UnitConverter.Convert(DataManager.Instance.upgradeDatas.Unlock_Data[currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost);
            textUpgrade.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnClose()
    {
        if(DataManager.Instance.numTutorialStep > 2)
        {
            if (OperatorManager.Instance.listOperators[currentZone - 1].isOpen || OperatorManager.Instance.listZone[currentZone - 1].isOpen)
            {
                isOpen = false;
                var tweener = panelUpgrade.transform.DOLocalMove(panelUpgrade.transform.localPosition - new Vector3(0, 1000, 0), 0.3f).SetEase(Ease.Linear);
                //buttonClose.gameObject.SetActive(false);
                //OperatorManager.Instance.listOperators[currentZone - 1].gameObject.GetComponent<Outline>().enabled = false;
                //OperatorManager.Instance.listOperators[currentZone - 1].isOpen = false;
                StartCoroutine(CoWaitClose(tweener));
            }
        }
        
    }

    IEnumerator CoWaitClose(Tweener tweener)
    {
        GameUIManager.Instance.cameraObject.GetComponent<Camera>().DOOrthoSize(5, 0.3f).SetEase(Ease.Linear);
        OperatorManager.Instance.listOperators[currentZone - 1].anim.Play("arrow");
        yield return tweener.WaitForCompletion();
        OperatorManager.Instance.listOperators[currentZone - 1].isOpen = false;
        OperatorManager.Instance.listZone[currentZone - 1].isOpen = false;
        isOpenDone = false;
    }

    public void OnClickUnlock()
    {
        Debug.Log("unlock");
        StartCoroutine(CoWaitUnlock());
    }

    IEnumerator CoWaitUnlock()
    {
        mask.SetActive(true);
        OperatorManager.Instance.listZone[currentZone - 1].isReadyUnlock = false;
        OperatorManager.Instance.listZone[currentZone - 1].lockObject.GetComponent<Animator>().Play("unlock");
        yield return new WaitForSeconds(2.8f);
        mask.SetActive(false);
        //OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(0).gameObject.SetActive(true);
        //OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(1).gameObject.SetActive(false);
        //OperatorManager.Instance.effSmokeUnlock.transform.position = OperatorManager.Instance.listZone[currentZone - 1].lockObject.transform.position;
        //OperatorManager.Instance.effSmokeUnlock.SetActive(true);
        //OperatorManager.Instance.effSmokeUnlock.GetComponent<ParticleSystem>().Play();

        OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(1).transform.DOLocalMove(new Vector3(0, -2, 0), 0.5f).
            SetEase(Ease.Linear).OnComplete(() => OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(1).gameObject.SetActive(false));
        OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(0).gameObject.SetActive(true);
        OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(0).transform.localPosition = new Vector3(0, -3, 0);
        OperatorManager.Instance.listZone[currentZone - 1].transform.GetChild(0).transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);

        GameUIManager.Instance.SpendMoney(DataManager.Instance.upgradeDatas.Unlock_Data[currentZone - 1 + 3 * LoadingManager.Instance.currentMap].cost);
        GameManager.Instance.SetUpShip(currentZone - 1);
        GameManager.Instance.SetUpWorkerForZone(currentZone - 1);
        DataManager.Instance.listOperatorUnlock[currentZone + 3 * LoadingManager.Instance.currentMap - 1] = 1;
        OperatorManager.Instance.listOperators[currentZone - 1].isUnlock = true;
        panelUnlock.SetActive(false);
        bgUnlock.SetActive(false);
        listUpgradePanel[currentZone - 1].SetActive(true);
        textUpgrade.transform.parent.gameObject.SetActive(true);
        SendEventFirebase.ProgressionEvent("Unlock new dock", 0, "Workshop", currentZone);
        if(currentZone == 3)
        {
            UnlockMap();
        }
    }

    public void UnlockMap()
    {
        DataManager.Instance.isMenuMapUnlocked = 1;
        GameUIManager.Instance.textMapUnlocked.SetActive(true);
        GameUIManager.Instance.buttonMapLock.SetActive(false);
        GameUIManager.Instance.textMapUnlocked.transform.DOLocalMove(GameUIManager.Instance.textMapUnlocked.transform.localPosition + new Vector3(0, 100, 0), 1f).SetEase(Ease.Linear);
        GameUIManager.Instance.textMapUnlocked.GetComponent<Text>().DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.Linear).OnComplete(() => GameUIManager.Instance.textMapUnlocked.SetActive(false));

        if(mMapUnlockItem != null)
        {
            mMapUnlockItem.SetActive(true);
            Invoke("HideMapUnlockNitify", 3.5f);
        }
    }

    private void HideMapUnlockNitify()
    {
        mMapUnlockItem.SetActive(false);
    }    


    public void OnClickNextZone()
    {
        if(DataManager.Instance.numTutorialStep > 2)
        {
            OperatorManager.Instance.listOperators[currentZone - 1].isOpen = false;
            OperatorManager.Instance.listZone[currentZone - 1].isOpen = false;
            if (currentZone < OperatorManager.Instance.listOperators.Count)
            {
                ShowUpgradeZone(currentZone + 1);
            }
            else
            {
                ShowUpgradeZone(1);
            }
            OperatorManager.Instance.listOperators[currentZone - 1].isOpen = true;
            OperatorManager.Instance.listZone[currentZone - 1].isOpen = true;
        }
    }

    public void OnClickPreviousZone()
    {
        if (DataManager.Instance.numTutorialStep > 2)
        {
            OperatorManager.Instance.listOperators[currentZone - 1].isOpen = false;
            OperatorManager.Instance.listZone[currentZone - 1].isOpen = false;
            if (currentZone > 1)
            {
                ShowUpgradeZone(currentZone - 1);
            }
            else
            {
                ShowUpgradeZone(OperatorManager.Instance.listOperators.Count);
            }
            OperatorManager.Instance.listOperators[currentZone - 1].isOpen = true;
            OperatorManager.Instance.listZone[currentZone - 1].isOpen = true;
        }
    }

    public void OnClickNumUpgrade()
    {
        if (DataManager.Instance.numTutorialStep > 2)
        {
            if (DataManager.Instance.numUpgrade == 1)
            {
                DataManager.Instance.numUpgrade = 10;
            }
            else if (DataManager.Instance.numUpgrade == 10)
            {
                DataManager.Instance.numUpgrade = 50;
            }
            else if (DataManager.Instance.numUpgrade == 50)
            {
                DataManager.Instance.numUpgrade = 1;
            }
            textUpgrade.text = "x" + DataManager.Instance.numUpgrade;
            textUpgradeSale.text = "x" + DataManager.Instance.numUpgrade;
            GameUIManager.Instance.RefreshUI();
            SaleManager.Instance.RefreshUI();
        }
    }
}
