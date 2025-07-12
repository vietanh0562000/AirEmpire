using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnviController : MonoBehaviour
{
    public GameObject truck1;
    public List<GameObject> listTruck1Pos;

    public GameObject truck2;
    public List<GameObject> listTruck2Pos;

    public GameObject truck3;
    public List<GameObject> listTruck3Pos;

    public GameObject truck4;
    public List<GameObject> listTruck4Pos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetTruck1Path());
        StartCoroutine(SetTruck2Path());
        StartCoroutine(SetTruck3Path());
        StartCoroutine(SetTruck4Path());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetTruck1Path()
    {
        truck1.transform.eulerAngles = new Vector3(0, 180, 0);
        truck1.transform.position = listTruck1Pos[0].transform.position;
        truck1.transform.DOMove(listTruck1Pos[1].transform.position, 3).SetEase(Ease.Linear);
        truck1.transform.DOMove(listTruck1Pos[2].transform.position, 2).SetEase(Ease.Linear).SetDelay(4);

        //Sequence seq = DOTween.Sequence();
        //seq.Append(truck1.transform.DOMove(listTruck1Pos[1].transform.position, 3).SetEase(Ease.Linear));
        //seq.PrependInterval(4);
        //seq.Append(truck1.transform.DOMove(listTruck1Pos[2].transform.position, 2).SetEase(Ease.Linear));

        yield return new WaitForSeconds(Random.Range(7f, 9f));
        StartCoroutine(SetTruck1Path());
    }

    IEnumerator SetTruck2Path()
    {
        truck2.transform.position = listTruck2Pos[0].transform.position;
        truck2.transform.DOMove(listTruck2Pos[1].transform.position, 5).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Random.Range(6f, 8f));
        StartCoroutine(SetTruck2Path());
    }

    IEnumerator SetTruck3Path()
    {
        truck3.transform.position = listTruck3Pos[0].transform.position;
        truck3.transform.DOMove(listTruck3Pos[1].transform.position, 6).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Random.Range(7f, 9f));
        StartCoroutine(SetTruck3Path());
    }

    IEnumerator SetTruck4Path()
    {
        truck4.transform.position = listTruck4Pos[0].transform.position;
        truck4.transform.DOMove(listTruck4Pos[1].transform.position, 6).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Random.Range(7f, 9f));
        StartCoroutine(SetTruck4Path());
    }
}
