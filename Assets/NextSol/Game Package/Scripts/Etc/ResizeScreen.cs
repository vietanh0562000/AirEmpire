using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResizeScreen : MonoBehaviour
{
    public Camera mainCamera;
    public float camSizeLongest;
    public float camSizeLonger;

    // Start is called before the first frame update
    void Start()
    {
        //Resize();
    }

    public void Resize()
    {
        if (1f / mainCamera.aspect >= 2.3f)
        {
            mainCamera.orthographicSize = 37 - 7;
            AnimZoomCamera(mainCamera.orthographicSize);
        }
        else if (1f / mainCamera.aspect >= 2f)
        {
            mainCamera.orthographicSize = 35 - 7;
            AnimZoomCamera(mainCamera.orthographicSize);
        }
        else if (1f / mainCamera.aspect >= 1.7f)
        {
            mainCamera.orthographicSize = 30 - 7;
            AnimZoomCamera(mainCamera.orthographicSize);
        }
        else if (1f / mainCamera.aspect >= 1.5f)
        {
            mainCamera.orthographicSize = 28 - 7;
            AnimZoomCamera(mainCamera.orthographicSize);
        }
        else
        {
            mainCamera.orthographicSize = 28 - 7;
            AnimZoomCamera(mainCamera.orthographicSize);
        }
    }

    public void AnimZoomCamera(float value)
    {
        mainCamera.DOOrthoSize(value + 7, 3).SetEase(Ease.Linear);
    }
}
