using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerController : MonoBehaviour
{
    public Animator anim;
    public List<Vector3> listGoPath;
    public int numParking;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start go");
        SetGoing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGoPath()
    {
        
    }

    public void SetGoing()
    {
        anim.Play("walk");
        transform.position = SaleManager.Instance.listCustomerPathPos[0].transform.position;
        transform.DOMove(SaleManager.Instance.listCustomerPathPos[numParking + 1].transform.position,
            GetTimeTransport2Point(transform.position, SaleManager.Instance.listCustomerPathPos[numParking + 1].transform.position)).SetEase(Ease.Linear);
        StartCoroutine(SetSail());
    }

    IEnumerator SetSail()
    {
        yield return new WaitForSeconds(GetTimeTransport2Point(transform.position, SaleManager.Instance.listCustomerPathPos[numParking + 1].transform.position));
        anim.Play("talk");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public float GetTimeTransport2Point(Vector3 start, Vector3 des)
    {
        float distance = Vector3.Distance(start, des);
        float time = distance / speed;
        return time;
    }
}
