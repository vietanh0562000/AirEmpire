using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StruckController : MonoBehaviour
{
    public static StruckController Instance;
    public GameObject truckObject;
    public GameObject effectSmoke;
    public int workZone;

    public Transform Root;
    public Transform TrucXoay;
    public Transform Truot;
    public Transform Moc;
    public Transform Day;

    float CurrentAngle = 0;
    bool lockRotate = false;

    bool lockMoveTruot = false;
    public float currentValue;
    public float maxValue;
    public float minValue;

    float rank;

    public float currentHigh;
    public float maxHigh;
    public float minHigh;
    float rankHigh;
    bool lockMoveHigh = false;

    Vector3 posCrane;

    public bool isFinish = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        posCrane = transform.position;
        rank = Mathf.Abs(maxValue - minValue);
        rankHigh = Mathf.Abs(maxHigh - minHigh);
        MoveTruot(0);
        MoveCau(1f);
        //isFinish = true;
    }

    public float timeTest = 0;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    // Rotation((Random.Range(0f, 1f) > 0x.5f)?1:-1, 90);
        //    // MoveTruot(1f);
        //    //MoveCau(0f);
        //    StartCoroutine(CauHangIE());
        //}


        if (isFinish)
        {
            Debug.Log("finish");

            //if (DataManager.Instance.isParkingAvailable == 1)
            //{
            //    Debug.Log("new ship");
            //    StartCoroutine(CoWaitMoveShip());
            //    isFinish = false;
            //    DataManager.Instance.isParkingAvailable = 0;
            //}

            if (DataManager.Instance.listZoneIsAvailable[workZone + 3 * LoadingManager.Instance.currentMap] == 1)
            {
                Debug.Log("new ship");
                StartCoroutine(CoWaitMoveShip());
                isFinish = false;
                DataManager.Instance.listZoneIsAvailable[workZone + 3 * LoadingManager.Instance.currentMap] = 0;
            }
        }
    }

    public void Rotation(int direction, int angle) {

        if (lockRotate) return;

        Quaternion newQat = TrucXoay.localRotation;
        CurrentAngle += direction * angle;
        Vector3 vector3 = new Vector3(0, CurrentAngle, 0);
        newQat.eulerAngles = vector3;
     
        TrucXoay.DOLocalRotateQuaternion(newQat, 2).SetEase(Ease.Linear).OnStart(()=> {
            lockRotate = true;
        }).OnComplete(() =>
        {
            lockRotate = false;
        });

    }

    public void MoveTruot(float value) // 0-1
    {
        if (lockMoveTruot) return;
        value = Mathf.Clamp(value, 0f, 1f);
        float alpha = minValue + value * rank;
        //Debug.Log("Move:" + alpha);
       // float kc = alpha - currentValue;
       // Debug.Log("Move:" + kc);
        Truot.DOLocalMoveX(alpha, 1f).OnStart(()=> { lockMoveTruot = true; }).OnComplete(()=> {
            currentValue = alpha;
            lockMoveTruot = false; });
    }

    public void MoveCau(float value)
    {
        if (lockMoveHigh) return;

        value = Mathf.Clamp(value, 0f, 1f);
        float alpha = minHigh + value * rankHigh;

        Day.DOScaleY(1-value, 1f);
        Moc.DOLocalMoveY(alpha, 1f).OnStart(() => { lockMoveHigh = true; }).OnComplete(() => {
            currentHigh = alpha;
            lockMoveHigh = false;
        });
    }

    public void SequenceCauHang()
    {
        Sequence sequence = DOTween.Sequence();

    }

    public void RunCrane()
    {
        StartCoroutine(CauHangIE());
    }

    IEnumerator CauHangIE()
    {
        yield return new WaitForSeconds(0.5f);
        MoveTruot(0.1f);
        yield return new WaitForSeconds(1.5f);
        MoveCau(0.35f);
        yield return new WaitForSeconds(1.5f);
        MoveCau(1f);
        yield return new WaitForSeconds(1.5f);
        MoveTruot(0.5f);
        yield return new WaitForSeconds(1.5f);
        Rotation(1, 180);
        yield return new WaitForSeconds(2f);
        MoveTruot(0.8f);
        FindPath();
        yield return new WaitForSeconds(1.5f);
        MoveCau(0.9f);
        yield return new WaitForSeconds(1);
        ApplyPart();
        yield return new WaitForSeconds(0.5f);
        MoveCau(1f);
        ReturnPath();
        yield return new WaitForSeconds(1);
        Rotation(1, 180);
        yield return new WaitForSeconds(2);
        //StartCoroutine(CauHangIE());
    }

    public void FindPath()
    {
        //transform.DOMove(new Vector3(transform.position.x, transform.position.y, GameManager.Instance.listDecorObject[GameManager.Instance.indexDecor].transform.position.z), 1).SetEase(Ease.Linear);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y,
            GameManager.Instance.listListDecorObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].transform.position.z), 1).SetEase(Ease.Linear);
    }

    public void ApplyPart()
    {
        effectSmoke.transform.position = GameManager.Instance.listListDecorObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].transform.position;
        effectSmoke.GetComponent<ParticleSystem>().Play();
        GameManager.Instance.listListDecorObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].SetActive(true);
        GameManager.Instance.listListDecorTransportObject[workZone][GameManager.Instance.listNumPartDecorCompleted[workZone]].SetActive(false);
        GameManager.Instance.listNumPartDecorCompleted[workZone]++;
        //TruckController.Instance.isfinishloop = true;
        //TruckController.Instance.listIsFinishLoop[workZone] = true;
        truckObject.GetComponent<TransportCarController>().isfinishloop = true;
        if (GameManager.Instance.listNumPartDecorCompleted[workZone] < GameManager.Instance.listListDecorTransportObject[workZone].Count)
        {
            truckObject.GetComponent<TransportCarController>().TransportDecor(workZone);
        }
        else
        {
            isFinish = true;
            SaleManager.Instance.numParking++;
            //DataManager.Instance.listZoneFinish[workZone + 3 * LoadingManager.Instance.currentMap] = 1;
        }
    }

    IEnumerator CoWaitMoveShip()
    {
        GameManager.Instance.listEffectCoinBlast[workZone].SetActive(true);
        yield return new WaitForSeconds(1);
        GameManager.Instance.MoveShip(workZone);
        yield return new WaitForSeconds(1);
        GameManager.Instance.listEffectCoinBlast[workZone].SetActive(false);
    }

    IEnumerator CoWaitSetUpNewShip(int workZone)
    {
        GameManager.Instance.listEffectCoinBlast[workZone].SetActive(true);
        yield return new WaitForSeconds(1);
        //GameManager.Instance.SetUpNewShip(workZone);
        yield return new WaitForSeconds(1);
        GameManager.Instance.listEffectCoinBlast[workZone].SetActive(false);
    }

    public void ReturnPath()
    {
        //transform.DOMove(new Vector3(-3.1f, transform.position.y, transform.position.z), 1).SetEase(Ease.Linear);
        transform.DOMove(posCrane, 1).SetEase(Ease.Linear);
    }
}
