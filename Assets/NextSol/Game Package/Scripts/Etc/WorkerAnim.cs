using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnim : MonoBehaviour
{
    public Animator animWorker;
    public Animator animHammer;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SetRandomAnim(Random.Range(5, 8), Random.Range(3, 6)));
        float rand = Random.Range(0.4f, 0.8f);
        animWorker.speed = rand;
        animHammer.speed = rand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetRandomAnim(float time1, float time2)
    {
        animWorker.enabled = true;
        animHammer.gameObject.SetActive(true);
        animHammer.enabled = true;
        yield return new WaitForSeconds(time1);
        animWorker.enabled = false;
        animHammer.enabled = false;
        animHammer.gameObject.SetActive(false);
        yield return new WaitForSeconds(time2);
        StartCoroutine(SetRandomAnim(Random.Range(5, 8), Random.Range(3, 6)));
    }
}
