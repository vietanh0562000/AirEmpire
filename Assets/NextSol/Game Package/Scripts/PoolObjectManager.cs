using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObjectItem
{
    public string tagOfObject;
    public List<GameObject> mainObjectPreb;
    public Transform parent;
    public int count;
    public bool isExtend;
}

[System.Serializable]
public class PoolEffectItem
{
    public string tagOfEffect;
    public GameObject mainEffectPreb;
    public Transform parent;

    public int count;
    public bool isExtend;
}
public class PoolObjectManager : MonoBehaviour
{

    #region Instance
    private static PoolObjectManager instance;
    public static PoolObjectManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    #endregion

    #region Varible

    public List<PoolObjectItem> listPoolItem;
    public List<PoolEffectItem> listEffectItem;
    public List<GameObject> listPoolObject;
    public List<GameObject> listPoolEffect;
    #endregion

    #region Funcition G
    public void StartPooling()
    {
        List<GameObject> tmplistObject = new List<GameObject>();
        foreach (PoolObjectItem item in listPoolItem)
        {
            for (int i = 0; i < item.count; i++)
            {
                for (int j = 0; j < item.mainObjectPreb.Count; j++)
                {
                    GameObject GObj = Instantiate(item.mainObjectPreb[j], item.parent);
                    GObj.SetActive(false);
                    tmplistObject.Add(GObj);
                }
            }
        }
        SetRandomListPoolObject(tmplistObject);

        //Effect
        foreach (PoolEffectItem item in listEffectItem)
        {
            for (int i = 0; i < item.count; i++)
            {
                GameObject EObj = Instantiate(item.mainEffectPreb, item.parent);
                EObj.SetActive(false);
                listPoolEffect.Add(EObj);
            }
        }
    }

    void SetRandomListPoolObject(List<GameObject> listOBJ)
    {
        while (listOBJ.Count > 0)
        {
            int index = Random.Range(0, listOBJ.Count);
            listPoolObject.Add(listOBJ[index]);
            listOBJ.RemoveAt(index);
        }
    }
    public GameObject GetNextEffectPooling(string Tag)
    {
        for (int i = 0; i < listPoolEffect.Count; i++)
        {
            if (!listPoolEffect[i].activeInHierarchy && listPoolEffect[i].CompareTag(Tag))
            {
                return listPoolEffect[i];
            }
        }

        foreach (PoolEffectItem item in listEffectItem)
        {
            if (item.tagOfEffect == Tag)
            {
                if (item.isExtend)
                {
                    GameObject EObj = Instantiate(item.mainEffectPreb, item.parent);
                    EObj.SetActive(false);
                    listPoolObject.Add(EObj);
                    return EObj;
                }
            }
        }
        return null;
    }
    public GameObject GetNextObjectPooling(string Tag)
    {
        for (int i = 0; i < listPoolObject.Count; i++)
        {
            if (!listPoolObject[i].activeInHierarchy && listPoolObject[i].CompareTag(Tag))
            {
                return listPoolObject[i];
            }
        }

        foreach (PoolObjectItem item in listPoolItem)
        {
            if (item.tagOfObject == Tag)
            {
                if (item.isExtend)
                {
                    GameObject GObj = Instantiate(item.mainObjectPreb[Random.Range(0, item.mainObjectPreb.Count)], item.parent);
                    GObj.SetActive(false);
                    listPoolObject.Add(GObj);
                    return GObj;
                }
            }
        }
        return null;
    }

    #endregion
}
