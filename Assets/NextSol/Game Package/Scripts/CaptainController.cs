using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CaptainController : MonoBehaviour
{
    public GameObject hiredObject;
    public GameObject unhiredObject;
    public Animator anim;
    public int workZone;
    //public bool isHired = false;
    public bool isOpen = false;
    public float boostSpeed, boostIncome;

    private float timeClick;
    private bool isClick = false;

    // Start is called before the first frame update
    void Start()
    {
        if(DataManager.Instance.listHireCaptain[workZone + 3 * LoadingManager.Instance.currentMap] == 1)
        {
            //anim.Play("work");
            LoadCaptainData();
            hiredObject.SetActive(true);
            unhiredObject.SetActive(false);
        }
        else
        {
            //anim.Play("idle");
            hiredObject.SetActive(false);
            if (DataManager.Instance.numTutorialStep < 3)
            {
                unhiredObject.SetActive(false);
            }
            else
            {
                unhiredObject.SetActive(true);
            }
        }
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

    public void LoadCaptainData()
    {
        if (DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].type == 0)
        {
            boostSpeed = DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone + 3 * LoadingManager.Instance.currentMap]].boost / 100;
            boostIncome = 0;
        }
        else if (DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].type == 1)
        {
            boostSpeed = 0;
            boostIncome = DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone + 3 * LoadingManager.Instance.currentMap]].boost / 100;
        }
        GameManager.Instance.UpgradeSpeedForZone(workZone);
    }

    private void OnMouseUpAsButton()
    {
        if(DataManager.Instance.numTutorialStep > 2)
        {
            if (!GameManager.Instance.IsMouseOverUI())
            {
                if (!isOpen && timeClick < 0.2f)
                {
                    PopupCaptain.Instance.zone = workZone;
                    GameUIManager.Instance.popupCaptain.SetActive(true);

                    PopupCaptain.Instance.buttonClose.gameObject.SetActive(true);
                    if (workZone == 3)
                    {
                        PopupCaptain.Instance.textTitle.text = "Sale Captain";
                    }
                    else
                    {
                        PopupCaptain.Instance.textTitle.text = "Foreman " + (workZone + 1);
                    }
                    var tweener = PopupCaptain.Instance.panelCaptain.transform.DOLocalMove(PopupCaptain.Instance.panelCaptain.transform.localPosition + new Vector3(0, 1000, 0), 0.5f).SetEase(Ease.Linear);

                    if (DataManager.Instance.listHireCaptain[workZone + 3 * LoadingManager.Instance.currentMap] == 1)
                    {
                        PopupCaptain.Instance.buttonHire.gameObject.SetActive(false);
                        PopupCaptain.Instance.imageHired.gameObject.SetActive(true);
                        PopupCaptain.Instance.textCaptainName.text = DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].name;
                        PopupCaptain.Instance.textCaptainBoost.text = "+" +
                            DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].boost.ToString() + "% " +
                            OperatorManager.Instance.listBoostNames[DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].type];
                    }
                    else
                    {
                        PopupCaptain.Instance.buttonHire.gameObject.SetActive(true);
                        PopupCaptain.Instance.imageHired.gameObject.SetActive(false);
                        OperatorManager.Instance.listRandomCaptainID.Clear();
                        for (int i = 0; i < DataManager.Instance.upgradeDatas.Captain_Data.Count; i++)
                        {
                            OperatorManager.Instance.listRandomCaptainID.Add(i);
                        }

                        for (int i = 0; i < OperatorManager.Instance.listCaptainIDHired.Count; i++)
                        {
                            OperatorManager.Instance.listRandomCaptainID.Remove(OperatorManager.Instance.listCaptainIDHired[i]);
                        }

                        int rand = Random.Range(0, OperatorManager.Instance.listRandomCaptainID.Count);
                        DataManager.Instance.listCaptainID[workZone] = OperatorManager.Instance.listRandomCaptainID[rand];
                        PopupCaptain.Instance.textCaptainName.text = DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].name;
                        PopupCaptain.Instance.textCaptainBoost.text = "+" +
                            DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].boost.ToString() + "% " +
                            OperatorManager.Instance.listBoostNames[DataManager.Instance.upgradeDatas.Captain_Data[DataManager.Instance.listCaptainID[workZone]].type];
                    }
                    StartCoroutine(CoWaitOpen(tweener));

                    TutorialManager.Instance.effHandPointer.SetActive(false);
                }
            }
        }               
    }

    IEnumerator CoWaitOpen(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        isOpen = true;
        if (DataManager.Instance.numTutorialStep == 3)
        {
            TutorialManager.Instance.ActiveTutorialCaptainStep2();
        }
    }
}
