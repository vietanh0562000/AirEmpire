using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BosterController : MonoBehaviour
{
    public Image bgimage;
    public int count;
    // Start is called before the first frame update
    private void OnEnable()
    {
        count = 15;
        CoutingHide(count);
        StartCoroutine(HideGO(count));
    }
    private void OnDisable()
    {
        count = 15;
        bgimage.fillAmount = 1;
    }
    public void CoutingHide(int count)
    {
        //for (int i = count * 20; i > 0; i--)
        //{
        //    bgimage.fillAmount = (float)i / (count * 20);
        //    yield return new WaitForSeconds(0.05f);
        //}
        bgimage.DOFillAmount(0, count).SetEase(Ease.Linear);
    }
    IEnumerator HideGO(float time)
    {
        yield return new WaitForSeconds(time);
        //if (gameObject.CompareTag("BTTspeedx2"))
        //{
        //    GameManager.Instance.timeshownextspeedup = Time.time + 180;
        //}
        //else if (gameObject.CompareTag("BTTgoldx3"))
        //{
        //    GameManager.Instance.timeshownextx3 = Time.time + 180;
        //}
        //gameObject.transform.DOLocalMove(gameObject.transform.localPosition - new Vector3(300, 0, 0), 0.5f).SetEase(Ease.InBack).OnComplete(()=>gameObject.SetActive(false));
    }
}
