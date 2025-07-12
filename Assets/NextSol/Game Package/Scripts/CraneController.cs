using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraneController : MonoBehaviour
{
    public static CraneController Instance;
    public Animator craneAnimator;

    public GameObject effectSmoke, effectKhoiden;
    private float deltaTime, timeEffect = 6;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        craneAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if(deltaTime > timeEffect)
        {
            effectKhoiden.GetComponent<ParticleSystem>().Play();
            timeEffect = Random.Range(6f, 10f);
            deltaTime = 0;
        }
    }

    public void FindPath()
    {
        transform.DOMove(new Vector3(GameManager.Instance.listDecorObject[GameManager.Instance.indexDecor].transform.position.x, transform.position.y, transform.position.z), 1).SetEase(Ease.Linear);
    }

    public void ApplyPart()
    {
        effectSmoke.transform.position = GameManager.Instance.listDecorObject[GameManager.Instance.indexDecor].transform.position;
        GameManager.Instance.listDecorObject[GameManager.Instance.indexDecor].SetActive(true);
        GameManager.Instance.listDecorTransportObject[GameManager.Instance.indexDecor].SetActive(false);
        effectSmoke.GetComponent<ParticleSystem>().Play();
        GameManager.Instance.indexDecor++;
        TruckController.Instance.isfinishloop = true;
        if(GameManager.Instance.indexDecor < GameManager.Instance.listDecorTransportObject.Count)
        {
            TruckController.Instance.TransportDecor();
        }
    }

    public void ReturnPath()
    {
        transform.DOMove(new Vector3(-3.1f, transform.position.y, transform.position.z), 1).SetEase(Ease.Linear);
    }

    public void StopAnim()
    {
        if(GameManager.Instance.indexDecor >= GameManager.Instance.listDecorTransportObject.Count)
        {
            craneAnimator.enabled = false;
            GameManager.Instance.effectSmokeFinish.SetActive(true);
            GameManager.Instance.effConfettiFinish.SetActive(true);
            //GameManager.Instance.highPathObject.SetActive(false);
            if (PlayerPrefsController.shipSelect < 1)
            {
                GameUIManager.Instance.bttUnlockNewShip.SetActive(true);
                GameUIManager.Instance.canvasUI.SetActive(false);
            }
        }
    }
}
