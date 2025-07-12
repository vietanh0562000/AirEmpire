using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ZoneController : MonoBehaviour
{
    public int zone;

    private float timeClick;
    private bool isClick = false;
    public bool isOpen = false;
    public bool isUnlock = false;

    public GameObject lockObject;
    public bool isReadyUnlock;

    // Start is called before the first frame update
    void Start()
    {
        isUnlock = DataManager.Instance.listOperatorUnlock[zone + 3 * LoadingManager.Instance.currentMap - 1] == 1;
        if (zone == 1)
        {
            isUnlock = true;
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(isUnlock);
        gameObject.transform.GetChild(1).gameObject.SetActive(!isUnlock);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            timeClick += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        isClick = true;
        timeClick = 0;
    }

    private void OnMouseUp()
    {
        isClick = false;
    }

    private void OnMouseUpAsButton()
    {
        if(DataManager.Instance.numTutorialStep > 3)
        {
            if (!GameManager.Instance.IsMouseOverUI())
            {
                if (DataManager.Instance.listOperatorUnlock[zone + 3 * LoadingManager.Instance.currentMap - 1] == 0)
                {
                    if (!PopupUpgrade.Instance.isOpenDone && !isOpen)
                    {
                        if (timeClick < 0.2f)
                        {
                            //gameObject.GetComponent<Outline>().enabled = true;
                            GameUIManager.Instance.popupUpgrade.SetActive(true);
                            var tweener = PopupUpgrade.Instance.panelUpgrade.transform.DOLocalMove(PopupUpgrade.Instance.panelUpgrade.transform.localPosition + new Vector3(0, 1000, 0), 0.5f).SetEase(Ease.Linear);
                            //PopupUpgrade.Instance.buttonClose.gameObject.SetActive(true);
                            PopupUpgrade.Instance.ShowUpgradeZone(zone);
                            StartCoroutine(CoWaitOpen(tweener));
                        }
                    }
                }
            }
        }       
    }

    IEnumerator CoWaitOpen(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        isOpen = true;
    }
}
