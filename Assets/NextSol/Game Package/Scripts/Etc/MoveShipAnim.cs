using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShipAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAnim()
    {
        transform.parent.gameObject.SetActive(false);
        transform.parent.transform.localPosition = Vector3.zero;
    }
}
