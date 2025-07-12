using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasingManager : MonoBehaviour
{
	public void OnPressDown(int i)
	{
		switch (i)
		{
			case 1:
				GameUIManager.Instance.AddMoney(1000);
				IAPManager.Instance.BuyProductID(IAPKey.PACK1);
				break;
			case 2:
				GameUIManager.Instance.AddMoney(3000);
				IAPManager.Instance.BuyProductID(IAPKey.PACK2);
				break;
			case 3:
				GameUIManager.Instance.AddMoney(5000);
				IAPManager.Instance.BuyProductID(IAPKey.PACK3);
				break;
			case 4:
				GameUIManager.Instance.AddMoney(10000);
				IAPManager.Instance.BuyProductID(IAPKey.PACK4);
				break;

			case 5:
				GameDataManager.Instance.playerData.AddDiamond(10);
				IAPManager.Instance.BuyProductID(IAPKey.PACK5);
				break;
			case 6:
				GameDataManager.Instance.playerData.AddDiamond(30);
				IAPManager.Instance.BuyProductID(IAPKey.PACK6);
				break;
			case 7:
				GameDataManager.Instance.playerData.AddDiamond(50);
				IAPManager.Instance.BuyProductID(IAPKey.PACK7);
				break;
			case 8:
				GameDataManager.Instance.playerData.AddDiamond(100);
				IAPManager.Instance.BuyProductID(IAPKey.PACK8);
				break;
		}
	}

	public void Sub(int i) { GameDataManager.Instance.playerData.SubDiamond(i); }
}
