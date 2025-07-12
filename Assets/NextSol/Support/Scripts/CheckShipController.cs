using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckShipController : MonoBehaviour
{
    public Transform KinhLup;
    public Transform BangCheck;
    public Transform RootPath;

    Vector3[] TestPointPosition;
    // Start is called before the first frame update
    void Start()
    {
        TestPointPosition = new Vector3[RootPath.childCount];
        for (int i = 0; i < RootPath.childCount; i++)
        {
            TestPointPosition[i] = RootPath.GetChild(i).position;
        }

        StartCoroutine(XuatHien());
    }

    public void SaveReset()
    {

    }

    public void ResetState()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            XuatHien();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StarTest();
        }
    }

    IEnumerator XuatHien()
    {
        yield return new WaitForSeconds(1f);
        KinhLup.gameObject.SetActive(true);
        KinhLup.DOScale(0.1f, 1).From(true).SetEase(Ease.InOutQuint);

        BangCheck.gameObject.SetActive(true);
        BangCheck.DOScale(0.1f, 1).From(true).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(1f);
        StarTest();
    }

    public void StarTest()
    {

        //  MovePingPongY(KinhLup.GetChild(0), KinhLup.position.y, KinhLup.position.y + .2f);
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(KinhLup.DOMove(TestPointPosition[0], 1))
           .AppendInterval(3)
           .AppendCallback(() =>{
               BangCheck.GetChild(0).GetChild(0).gameObject.SetActive(true);
               BangCheck.GetChild(0).GetChild(0).DOScale(3, 1).From().SetEase(Ease.InOutQuint);
           })
           .AppendInterval(1)
         .Append(KinhLup.DOMove(TestPointPosition[1], 1))
          .AppendInterval(3)
          .AppendCallback(() => {
              BangCheck.GetChild(0).GetChild(1).gameObject.SetActive(true);
              BangCheck.GetChild(0).GetChild(1).DOScale(3, 1).From().SetEase(Ease.InOutQuint);
          })

         .Append(KinhLup.DOMove(TestPointPosition[2], 1))
         .AppendInterval(3)
         .AppendCallback(() => {
             BangCheck.GetChild(0).GetChild(2).gameObject.SetActive(true);
             BangCheck.GetChild(0).GetChild(2).DOScale(3, 1).From().SetEase(Ease.InOutQuint);
         })
         .AppendInterval(1)
            .AppendCallback(() => {
                BangCheck.GetChild(0).GetChild(0).DOScale(3, 1).From().SetEase(Ease.InOutQuint);
                BangCheck.GetChild(0).GetChild(1).DOScale(3, 1).From().SetEase(Ease.InOutQuint).SetDelay(0.5f);
                BangCheck.GetChild(0).GetChild(2).DOScale(3, 1).From().SetEase(Ease.InOutQuint).SetDelay(1f);
            });
        mySequence.Play();
        mySequence.OnComplete(() => StarTest());
    }

    public void MovePingPongY(Transform target, float from, float to)
    {
        target.DOLocalMoveY(to, 1f).OnComplete(() =>
        {
            MovePingPongY(target, to, from);
        }).SetEase(Ease.InOutSine);
    }

    //IEnumerator TestIE()
    //{
    //    float time1 = 1f;
    //    ;
    //    yield return new WaitForSeconds(time1);
    //}

    public void AnDi()
    {

        KinhLup.DOScale(0.1f, 2f).OnComplete(() =>
        {
            KinhLup.gameObject.SetActive(true);
        });


        BangCheck.DOScale(0.1f, 2f).OnComplete(() =>
        {
            BangCheck.gameObject.SetActive(true);
        });
    }
}
