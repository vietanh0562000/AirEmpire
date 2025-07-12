using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct Equipment
//{
//    List<Part> partList;
//}

[System.Serializable]
public struct Part
{
    public string partName;
    public List<GameObject> models;
    public float delay;
}

public class ShipProcess : MonoBehaviour
{
    public Part BaseParts;

    public Part BoneParts;

    public Part Floor1Parts;
    public Part Floor2Parts;
    public List<GameObject> UpLoor;
    public Part FloorEndParts;

    public Part SellParts;
    
    public Part CabinParts;

    public Part PhaoParts;

    private void Start()
    {
        StartCoroutine(ProcessOne());
    }

    IEnumerator ProcessOne()
    {
        yield return new WaitForSeconds(1f);

        for(int i = 0; i < BoneParts.models.Count; i++)
        {
            yield return new WaitForSeconds(BoneParts.delay);
            BoneParts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < Floor1Parts.models.Count; i++)
        {
            yield return new WaitForSeconds(Floor1Parts.delay);
            Floor1Parts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < Floor2Parts.models.Count; i++)
        {
            yield return new WaitForSeconds(Floor2Parts.delay);
            Floor2Parts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < UpLoor.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            UpLoor[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < FloorEndParts.models.Count; i++)
        {
            yield return new WaitForSeconds(FloorEndParts.delay);
            FloorEndParts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < SellParts.models.Count; i++)
        {
            yield return new WaitForSeconds(SellParts.delay);
            SellParts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < CabinParts.models.Count; i++)
        {
            yield return new WaitForSeconds(CabinParts.delay);
            CabinParts.models[i].SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < PhaoParts.models.Count; i++)
        {
            yield return new WaitForSeconds(PhaoParts.delay);
            PhaoParts.models[i].SetActive(true);
        }
    }

}
