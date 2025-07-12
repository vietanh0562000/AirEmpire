using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickYes()
    {
        Application.Quit();
    }

    public void Close()
    {
        GameUIManager.Instance.popupExit.SetActive(false);
    }
}
