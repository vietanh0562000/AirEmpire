using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
    public GameObject posNear, posFar;
    public List<GameObject> listPillarObject;

    [Header("--Pos Low--")]
    public List<GameObject> listWorkingPillarPos;
    public List<GameObject> listWorkingPillarPos2;
    public List<GameObject> listReturnPillarPos;
    public List<GameObject> listReturnPillarPos2;

    [Header("--Pos High--")]
    public GameObject highPathObject;
    public List<GameObject> listHighWorkingPos;
    public List<GameObject> listHighReturnPos;
    public List<GameObject> listHighWorkingPos2;
    public List<GameObject> listHighReturnPos2;

    [Header("--Pos Celeb--")]
    public List<GameObject> listPosCelebWork;
    public List<GameObject> listPosCelebReturn;

    [Header("--Pos Landing--")]
    public List<GameObject> listPosLanding;

    [HideInInspector]
    public int numPartTransported = 0, numPartCompleted = 0;
    public int numFinishSanGoLow, numFinishSanGoHigh, numFinishVoTauLow, numFinishVoTauHigh, numFinishSanTau;

    //public GameObject shipCompletedObject;
    //public GameObject truckObject;
    public float speed = 10;
    public List<Vector3> listPathParking;
    public bool isTest = false;
    public float timeTest = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //timeTest += Time.deltaTime;

        //if (isTest)
        //{
        //    if(timeTest > 1)
        //    {
        //        SetShipGo(0);
        //        timeTest = -100;
        //    }
        //}
    }

    public float GetDistancePath(List<Vector3> listPos)
    {
        float totalDistance = 0;
        for (int i = 0; i < listPos.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(listPos[i], listPos[i + 1]);
        }
        return totalDistance;
    }

    public float GetTimeTransport(List<Vector3> listPos)
    {
        return GetDistancePath(listPos) / speed;
    }

    public float GetTimeTransport2Point(Vector3 start, Vector3 des)
    {
        float distance = Vector3.Distance(start, des);
        float time = distance / speed;
        return time;
    }

    //public void SetShipGo(int zone)
    //{        
    //    StartCoroutine(CoWaitShipGo(zone));
    //}

    //IEnumerator CoWaitShipGo(int zone)
    //{
    //    //transform.GetChild(0).gameObject.SetActive(true);
    //    //float time1 = GetTimeTransport2Point(transform.GetChild(0).gameObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position);
    //    //transform.GetChild(0).gameObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position, time1).SetEase(Ease.Linear);
    //    //yield return new WaitForSeconds(time1);
    //    //float time2 = GetTimeTransport2Point(transform.GetChild(0).gameObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.3f, 0));
    //    //transform.GetChild(1).gameObject.SetActive(false);
    //    //yield return new WaitForSeconds(0.5f);
    //    //transform.GetChild(0).gameObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.3f, 0), 2).SetEase(Ease.Linear);
    //    //yield return new WaitForSeconds(2);
    //    //Vector3 pos1 = new Vector3(transform.GetChild(0).gameObject.transform.position.x, transform.GetChild(0).gameObject.transform.position.y, SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position.z);
    //    //float time3 = GetTimeTransport2Point(transform.GetChild(0).gameObject.transform.position, pos1);
    //    //transform.GetChild(0).gameObject.transform.DOMove(pos1, time3).SetEase(Ease.Linear);
    //    //yield return new WaitForSeconds(time3 + 1);
    //    //float time4 = GetTimeTransport2Point(transform.GetChild(0).gameObject.transform.position, SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position);
    //    //transform.GetChild(0).gameObject.transform.DOMove(SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position, time4).SetEase(Ease.Linear);
    //    SaleManager.Instance.numParking++;
    //    shipCompletedObject.SetActive(true);
    //    float time1 = GetTimeTransport2Point(shipCompletedObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position);
    //    shipCompletedObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position, time1).SetEase(Ease.Linear);
    //    yield return new WaitForSeconds(time1);
    //    float time2 = GetTimeTransport2Point(shipCompletedObject.transform.position, SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.3f, 0));
    //    truckObject.SetActive(false);
    //    yield return new WaitForSeconds(0.5f);
    //    shipCompletedObject.transform.DOMove(SaleManager.Instance.listPosFirstZone[zone].transform.position - new Vector3(0, 0.3f, 0), 2).SetEase(Ease.Linear);
    //    SaleManager.Instance.listLaunchShipObject[zone].transform.DOMove(SaleManager.Instance.listLaunchShipObject[zone].transform.position - new Vector3(0, 0.4f, 0), 2).SetEase(Ease.Linear);
    //    yield return new WaitForSeconds(1);
    //    Debug.Log("up");
    //    SaleManager.Instance.listGateAnim[zone].Play("up");
    //    yield return new WaitForSeconds(1);
    //    Vector3 pos1 = new Vector3(shipCompletedObject.transform.position.x, shipCompletedObject.transform.position.y, SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position.z);
    //    float time3 = GetTimeTransport2Point(shipCompletedObject.transform.position, pos1);
    //    shipCompletedObject.transform.DOMove(pos1, time3).SetEase(Ease.Linear);
    //    yield return new WaitForSeconds(time3 + 1);
    //    SaleManager.Instance.listGateAnim[zone].Play("down");
    //    float time4 = GetTimeTransport2Point(shipCompletedObject.transform.position, SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position);
    //    shipCompletedObject.transform.DOMove(SaleManager.Instance.listPosParking[SaleManager.Instance.numParking - 1].transform.position, time4).SetEase(Ease.Linear);
    //}

    IEnumerator CoWaitToPark(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
    }
}
