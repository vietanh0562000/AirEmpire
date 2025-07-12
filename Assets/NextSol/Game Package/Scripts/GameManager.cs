using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	public static GameManager  Instance;
	public        DataManager  dataManager;
	public        ShipData     shipData;
	public        GameObject   panelWorker;
	public        GameObject   prefabWorker;
	public        GameObject[] listShipWorking;
	//public GameObject currentShipWorking;
	//public GameObject currentShipWorking2;
	//public GameObject currentShipWorking3;
	public List<Transform> listPosInstantiateShipWork;
	public List<Transform> listPosInstantiateDecorShipWork;

	// pos path for worker
	//public GameObject posNear, posFar;
	//public List<GameObject> listPillarObject;
	//public List<GameObject> listWorkingPillarPos;
	//public List<GameObject> listWorkingPillarPos2;
	//public List<GameObject> listReturnPillarPos;
	//public List<GameObject> listReturnPillarPos2;

	//// pos high
	//public GameObject highPathObject;
	//public List<GameObject> listHighPartObject;
	//public List<GameObject> listHighWorkingPos;
	//public List<GameObject> listHighReturnPos;
	//public List<GameObject> listHighWorkingPos2;
	//public List<GameObject> listHighReturnPos2;

	public bool isbosterspeedup;
	//public bool isx2goldbosterup;
	//public bool isx3goldbosterup;
	//public float timex2goldspeedup;
	//public float timex3goldspeedup;
	//public float timebosterspeedup;
	//public float timeshownextx3;
	//public float timeshownextspeedup;
	//public bool isfirstshowx2gold;
	//public bool isfirstshowx2speed;
	//public bool isclickbosterspeedup;
	//public bool isclickbosterx3gold;

	public                   List<List<GameObject>> listListWorker;
	public                   List<GameObject>       listWorker,listWorker1,listWorker2,listWorker3;
	public                   int                    numberTotalWorker;
	[HideInInspector] public int                    numPartTransported=0,numPartCompleted=0;
	private                  int                    numberWorkerSpawn =0;
	private                  float                  timeSpawn;
	private                  float                  timespeedup;
	private                  bool                   isclickspeedup;

	public float numSpeedBooster=0,numSpeedClick=0;
	public float koeffmoney;

	public                   List<List<GameObject>> listListDecorTransportObject;
	public                   List<List<GameObject>> listListDecorObject;
	public                   List<GameObject>       listDecorTransportObject;
	public                   List<GameObject>       listDecorObject;
	[HideInInspector] public int                    indexDecor=0;
	public                   int[]                  listNumPartDecorCompleted;

	[HideInInspector] public GameObject effectSmokeFinish,effConfettiFinish;

	public List<GameObject> listShipObject;

	public int    offlineTime;
	public double moneyOffline;

	public List<Tween>       listTweener;
	public List<List<Tween>> listListTweener;

	public List<int>        listNumPartTransported,listNumPartCompleted;
	public List<GameObject> listEffectCoinBlast;

	public bool isMainScene=true;

	private float timeClick=0,timeAds=0;
	public  bool  isShowAds=false;

	public List<GameObject> listDecorEffects;

	public  int  currentMap;
	private void Awake() { Instance=this; }

	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate =60;
		Input.multiTouchEnabled     =false;
		listShipWorking             =new GameObject[3];
		listListDecorObject         =new List<List<GameObject>>();
		listListDecorTransportObject=new List<List<GameObject>>();
		listListWorker              =new List<List<GameObject>>();

		for(int i=0;i<3;i++)
		{
			listListDecorObject.Add(new List<GameObject>());
			listListDecorTransportObject.Add(new List<GameObject>());
			listListWorker.Add(new List<GameObject>());
		}

		listNumPartDecorCompleted=new int[3];

		listTweener    =new List<Tween>();
		listListTweener=new List<List<Tween>>();
		for(int i=0;i<3;i++)
		{
			listListTweener.Add(new List<Tween>());
		}

		dataManager.LoadData();
		LoadData();
		//numberTotalWorker = 1 + DataManager.Instance.levelWorker;       

		DataManager.Instance.isScene1=false;
	}

	// Update is called once per frame
	void Update()
	{
		//timeSpawn += Time.deltaTime;

		if(Input.GetMouseButton(0))
		{
			timeClick+=Time.deltaTime;

			if(!IsMouseOverUI())
			{
				if(PopupUpgrade.Instance.isOpen)
				{
					PopupUpgrade.Instance.OnClose();
				}

				if(PopupSaleUpgrade.Instance.isOpen)
				{
					PopupSaleUpgrade.Instance.OnClose();
				}
			}

			//if (!IsMouseOverUI() && !isbosterspeedup)
			//{
			//    isclickspeedup = true;
			//    timespeedup = 5;
			//}
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(timeClick<0.2f)
			{
				if(!IsMouseOverUI() && !isbosterspeedup)
				{
					isclickspeedup=true;
					timespeedup   =5;
				}
			}
			timeClick=0;

			InitTapFX();
		}

		if(isclickspeedup)
		{
			timespeedup-=Time.deltaTime;
			if(timespeedup>0)
			{
				foreach(GameObject workerA in listWorker)
				{
					workerA.GetComponent<WorkerController>().trail.SetActive(true);
				}

				foreach(GameObject effect in listDecorEffects)
				{
					effect.SetActive(true);
				}
				numSpeedClick=0.5f;
				UpgradeSpeedForZone(0);
				UpgradeSpeedForZone(1);
				UpgradeSpeedForZone(2);
				//Time.timeScale = 1.5f;
			}
			else
			{
				isclickspeedup=false;
				foreach(GameObject workerA in listWorker)
				{
					workerA.GetComponent<WorkerController>().trail.SetActive(false);
				}
				foreach(GameObject effect in listDecorEffects)
				{
					effect.SetActive(false);
				}
				numSpeedClick=0;
				UpgradeSpeedForZone(0);
				UpgradeSpeedForZone(1);
				UpgradeSpeedForZone(2);
				//Time.timeScale = 1;
			}
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if(focus==true && !isShowAds)
		{
			offlineTime=UnitConverter.Offline(PlayerPrefs.GetString("exittime"));
			//Debug.Log("offTime " + offlineTime);
			if(offlineTime>(3600*6))
			{
				offlineTime=3600*6;
			}

			if(DataManager.Instance.numTutorialStep>1)
			{
				if(offlineTime>=20)
				{
					GameUIManager.Instance.popupEarning.SetActive(true);
					//moneyOffline = offlineTime * GameUIManager.Instance.moneyPerPart / 1000;
					moneyOffline                             =offlineTime;
					GameUIManager.Instance.offlineIncome.text=UnitConverter.Convert(moneyOffline);
				}
			}
		}

		if(focus==true)
		{
#if UNITY_ANDROID
			isShowAds=false;
#endif
		}
	}

	public void InitTapFX()
	{
		GameObject go=PoolObjectManager.Instance.GetNextEffectPooling("tap_fx");
		go.SetActive(true);
		go.transform.position=Input.mousePosition;
		StartCoroutine(CoWaitFX(go));
	}

	IEnumerator CoWaitFX(GameObject go)
	{
		yield return new WaitForSeconds(1);
		go.SetActive(false);
	}

	public void SetUpShip(int index)
	{
		//listShipWorking[index] = Instantiate(shipData.ship[index].ShipModel, listPosInstantiateShipWork[index].position, Quaternion.identity);
		if(isMainScene)
		{
			listShipWorking[index]=Instantiate(shipData.ship[index].ShipModel,listPosInstantiateShipWork[index].position,Quaternion.identity);
		}
		else
		{
			listShipWorking[index]=Instantiate(shipData.ship[index].ShipModel,listPosInstantiateShipWork[index].position,Quaternion.Euler(0,90,0));
			listShipWorking[index].transform.localScale=new Vector3(8,8,8);
		}
		listShipWorking[index].GetComponent<ShipController>().numPartTransported=
			DataManager.Instance.listNumPartCompleted[index+3*LoadingManager.Instance.currentMap];
		listShipWorking[index].GetComponent<ShipController>().numPartCompleted=
			DataManager.Instance.listNumPartCompleted[index+3*LoadingManager.Instance.currentMap];

		for(int i=0;i<listShipWorking[index].GetComponent<ShipController>().numPartTransported;i++)
		{
			listShipWorking[index].GetComponent<ShipController>().listPillarObject[i].SetActive(true);
		}

		if(isMainScene)
		{
			for(int i=0;i<listShipWorking[index].transform.GetChild(2).transform.childCount;i++)
			{
				listListDecorObject[index].Add(listShipWorking[index].transform.GetChild(2).transform.GetChild(i).gameObject);
				GameObject decor=Instantiate(
				listListDecorObject[index][i],
				listPosInstantiateDecorShipWork[index].position,
				Quaternion.identity,
				listPosInstantiateDecorShipWork[index]);
				listListDecorTransportObject[index].Add(decor);
				listListDecorTransportObject[index][i].SetActive(false);
				listListDecorTransportObject[index][i].transform.localScale=new Vector3(4,4,4);
				listListDecorTransportObject[index][i].AddComponent<DecorController>();
				listListDecorTransportObject[index][i].GetComponent<DecorController>().workZone=index;
				listListDecorTransportObject[index][i].AddComponent<BoxCollider>();
			}

			for(int i=0;i<listNumPartDecorCompleted[index];i++)
			{
				listListDecorObject[index][i].SetActive(true);
			}
		}
	}

	public void SetUpNewShip(int index)
	{
		DataManager.Instance.listZoneFinish[index+3*LoadingManager.Instance.currentMap]=0;
		listShipWorking[index].GetComponent<ShipController>().numPartTransported       =0;
		listShipWorking[index].GetComponent<ShipController>().numPartCompleted         =0;
		listNumPartDecorCompleted[index]                                               =0;

		listShipWorking[index].GetComponent<ShipController>().transform.GetChild(0).gameObject.SetActive(true);

		for(int i=0;i<listShipWorking[index].GetComponent<ShipController>().listPillarObject.Count;i++)
		{
			listShipWorking[index].GetComponent<ShipController>().listPillarObject[i].SetActive(false);
		}
		for(int i=0;i<listListDecorObject[index].Count;i++)
		{
			listListDecorObject[index][i].SetActive(false);
		}

		listShipWorking[index].GetComponent<ShipController>().highPathObject.SetActive(false);

		SetUpWorkerForZone(index);
	}

	public void MoveShip(int index) { StartCoroutine(CoWaitMoveShip(index)); }

	IEnumerator CoWaitMoveShip(int index)
	{
		listShipWorking[index].GetComponent<ShipController>().numPartTransported=0;
		listShipWorking[index].GetComponent<ShipController>().numPartCompleted  =0;
		listNumPartDecorCompleted[index]                                        =0;

		listShipWorking[index].GetComponent<ShipController>().transform.GetChild(0).gameObject.SetActive(false);

		for(int i=0;i<listShipWorking[index].GetComponent<ShipController>().listPillarObject.Count;i++)
		{
			listShipWorking[index].GetComponent<ShipController>().listPillarObject[i].SetActive(false);
		}
		for(int i=0;i<listListDecorObject[index].Count;i++)
		{
			listListDecorObject[index][i].SetActive(false);
		}
		listShipWorking[index].GetComponent<ShipController>().highPathObject.SetActive(false);

		SaleManager.Instance.InitShipCompleted(index);

		yield return new WaitForSeconds(10);
		SetUpNewShip(index);
	}

	public void LoadData()
	{
		for(int i=0;i<listNumPartDecorCompleted.Length;i++)
		{
			listNumPartDecorCompleted[i]=DataManager.Instance.listNumPartDecorTransported[i+3*LoadingManager.Instance.currentMap];
		}

		//isfirstshowx2gold = true;
		//isfirstshowx2speed = true;
		offlineTime=UnitConverter.Offline(PlayerPrefs.GetString("exittime"));

		if(offlineTime>(3600*6))
		{
			offlineTime=3600*6;
		}

		if(DataManager.Instance.numTutorialStep>1)
		{
			if(offlineTime>=60)
			{
				GameUIManager.Instance.popupEarning.SetActive(true);
				//moneyOffline = offlineTime * GameUIManager.Instance.moneyPerPart / 1000;
				moneyOffline                             =offlineTime;
				GameUIManager.Instance.offlineIncome.text=UnitConverter.Convert(moneyOffline);
			}
		}

		if(DataManager.Instance.numTutorialStep!=0)
		{
			for(int i=0;i<3;i++)
			{
				if(DataManager.Instance.listOperatorUnlock[i+LoadingManager.Instance.currentMap*3]==1)
				{
					SetUpShip(i);
					SetUpWorkerForZone(i);
				}
			}
		}

		//SetUpShip(0);

		//SetUpWorkerForZone(0);
		//SetUpWorkerForZone(1);
		//SetUpWorkerForZone(2);

		if(isMainScene)
		{
			for(int i=0;i<3;i++)
			{
				if(listNumPartDecorCompleted[i]==0)
				{
					TruckController.Instance.listFirstDecor[i].SetActive(true);
				}
				else
				{
					TruckController.Instance.listFirstDecor[i].SetActive(false);
				}
			}
		}
	}

	IEnumerator CoWaitTest()
	{
		yield return new WaitForSeconds(1);
		TruckController.Instance.TransportDecor();
	}

	IEnumerator WaitShowBooster(GameObject go,float time)
	{
		yield return new WaitForSeconds(time);
		go.SetActive(true);
		go.transform.DOLocalMove(go.transform.localPosition+new Vector3(300,0,0),0.5f).SetEase(Ease.Linear);
		//isfirstshowx2speed = false;        
	}

	public void SetUpWorker(int workZone)
	{
		GameObject goWorker=Instantiate(prefabWorker,panelWorker.transform);
		goWorker.GetComponent<WorkerController>().workZone=workZone;
		//listWorker.Add(goWorker);

		listListWorker[workZone].Add(goWorker);
		listWorker.Add(goWorker);
	}

	public void SetUpNewWorker(int workZone)
	{
		GameObject goWorker=Instantiate(prefabWorker,panelWorker.transform);
		goWorker.GetComponent<WorkerController>().workZone =workZone;
		goWorker.GetComponent<WorkerController>().isLanding=true;

		listListWorker[workZone].Add(goWorker);
		listWorker.Add(goWorker);
	}

	public void SetUpWorkerForZone(int workZone)
	{
		//StartCoroutine(CoWaitSetUpWorker(workZone, Mathf.Min(10, DataManager.Instance.listLevelWorker[workZone] + 1)));
		if(listShipWorking[workZone].GetComponent<ShipController>().numPartTransported==
		   listShipWorking[workZone].GetComponent<ShipController>().listPillarObject.Count)
		{
			if(listNumPartDecorCompleted[workZone]<listListDecorObject[workZone].Count)
			{
				TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().workZone=workZone;
				TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().truckObject.GetComponent<TransportCarController>()
					.TransportDecor(workZone);
			}
			else
			{
				TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().workZone=workZone;
				TruckController.Instance.listCraneObject[workZone].GetComponent<StruckController>().isFinish=true;
			}


			//GameManager.Instance.listWorker.Remove(gameObject);
			//Destroy(gameObject, 1);

			//GameManager.Instance.listListWorker[workZone].Remove(gameObject);
			//GameManager.Instance.listWorker.Remove(gameObject);
			//Destroy(gameObject, 1);
		}
		else
		{
			StartCoroutine(
			CoWaitSetUpWorker(
			workZone,
			Mathf.Min(
			10,
			(int) Mathf.Floor(DataManager.Instance.upgradeDatas.Upgrade_Worker[GameUIManager.Instance.listListUpgradeLevel[workZone][2]].worker))));
		}
	}

	IEnumerator CoWaitSetUpWorker(int workZone,int max)
	{
		for(int i=0;i<max;i++)
		{
			yield return new WaitForSeconds(1);
			SetUpWorker(workZone);
		}
	}

	public void CompletePart(int workZone,int index)
	{
		listShipWorking[workZone].GetComponent<ShipController>().listPillarObject[index].SetActive(true);
		listShipWorking[workZone].GetComponent<ShipController>().numPartCompleted++;
	}

	//public void UpgradeSpeed()
	//{
	//    for (int i = 0; i < listTweener.Count; i++)
	//    {
	//        listTweener[i].DOTimeScale(1f + (DataManager.Instance.levelSpeed - 1) * 0.03f, 0.1f).SetEase(Ease.Linear);
	//    }
	//}

	public void UpgradeSpeedForZone(int zone)
	{
		if(zone==3)
		{

		}
		else
		{
			float speed=DataManager.Instance.upgradeDatas.Upgrade_Speed[GameUIManager.Instance.listListUpgradeLevel[zone][1]-1].speed;
			for(int i=0;i<listListTweener[zone].Count;i++)
			{
				listListTweener[zone][i].DOTimeScale(
				speed+(speed*OperatorManager.Instance.listCaptains[zone].boostSpeed)+numSpeedBooster+numSpeedClick,
				0.1f).SetEase(Ease.Linear);
			}
			for(int i=0;i<listListWorker[zone].Count;i++)
			{
				listListWorker[zone][i].GetComponent<WorkerController>().anim.speed=
					speed+(speed*OperatorManager.Instance.listCaptains[zone].boostSpeed)+numSpeedBooster+numSpeedClick;
			}
		}
	}

	public bool IsMouseOverUI()
	{
		if(Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor)
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
		else
		{
			//return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
			return IsPointerOverUIObject();
		}
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition=new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position=new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		List<RaycastResult> results=new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
		return results.Count>0;
	}
}
