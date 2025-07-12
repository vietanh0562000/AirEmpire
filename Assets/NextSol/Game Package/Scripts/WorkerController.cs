using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorkerController : MonoBehaviour
{
    public int workZone;
    public Animator anim;
    public GameObject effectObject, effectObject2;
    public GameObject trail;
    public List<Vector3> listWorkingPath, listReturnPath;
    public float speed = 10;
    public int numPartWorking;
    public int numEffect = 1;
    public bool isLanding = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isLanding)
        {
            SetLanding();
        }
        else
        {
            SetWorkingPath();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLanding()
    {
        Debug.Log("landing");
        anim.speed = 1;
        anim.Play("appear");
        transform.eulerAngles = new Vector3(0, -90, 0);
        transform.position = GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[0].transform.position;
        transform.DOMove(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[1].transform.position, 2f).SetEase(Ease.Linear).OnComplete(() => SetWorkingPath());
        //yield return new WaitForSeconds(2);
        //anim.Play("worker2move");
        //SetWorkingPath()
        //.OnComplete(() => SetWorkingPath())
    }

    public void SetWorking()
    {
        //Debug.Log("working");
        SetWorkingPillarPath();
        transform.position = listWorkingPath[0];
        var tweener = transform.DOPath(listWorkingPath.ToArray(), GetTimeTransport(listWorkingPath)).SetEase(Ease.Linear).SetLookAt(0);
        //StartCoroutine(CoWaitCompleteLowPart(GetTimeTransport(listWorkingPath)));
        StartCoroutine(CoWaitCompleteLowPart(tweener));

        //GameManager.Instance.listTweener.Add(tweener);
        GameManager.Instance.listListTweener[workZone].Add(tweener);

        //GameManager.Instance.UpgradeSpeed();
        GameManager.Instance.UpgradeSpeedForZone(workZone);
    }

    IEnumerator CoWaitCompleteLowPart(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        //yield return new WaitForSeconds(delay);
        anim.Play("worker2working");
        if (numEffect == 1)
        {
            effectObject.SetActive(true);
        }
        else
        {
            effectObject2.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        GameManager.Instance.CompletePart(workZone, numPartWorking);
        yield return new WaitForSeconds(1);
        if (numEffect == 1)
        {
            effectObject.SetActive(false);
        }
        else
        {
            effectObject2.SetActive(false);
        }
        SetReturnFactory();
        anim.Play("worker2move");
        GameUIManager.Instance.InitMoney(gameObject, workZone);
    }

    public void SetReturnFactory()
    {
        SetReturnPillarPath();
        Tweener tweener = transform.DOPath(listReturnPath.ToArray(), GetTimeTransport(listReturnPath)).SetEase(Ease.Linear).SetLookAt(0);
        //StartCoroutine(CoWaitReturnFactory(GetTimeTransport(listReturnPath)));
        StartCoroutine(CoWaitReturnFactory(tweener));

        //GameManager.Instance.listTweener.Add(tweener);
        GameManager.Instance.listListTweener[workZone].Add(tweener);
        //GameManager.Instance.UpgradeSpeed();
        GameManager.Instance.UpgradeSpeedForZone(workZone);
    }

    IEnumerator CoWaitReturnFactory(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        yield return new WaitForSeconds(1);


        if (DataManager.Instance.numTutorialStep < 3)
        {
            if(DataManager.Instance.numStep1 < 2)
            {
                TutorialManager.Instance.ActivePanelTutorial(1);
                TutorialManager.Instance.effHandPointer.SetActive(true);
                GameManager.Instance.listListWorker[workZone].Remove(gameObject);
                GameManager.Instance.listWorker.Remove(gameObject);
                Destroy(gameObject, 1);
            }
        }
        else
        {
            SetWorkingPath();
        }
    }

    public void SetWorkingHigh()
    {
        //SetWorkingPillarPath();
        SetWorkingHighPath();
        transform.position = listWorkingPath[0];
        var tweener = transform.DOPath(listWorkingPath.ToArray(), GetTimeTransport(listWorkingPath)).SetEase(Ease.Linear).SetLookAt(0);
        //StartCoroutine(CoWaitCompleteHighPart(GetTimeTransport(listWorkingPath)));
        StartCoroutine(CoWaitCompleteHighPart(tweener));

        //if (!GameManager.Instance.highPathObject.activeSelf)
        //{
        //    GameManager.Instance.highPathObject.SetActive(true);
        //}

        if (!GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().highPathObject.activeSelf)
        {
            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().highPathObject.SetActive(true);
        }

        //GameManager.Instance.listTweener.Add(tweener);
        GameManager.Instance.listListTweener[workZone].Add(tweener);
        //GameManager.Instance.UpgradeSpeed();
        GameManager.Instance.UpgradeSpeedForZone(workZone);
    }

    IEnumerator CoWaitCompleteHighPart(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        anim.Play("worker2working");
        //yield return new WaitForSeconds(delay);
        if (numEffect == 1)
        {
            effectObject.SetActive(true);
        }
        else
        {
            effectObject2.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        GameManager.Instance.CompletePart(workZone, numPartWorking);
        yield return new WaitForSeconds(1);
        if (numEffect == 1)
        {
            effectObject.SetActive(false);
        }
        else
        {
            effectObject2.SetActive(false);
        }
        //GameManager.Instance.CompletePillarPart(numPartWorking);
        SetReturnFactoryHigh();
        anim.Play("worker2move");
        GameUIManager.Instance.InitMoney(gameObject, workZone);
    }

    public void SetReturnFactoryHigh()
    {
        //SetReturnPillarPath();
        SetReturnHighPath();
        var tweener = transform.DOPath(listReturnPath.ToArray(), GetTimeTransport(listReturnPath)).SetEase(Ease.Linear).SetLookAt(0);
        //StartCoroutine(CoWaitReturnFactoryHigh(GetTimeTransport(listReturnPath)));
        StartCoroutine(CoWaitReturnFactoryHigh(tweener));

        //GameManager.Instance.listTweener.Add(tweener);
        GameManager.Instance.listListTweener[workZone].Add(tweener);
        //GameManager.Instance.UpgradeSpeed();
        GameManager.Instance.UpgradeSpeedForZone(workZone);
    }

    IEnumerator CoWaitReturnFactoryHigh(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        yield return new WaitForSeconds(1);

        SetWorkingPath();
    }

    public void SetWorkingPath()
    {
        anim.Play("worker2move");
        OperatorManager.Instance.listOperators[workZone].isWorkingDone = false;
        if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numFinishSanGoLow)
        {
            SetWorking();
            numEffect = 1;
        }
        else if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numFinishSanGoHigh)
        {
            SetWorkingHigh();
            numEffect = 1;
        }
        else if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numFinishVoTauLow)
        {
            SetWorking();
            numEffect = 1;
        }
        else if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numFinishVoTauHigh)
        {
            SetWorkingHigh();
            numEffect = 1;
        }
        else if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numFinishSanTau)
        {
            SetWorkingHigh();
            numEffect = 2;
        }
        else if (GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject.Count)
        {
            SetWorkingHigh();
            numEffect = 1;
        }

        if ((GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported == GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject.Count) && GameManager.Instance.listNumPartDecorCompleted[workZone] < GameManager.Instance.listListDecorObject[workZone].Count)
        {
            DataManager.Instance.listZoneFinish[workZone + 3 * LoadingManager.Instance.currentMap] = 1;
            TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().workZone = workZone;
            TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().
                truckObject.GetComponent<TransportCarController>().TransportDecor(workZone);

            GameManager.Instance.listListWorker[workZone].Remove(gameObject);
            GameManager.Instance.listWorker.Remove(gameObject);
            Destroy(gameObject, 1);
        }
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

    public void SetWorkingPillarPath()
    {
        listWorkingPath.Clear();

        Vector3 pos = GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position;
        if (Vector3.Distance(pos, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posNear.transform.position) < Vector3.Distance(pos, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posFar.transform.position))
        {
            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 2].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 2].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 2].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 1].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 1].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count - 1].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            if (isLanding)
            {
                listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[1].transform.position);
                for (int i = 1; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[i].transform.position);
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos[i].transform.position);
                }
            }
        }
        else
        {
            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 2].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 2].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 2].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 1].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 1].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count - 1].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            if (isLanding)
            {
                listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[1].transform.position);
                for (int i = 1; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[i].transform.position);
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listWorkingPillarPos2[i].transform.position);
                }
            }
        }
        //numPartWorking = GameManager.Instance.numPartTransported;
        //GameManager.Instance.numPartTransported++;

        numPartWorking = GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported;
        GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported++;
    }

    public void SetReturnPillarPath()
    {
        isLanding = false;

        listReturnPath.Clear();

        listReturnPath.Add(transform.position);

        if (Vector3.Distance(transform.position, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posNear.transform.position) < Vector3.Distance(transform.position, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posFar.transform.position))
        {
            for (int i = 0; i < GameManager.Instance.listShipWorking[workZone]
                .GetComponent<ShipController>().listReturnPillarPos.Count; i++)
            {
                listReturnPath.Add(GameManager.Instance.listShipWorking[workZone]
                .GetComponent<ShipController>().listReturnPillarPos[i].transform.position);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.listShipWorking[workZone]
                .GetComponent<ShipController>().listReturnPillarPos2.Count; i++)
            {
                listReturnPath.Add(GameManager.Instance.listShipWorking[workZone]
                .GetComponent<ShipController>().listReturnPillarPos2[i].transform.position);
            }
        }
    }

    public void SetWorkingHighPath()
    {
        listWorkingPath.Clear();

        Vector3 pos = GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position;

        if (Vector3.Distance(pos, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posNear.transform.position) < Vector3.Distance(pos, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posFar.transform.position))
        {
            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 2].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 2].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 2].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 1].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 1].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count - 1].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            if (isLanding)
            {
                listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[1].transform.position);
                for (int i = 1; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[i].transform.position);
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos[i].transform.position);
                }
            }
            
        }
        else
        {
            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 2].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 2].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 2].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 1].transform.position =
                new Vector3(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 1].transform.position.x,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count - 1].transform.position.y,
                GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported].transform.position.z);

            if (isLanding)
            {
                listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosLanding[1].transform.position);
                for (int i = 1; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[i].transform.position);
                }
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2.Count; i++)
                {
                    listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighWorkingPos2[i].transform.position);
                }
            }  
        }

        numPartWorking = GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported;
        GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().numPartTransported++;
    }

    public void SetReturnHighPath()
    {
        isLanding = false;

        listReturnPath.Clear();

        listReturnPath.Add(transform.position);

        if (Vector3.Distance(transform.position, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posNear.transform.position) < Vector3.Distance(transform.position, GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().posFar.transform.position))
        {
            for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighReturnPos.Count; i++)
            {
                listReturnPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighReturnPos[i].transform.position);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighReturnPos2.Count; i++)
            {
                listReturnPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listHighReturnPos2[i].transform.position);
            }
        }
    }
}
