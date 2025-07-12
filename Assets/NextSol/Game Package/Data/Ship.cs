
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship
{
    public int idship;
    //public GameObject ShipModelComplete;
    public GameObject ShipModel;
    public int numFinishSanGoLow, numFinishSanGoHigh, numFinishVoTauLow, numFinishVoTauHigh, numFinishSanTau;
    public TypeUnlockShip typeUnlock = TypeUnlockShip.Unlock;
}
