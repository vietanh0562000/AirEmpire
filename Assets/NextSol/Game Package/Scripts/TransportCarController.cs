using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransportCarController : MonoBehaviour
{
    //public int workZone;
    [HideInInspector]
    public List<Vector3> listTruckPath, listTruckTransportPath, listTruckReturnPath;

    public bool isfinishloop;

    // Start is called before the first frame update
    void Start()
    {
        isfinishloop = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TransportDecor(2);
        //}
    }

    public void SetTruckPath(int workZone)
    {
        listTruckPath.Clear();
        listTruckTransportPath.Clear();
        listTruckReturnPath.Clear();

        if (workZone == 0)
        {
            for (int i = 0; i < TruckController.Instance.listTruckPos.Count - 1; i++)
            {
                listTruckTransportPath.Add(TruckController.Instance.listTruckPos[i].transform.position);
            }
            for (int i = TruckController.Instance.listTruckPos.Count - 2; i < TruckController.Instance.listTruckPos.Count; i++)
            {
                listTruckPath.Add(TruckController.Instance.listTruckPos[i].transform.position);
            }
        }
        else if(workZone == 1)
        {
            for (int i = 0; i < TruckController.Instance.listTruckPos2.Count - 1; i++)
            {
                listTruckTransportPath.Add(TruckController.Instance.listTruckPos2[i].transform.position);
            }
            for (int i = TruckController.Instance.listTruckPos2.Count - 2; i < TruckController.Instance.listTruckPos2.Count; i++)
            {
                listTruckPath.Add(TruckController.Instance.listTruckPos2[i].transform.position);
            }
        }
        else if(workZone == 2)
        {
            for (int i = 0; i < TruckController.Instance.listTruckPos3.Count - 1; i++)
            {
                listTruckTransportPath.Add(TruckController.Instance.listTruckPos3[i].transform.position);
            }
            for (int i = TruckController.Instance.listTruckPos3.Count - 2; i < TruckController.Instance.listTruckPos3.Count; i++)
            {
                listTruckPath.Add(TruckController.Instance.listTruckPos3[i].transform.position);
            }
        }
    }

    public void SetTruckReturnPath(int workZone)
    {
        listTruckReturnPath.Clear();
        if (workZone == 0)
        {
            for (int i = TruckController.Instance.listTruckPos.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(TruckController.Instance.listTruckPos[i].transform.position);
            }
        }
        else if (workZone == 1)
        {
            for (int i = TruckController.Instance.listTruckPos2.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(TruckController.Instance.listTruckPos2[i].transform.position);
            }
        }
        else if (workZone == 2)
        {
            for (int i = TruckController.Instance.listTruckPos3.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(TruckController.Instance.listTruckPos3[i].transform.position);
            }
        }
    }

    public void SetActiveDecor2(int workZone)
    {
        for (int i = 0; i < GameManager.Instance.listListDecorTransportObject[workZone].Count; i++)
        {
            GameManager.Instance.listListDecorTransportObject[workZone][i].SetActive(false);
        }
        GameManager.Instance.listListDecorTransportObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].SetActive(true);
    }

    public void TransportDecor(int workZone)
    {
        if (isfinishloop)
        {
            SetTruckPath(workZone);
            isfinishloop = false;
            //SetActiveDecor2(workZone);
            //transform.position = listTruckPath[0];
            //transform.DOPath(listTruckPath.ToArray(), 3).SetEase(Ease.Linear)/*.OnComplete( ()=> { isfinishloop = true; })*/;
            //StartCoroutine(CoWaitTransprortDecor(workZone, 3));

            StartCoroutine(CoWaitGetDecor(workZone));
        }
    }

    IEnumerator CoWaitGetDecor(int workZone)
    {
        transform.position = listTruckTransportPath[0];
        transform.DOPath(listTruckTransportPath.ToArray(), 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2);
        TruckController.Instance.listFirstDecor[workZone].SetActive(false);
        SetActiveDecor2(workZone);
        transform.DOPath(listTruckPath.ToArray(), 2).SetEase(Ease.Linear);
        StartCoroutine(CoWaitTransprortDecor(workZone, 3));
    }

    IEnumerator CoWaitTransprortDecor(int workZone, float delay)
    {
        yield return new WaitForSeconds(delay);
        TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().RunCrane();
    }

    public void TruckReturn(int workZone)
    {
        SetTruckReturnPath(workZone);
        transform.DOPath(listTruckReturnPath.ToArray(), 3).SetEase(Ease.Linear)/*.OnComplete(() => { isfinishloop = true;})*/;
    }
}
