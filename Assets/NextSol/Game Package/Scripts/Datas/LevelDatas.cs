using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Map", menuName = "Level Map", order = 1)]
public class LevelDatas : ScriptableObject
{
    public List<LevelObject> listLevelObject;
}
