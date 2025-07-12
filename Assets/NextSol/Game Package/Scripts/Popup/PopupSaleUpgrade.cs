using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupSaleUpgrade : MonoBehaviour
{
    public static PopupSaleUpgrade Instance;
    public bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClose()
    {
        if (SaleManager.Instance.isOpen)
        {
            isOpen = false;
            var tweener = SaleManager.Instance.panelUpgrade.transform.DOLocalMove(SaleManager.Instance.panelUpgrade.transform.localPosition - new Vector3(0, 1000, 0), 0.3f).SetEase(Ease.Linear);
            SaleManager.Instance.buttonClose.gameObject.SetActive(false);
            //SaleManager.Instance.saleHouseObject.GetComponent<Outline>().enabled = false;
            SaleManager.Instance.buttonClose.gameObject.SetActive(false);
            StartCoroutine(CoWaitClose(tweener));
        }
    }

    IEnumerator CoWaitClose(Tweener tweener)
    {
        SaleHouseController.Instance.anim.Play("arrow");
        yield return tweener.WaitForCompletion();
        SaleManager.Instance.isOpen = false;
    }
}
