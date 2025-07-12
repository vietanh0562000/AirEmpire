using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SaleHouseController : MonoBehaviour
{
    public static SaleHouseController Instance;
    public Animator anim;
    public bool isClick = false;
    private float timeClick;

    // Start is called before the first frame update
    void Start()
    {
        //SaleManager.Instance.saleHouseObject.GetComponent<Outline>().enabled = false;
        Instance = this;
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
                if (!PopupSaleUpgrade.Instance.isOpen && !SaleManager.Instance.isOpen)
                {
                    if (timeClick < 0.3f)
                    {
                        anim.Play("selectnha2");
                        PopupSaleUpgrade.Instance.isOpen = true;
                        //SaleManager.Instance.saleHouseObject.GetComponent<Outline>().enabled = true;
                        GameUIManager.Instance.popupSaleUpgrade.gameObject.SetActive(true);
                        var tweener = SaleManager.Instance.panelUpgrade.transform.DOLocalMove(SaleManager.Instance.panelUpgrade.transform.localPosition + new Vector3(0, 1000, 0), 0.3f).SetEase(Ease.Linear);
                        SaleManager.Instance.buttonClose.gameObject.SetActive(true);
                        StartCoroutine(CoWaitOpen(tweener));
                    }
                }
            }
        }  
    }

    IEnumerator CoWaitOpen(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        SaleManager.Instance.isOpen = true;
    }
}
