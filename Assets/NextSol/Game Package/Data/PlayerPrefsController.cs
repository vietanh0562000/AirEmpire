using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController 
{
    public static void setTypeShip(int idship, TypeUnlockShip typeunlockship)
    {
        PlayerPrefs.SetString("typeship" + idship, typeunlockship.ToString());
    }
    public static TypeUnlockShip getTypeUnlockShip(int idItem)
    {
        //if (PlayerPrefs.GetString("typeship" + idItem) == TypeUnlockShip.Lock.ToString())
        //{
        //    return TypeUnlockShip.Lock;
        //}
        return PlayerPrefs.GetString("typeship" + idItem) == TypeUnlockShip.Lock.ToString() ? TypeUnlockShip.Lock : TypeUnlockShip.Unlock;
        //return TypeUnlockShip.Unlock;
    }
    public static int shipSelect
    {
        set { PlayerPrefs.SetInt("shipSelect", value); }
        get { return PlayerPrefs.GetInt("shipSelect"); }
    }
    public static bool firtApp()
    {
        if (PlayerPrefs.GetInt("firtApp") > 0) return false;
        return true;
    }
    public static void setFirtApp(int vl)
    {
        PlayerPrefs.SetInt("firtApp", vl);
    }
}
