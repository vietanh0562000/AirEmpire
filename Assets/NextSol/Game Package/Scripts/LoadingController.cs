using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public ShipData shipdata;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefsController.firtApp() == true)
        {
            for (int i = 0; i < shipdata.ship.Length; i++)
            {
                shipdata.ship[i].idship = i;
                PlayerPrefsController.setTypeShip(i, shipdata.ship[i].typeUnlock);
                PlayerPrefsController.shipSelect = 0;
            }
            PlayerPrefsController.setFirtApp(1);
        }
    }
    
}
