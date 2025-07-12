using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorkerCelebController : MonoBehaviour
{
    public int workZone;
    public int numWork;
    public Animator anim;
    //public GameObject effectObject, effectObject2;
    //public GameObject trail;
    public List<Vector3> listWorkingPath, listReturnPath;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        SetWorking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWorkPath()
    {
        listWorkingPath.Clear();

        //GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork[
        //    GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork.Count - 1]
        for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork.Count - numWork; i++)
        {
            //GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork[
            //GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork.Count - 1].transform.position =
            //new Vector3()
            listWorkingPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebWork[i].transform.position);
        }
        listWorkingPath.Add(new Vector3(listWorkingPath[listWorkingPath.Count - 1].x - 0.01f, listWorkingPath[listWorkingPath.Count - 1].y, listWorkingPath[listWorkingPath.Count - 1].z));
    }

    public void SetWorking()
    {
        SetWorkPath();
        anim.Play("walk");
        transform.position = listWorkingPath[0];
        var tweener = transform.DOPath(listWorkingPath.ToArray(), 4).SetEase(Ease.Linear).SetLookAt(0);
        StartCoroutine(CoWaitToCeleb(tweener));
    }

    IEnumerator CoWaitToCeleb(Tweener tweener)
    {
        yield return tweener.WaitForCompletion();
        anim.Play("celeb");
        yield return new WaitForSeconds(5);
        SetReturn();
    }

    public void SetReturnPath()
    {
        listReturnPath.Clear();
        listReturnPath.Add(transform.position);
        for (int i = 0; i < GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebReturn.Count; i++)
        {
            listReturnPath.Add(GameManager.Instance.listShipWorking[workZone].GetComponent<ShipController>().listPosCelebReturn[i].transform.position);
        }        
    }

    public void SetReturn()
    {
        SetReturnPath();
        anim.Play("walk");
        transform.position = listReturnPath[0];
        transform.DOPath(listReturnPath.ToArray(), 4).SetEase(Ease.Linear).SetLookAt(0).OnComplete(() => Destroy(gameObject));
        
    }
}
