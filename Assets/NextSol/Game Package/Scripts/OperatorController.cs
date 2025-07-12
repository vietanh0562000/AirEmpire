using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OperatorController : MonoBehaviour
{
    public static OperatorController Instance;
    public Animator anim;
    public bool isHireCaptain = false;
    public bool isWorkingDone = false;
    public int zone;

    private float timeClick;
    private bool isClick = false;
    public bool isOpen = false;
    public bool isUnlock = false;

    public GameObject mGaraObj;
    public List<Mesh> mListMeshPrefab;

    //public GameObject lockObject, unlockObject;

    // Start is called before the first frame update
    void Start()
    {
        isUnlock = DataManager.Instance.listOperatorUnlock[zone + 3 * LoadingManager.Instance.currentMap - 1] == 1;
        if(zone == 1)
        {
            isUnlock = true;
        }

        int earningLvl = DataManager.Instance.listLevelEarning[zone + 3 * LoadingManager.Instance.currentMap -1];

        int meshLvl = earningLvl / 40;
        meshLvl = Mathf.Clamp(meshLvl, 0, mListMeshPrefab.Count - 1);

        mGaraObj.GetComponent<MeshFilter>().mesh = mListMeshPrefab[meshLvl];
           
        //lockObject.gameObject.SetActive(!isUnlock);
        //unlockObject.gameObject.SetActive(isUnlock);
    }

    public void OnEarningLvlUp()
    {
        int earningLvl = DataManager.Instance.listLevelEarning[zone + 3 * LoadingManager.Instance.currentMap - 1];

        int meshLvl = earningLvl / 40;
        meshLvl = Mathf.Clamp(meshLvl, 0, mListMeshPrefab.Count - 1);

        mGaraObj.GetComponent<MeshFilter>().mesh = mListMeshPrefab[meshLvl];
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
        if(DataManager.Instance.numTutorialStep > 1 && DataManager.Instance.numTutorialStep != 3)
        {
            if (!GameManager.Instance.IsMouseOverUI())
            {
                if (!PopupUpgrade.Instance.isOpenDone && !isOpen)
                {
                    if (timeClick < 0.2f)
                    {
                        anim.Play("selectnha2");
                        GameUIManager.Instance.popupUpgrade.SetActive(true);
                        var tweener = PopupUpgrade.Instance.panelUpgrade.transform.DOLocalMove(PopupUpgrade.Instance.panelUpgrade.transform.localPosition + new Vector3(0, 1000, 0), 0.3f).SetEase(Ease.Linear);
                        //PopupUpgrade.Instance.buttonClose.gameObject.SetActive(true);
                        PopupUpgrade.Instance.ShowUpgradeZone(zone);
                        StartCoroutine(CoWaitOpen(tweener));

                        TutorialManager.Instance.effHandPointer.SetActive(false);
                    }
                }
            }
        }
    }

    IEnumerator CoWaitOpen(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        isOpen = true;
        if (DataManager.Instance.numTutorialStep == 2)
        {
            TutorialManager.Instance.ActivePanelUpgrade(1);
            TutorialManager.Instance.effPointerHouse.SetActive(false);
            TutorialManager.Instance.effPointerUI.SetActive(true);
            TutorialManager.Instance.effPointerUI.transform.position = GameUIManager.Instance.listListUpgrade[0][2].GetComponent<UpgradeController>().buttonUpgrade.transform.position;
        }
    }
}
