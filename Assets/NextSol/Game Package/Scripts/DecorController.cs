using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorController : MonoBehaviour
{
    public int workZone;
    public bool isMove = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            //transform.position = GameManager.Instance.craneObject.transform.position - new Vector3(0, distance, 0);
            transform.position = TruckController.Instance.listPickObject[workZone].transform.position - new Vector3(0, distance, 0);
            //transform.rotation = GameManager.Instance.craneObject.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "crane")
        {
            isMove = true;
            distance = col.gameObject.transform.position.y - transform.position.y;
            //TruckController.Instance.TruckReturn();
            TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().truckObject.GetComponent<TransportCarController>().TruckReturn(workZone);
            //StruckController.Instance.truckObject.GetComponent<TransportCarController>().TruckReturn(workZone);
        }
    }
}
