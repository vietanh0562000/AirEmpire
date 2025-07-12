using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupAds : MonoBehaviour
{
	public GameObject panelAdsSpeed;
	public GameObject panelAdsEarning;
	public GameObject panelAdsCompleteShipTesting;
	public GameObject panelAdsSpeedTest;
	public Text       textEarningTime;

	public GameObject panelAdsInvestor;
	public Text       textInvestorMoney;


	// Update is called once per frame
	void Update() { }

	public void GetIncomeForMap()
	{
		float income=0;
		//income += GetFormulaIncome(DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[3 * LoadingManager.Instance.currentMap]][3 * LoadingManager.Instance.currentMap], 
		//    DataManager.Instance.upgradeDatas.Upgrade_Worker[DataManager.Instance.list])
		income+=
			DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[3*LoadingManager.Instance.currentMap]][
				3*LoadingManager.Instance.currentMap];
		if(DataManager.Instance.listOperatorUnlock[1+3*LoadingManager.Instance.currentMap]==1)
		{

		}
		if(DataManager.Instance.listOperatorUnlock[2+3*LoadingManager.Instance.currentMap]==1)
		{

		}
	}

	public float GetFormulaIncome(float income,int worker,float speed) { return (income*worker*speed)/16; }

	public void OnClickBoosterSpeed()
	{
		InactiveAllPanel();
		panelAdsSpeed.SetActive(true);
	}

	public void OnClickBoosterEarning()
	{
		InactiveAllPanel();
		panelAdsEarning.SetActive(true);
		textEarningTime.text=AdsManager.Instance.numBoosterIncomeActive+" minutes";
	}

	public void OnClickAdsInvestor()
	{
		InactiveAllPanel();
		panelAdsInvestor.SetActive(true);
		textInvestorMoney.text="+"+UnitConverter.Convert(GetMaxIncome()*5);
		AdsManager.Instance.investorAdsObject.SetActive(false);
	}

	public void OnClickAdsCompleteShipTesting()
	{
		InactiveAllPanel();
		panelAdsCompleteShipTesting.SetActive(true);
	}

	public void OnClickSpeedTest()
	{
		InactiveAllPanel();
		panelAdsSpeedTest.SetActive(true);
	}

	public void InactiveAllPanel()
	{
		GameUIManager.Instance.popupAds.SetActive(true);
		panelAdsEarning.SetActive(false);
		panelAdsSpeed.SetActive(false);
		panelAdsInvestor.SetActive(false);
		panelAdsCompleteShipTesting.SetActive(false);
		panelAdsSpeedTest.SetActive(false);
	}

	public void OnClose() { GameUIManager.Instance.popupAds.SetActive(false); }

	public void WatchAdsx2Speed()
	{
		GameUIManager.Instance.popupAds.SetActive(false);
		x2SpeedBooster();
		SendEventFirebase.ClickOnAdsEvent("ads_reward_double_speed","Main Screen");
	}

	public void x2SpeedBooster() { AdsManager.Instance.ActivateBoosterSpeed(); }

	public void WatchAdsx2Income()
	{
		GameUIManager.Instance.popupAds.SetActive(false);
		x2IncomeBooster();
	}

	public void WatchAdsCompleteShipTest()
	{
		Debug.Log("watch ads complete ship test");
		GameUIManager.Instance.popupAds.SetActive(false);
		Debug.Log("active false");
		CompleteShipTest();
	}

	public void WatchAdsSpeedTest()
	{
		GameUIManager.Instance.popupAds.SetActive(false);
		x2SpeedTest();
	}

	void FailToLoadAds()
	{
		GameUIManager.Instance.popupNoads.SetActive(true);
		Invoke("AutoHidenPopup",3f);
	}

	public void AutoHidenPopup() { GameUIManager.Instance.popupNoads.SetActive(false); }

	void CompleteShipTest()
	{
		Debug.Log("call complete test");
		AdsManager.Instance.ActivateBoosterCompleteShipTest();
		Debug.Log("ActivateBoosterCompleteShipTest");
	}

	void x2SpeedTest()
	{
		if(GameDataManager.Instance.playerData.intDiamond>=100)
		{
			GameDataManager.Instance.playerData.SubDiamond(100);

			AdsManager.Instance.ActivateBoosterSpeedTest();
		}
	}

	void x2IncomeBooster()
	{
		if(GameDataManager.Instance.playerData.intDiamond>=100)
		{
			GameDataManager.Instance.playerData.SubDiamond(100);
			AdsManager.Instance.ActivateBoosterIncome();
		}
	}


	public void OnClickWatchInvestor()
	{
		if(GameDataManager.Instance.playerData.intDiamond>=100)
		{
			GameDataManager.Instance.playerData.SubDiamond(100);

			GameUIManager.Instance.popupAds.SetActive(false);
			WatchInvestorSucess();
		}
	}

	public void WatchInvestorSucess() { GameUIManager.Instance.AddMoney(GetMaxIncome()*5); }

	public float GetMaxIncome()
	{
		float max=0;

		if(DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[3*LoadingManager.Instance.currentMap]][
			   3*LoadingManager.Instance.currentMap]>max)
		{
			max=DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[3*LoadingManager.Instance.currentMap]][
				3*LoadingManager.Instance.currentMap];
		}
		if(DataManager.Instance.listOperatorUnlock[1+3*LoadingManager.Instance.currentMap]==1)
		{
			if(DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[1+3*LoadingManager.Instance.currentMap]][
				   1+3*LoadingManager.Instance.currentMap]>max)
			{
				max=DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[1+3*LoadingManager.Instance.currentMap]][
					1+3*LoadingManager.Instance.currentMap];
			}
		}
		if(DataManager.Instance.listOperatorUnlock[2+3*LoadingManager.Instance.currentMap]==1)
		{
			if(DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[2+3*LoadingManager.Instance.currentMap]][
				   2+3*LoadingManager.Instance.currentMap]>max)
			{
				max=DataManager.Instance.listUpgradeIncome[DataManager.Instance.listLevelEarning[2+3*LoadingManager.Instance.currentMap]][
					2+3*LoadingManager.Instance.currentMap];
			}
		}

		return max;
	}
}
