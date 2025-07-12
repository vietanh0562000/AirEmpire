using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TruckController : MonoBehaviour
{
    public static TruckController Instance;
    public List<GameObject> listCraneObject;
    public List<GameObject> listPickObject;
    public bool[] listIsFinishLoop;
    public bool isfinishloop;
    public List<GameObject> listTruckObject;
    public List<GameObject> listFirstDecor;
    public List<GameObject> listWorkerDecor;
    [HideInInspector]
    public GameObject truckObject;

    public List<GameObject> listTruckPos;
    public List<GameObject> listTruckPos2;
    public List<GameObject> listTruckPos3;
    [HideInInspector]
    public List<Vector3> listTruckPath, listTruckReturnPath;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SetTruckPath();
        SetTruckReturnPath();
        isfinishloop = true;
        listIsFinishLoop = new bool[3];
        for (int i = 0; i < listIsFinishLoop.Length; i++)
        {
            listIsFinishLoop[i] = true;
        }

        //listCraneObject[1].GetComponent<StruckController>().
        //        truckObject.GetComponent<TransportCarController>().TransportDecor(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTruckPath()
    {
        for (int i = 0; i < listTruckPos.Count; i++)
        {
            listTruckPath.Add(listTruckPos[i].transform.position);
        }
    }

    public void SetTruckPath2(int workZone)
    {
        listTruckPath.Clear();
        for (int i = 0; i < listTruckPos.Count; i++)
        {
            listTruckPath.Add(listTruckPos[i].transform.position);
        }
    }

    public void SetTruckReturnPath()
    {
        for (int i = listTruckPos.Count - 1; i >= 0; i--)
        {
            listTruckReturnPath.Add(listTruckPos[i].transform.position);
        }
    }
    public void SetTruckReturnPath2(int workZone)
    {
        listTruckReturnPath.Clear();
        if(workZone == 0)
        {
            for (int i = listTruckPos.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(listTruckPos[i].transform.position);
            }
        }
        else if(workZone == 1)
        {
            for (int i = listTruckPos2.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(listTruckPos2[i].transform.position);
            }
        }
        else if (workZone == 2)
        {
            for (int i = listTruckPos3.Count - 1; i >= 0; i--)
            {
                listTruckReturnPath.Add(listTruckPos3[i].transform.position);
            }
        }
    }


    public void SetActiveDecor(int index)
    {
        Debug.Log(GameManager.Instance.listDecorTransportObject.Count);
        for (int i = 0; i < GameManager.Instance.listDecorTransportObject.Count; i++) {
            GameManager.Instance.listDecorTransportObject[i].SetActive(false);
        }
        GameManager.Instance.listDecorTransportObject[GameManager.Instance.indexDecor].SetActive(true);
    }

    public void SetActiveDecor2(int workZone)
    {
        Debug.Log(GameManager.Instance.listListDecorTransportObject[workZone].Count);
        for (int i = 0; i < GameManager.Instance.listListDecorTransportObject[workZone].Count; i++)
        {
            GameManager.Instance.listListDecorTransportObject[workZone][i].SetActive(false);
        }
        GameManager.Instance.listListDecorTransportObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].SetActive(true);
    }

    public void TransportDecor()
    {
        if (isfinishloop)
        {
            isfinishloop = false;
            SetActiveDecor(GameManager.Instance.indexDecor);
            truckObject.transform.position = listTruckPath[0];
            truckObject.transform.DOPath(listTruckPath.ToArray(), 3).SetEase(Ease.Linear)/*.OnComplete( ()=> { isfinishloop = true; })*/;
            //if (!CraneController.Instance.craneAnimator.enabled)
            //{
            //    StartCoroutine(CoWaitTransprortDecor(3));
            //}
            StartCoroutine(CoWaitTransprortDecor(3));
        }       
    }

    public void TransportDecor2(int workZone)
    {

        if (listIsFinishLoop[workZone])
        {
            SetTruckPath2(workZone);
            listIsFinishLoop[workZone] = false;
            SetActiveDecor2(workZone);
            listTruckObject[workZone].transform.position = listTruckPath[0];
            listTruckObject[workZone].transform.DOPath(listTruckPath.ToArray(), 3).SetEase(Ease.Linear)/*.OnComplete( ()=> { isfinishloop = true; })*/;
            StartCoroutine(CoWaitTransprortDecor(3));
        }
    }

    IEnumerator CoWaitTransprortDecor(float delay)
    {
        yield return new WaitForSeconds(delay);
        //CraneController.Instance.craneAnimator.enabled = true;
        StruckController.Instance.RunCrane();
    }

    public void TruckReturn()
    {
        truckObject.transform.DOPath(listTruckReturnPath.ToArray(), 3).SetEase(Ease.Linear)/*.OnComplete(() => { isfinishloop = true;})*/;
    }
}
