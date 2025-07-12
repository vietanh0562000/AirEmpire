using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamer : MonoBehaviour
{
    public GameObject exportFX;
    public GameObject point;
    public float timeDestroy = 1f;

    public void ShowFX()
    {
        SwarmEffect();
    }


    public void SwarmEffect()
    {
        
        GameObject go = Instantiate(exportFX,transform.parent);
        go.transform.localPosition = new Vector3(point.transform.localPosition.z + Random.Range(-0.2f, 0.2f), point.transform.localPosition.y + Random.Range(-0.2f, 0.2f), point.transform.localPosition.z + Random.Range(-0.1f, 0.1f));
        go.SetActive(true);
        Destroy(go, timeDestroy);
    }
}
