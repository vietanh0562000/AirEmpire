using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
    }

    public void OnDisappear()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear);
        StartCoroutine(CoWaitDisappear());
    }

    IEnumerator CoWaitDisappear()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
