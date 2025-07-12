using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorManager : MonoBehaviour
{
    public static OperatorManager Instance;
    public GameObject prefabCaptain;
    public GameObject prefabHireCaptain;
    public List<GameObject> listParentCaptain;
    public List<GameObject> listCaptainPos;
    public List<CaptainController> listCaptains;
    public List<OperatorController> listOperators;
    public List<ZoneController> listZone;
    public List<string> listBoostNames;
    public List<int> listRandomCaptainID;
    public List<int> listCaptainIDHired;

    public GameObject effSmokeUnlock;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitCaptain();

        for (int i = 1; i < 3; i++)
        {
            if (GameUIManager.Instance.moneyTotal >= DataManager.Instance.upgradeDatas.Unlock_Data[i + 3 * LoadingManager.Instance.currentMap].cost)
            {
                listZone[i].isReadyUnlock = true;
                listZone[i].lockObject.GetComponent<Animator>().Play("lock_ready");
            }
        }
    }

    public void InitCaptain()
    {
        for (int i = 0; i < listCaptainPos.Count; i++)
        {
            GameObject goCaptain = Instantiate(prefabCaptain, listParentCaptain[i].transform);
            goCaptain.transform.position = listCaptainPos[i].transform.position;
            goCaptain.GetComponent<CaptainController>().workZone = i;
            listCaptains.Add(goCaptain.GetComponent<CaptainController>());
        }

        for (int i = 0; i < 4; i++)
        {
            if (DataManager.Instance.listHireCaptain[i + 3 * LoadingManager.Instance.currentMap] == 1)
            {
                listCaptainIDHired.Add(DataManager.Instance.listCaptainID[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
