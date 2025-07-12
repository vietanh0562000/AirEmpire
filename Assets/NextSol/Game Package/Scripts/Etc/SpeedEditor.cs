using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SpeedEditor : EditorWindow
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Change Speed/+1")]
    public static void SpeedUp()
    {
        Time.timeScale += 1;
    }

    [MenuItem("Change Speed/-1")]
    public static void SpeedDown()
    {
        Time.timeScale -= 1;
    }

    [MenuItem("Change Speed/Set Default Speed")]
    public static void SetSpeedDefault()
    {
        Time.timeScale = 1;
    }
}
#endif