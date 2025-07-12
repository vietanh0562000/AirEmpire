using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance;
    public GameObject camera_GameObject;

    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;

    public float MAX_ZOOM, MIN_ZOOM;
    public float MIN_X, MAX_X, MIN_Y, MAX_Y;
    public float sensitivity = 0.1f;
    public float MaxDistance, DistanceScale, ScreenSwipeScale;
    private float mouseLastPosX, mouseLastPosY;
    private bool isPress = false;

    private int touchCount;

    private void Start()
    {
        Instance = this;
        DistanceScale = MaxDistance / sensitivity;
        ScreenSwipeScale = (Screen.width / 3) / DistanceScale;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            touchCount = 0;
        }
        else
        {
            touchCount = 1;
        }
        SetCamStartScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0 && isZooming)
        {
            isZooming = false;
        }

        if(DataManager.Instance.numTutorialStep != 3)
        {
            if(Input.touchCount > (touchCount - 1))
            {
                if (!IsMouseOverUI())
                {
                    if (Input.touchCount == touchCount)
                    {
                        if (!isZooming)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                mouseLastPosX = Input.mousePosition.x;
                                mouseLastPosY = Input.mousePosition.y;
                                isPress = true;
                            }

                            if (Input.GetMouseButton(0))
                            {
                                //if (PopupUpgrade.Instance.isOpen)
                                //{
                                //    PopupUpgrade.Instance.OnClose();
                                //}
                                if (isPress == true)
                                {
                                    float tempPosX = Input.mousePosition.x;
                                    float tempPosY = Input.mousePosition.y;

                                    //transform.localPosition += new Vector3(-sensitivity * ((tempPosX - mouseLastPosX) / ScreenSwipeScale), -sensitivity * ((tempPosY - mouseLastPosY) / ScreenSwipeScale), 0);
                                    transform.localPosition += new Vector3(-sensitivity * ((tempPosX - mouseLastPosX) / ScreenSwipeScale), -sensitivity * ((tempPosY - mouseLastPosY) / ScreenSwipeScale), 0);

                                    if (transform.localPosition.x >= MAX_X)
                                    {
                                        transform.localPosition = new Vector3(MAX_X, transform.localPosition.y, transform.localPosition.z);
                                    }
                                    if (transform.localPosition.x <= MIN_X)
                                    {
                                        transform.localPosition = new Vector3(MIN_X, transform.localPosition.y, transform.localPosition.z);
                                    }
                                    if (transform.localPosition.y >= MAX_Y)
                                    {
                                        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, MAX_Y + 10);
                                        transform.localPosition = new Vector3(transform.localPosition.x, MAX_Y, transform.localPosition.z);
                                    }
                                    if (transform.localPosition.y <= MIN_Y)
                                    {
                                        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -MAX_Y);
                                        transform.localPosition = new Vector3(transform.localPosition.x, MIN_Y, transform.localPosition.z);
                                    }
                                    mouseLastPosX = tempPosX;
                                    mouseLastPosY = tempPosY;
                                }
                            }

                            if (Input.GetMouseButtonUp(0))
                            {
                                isPress = false;
                            }
                        }
                    }
                    else if (Input.touchCount == 2)
                    {
                        if (Input.GetTouch(1).phase == TouchPhase.Moved)
                        {
                            isZooming = true;

                            DragNewPosition = GetWorldPositionOfFinger(1);
                            Vector2 PositionDifference = DragNewPosition - DragStartPosition;

                            if (Vector2.Distance(DragNewPosition, Finger0Position) < DistanceBetweenFingers)
                                camera_GameObject.GetComponent<Camera>().orthographicSize += (PositionDifference.magnitude);

                            if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers)
                                camera_GameObject.GetComponent<Camera>().orthographicSize -= (PositionDifference.magnitude);

                            if (camera_GameObject.GetComponent<Camera>().orthographicSize < MIN_ZOOM)
                            {
                                camera_GameObject.GetComponent<Camera>().orthographicSize = MIN_ZOOM;
                            }

                            if (camera_GameObject.GetComponent<Camera>().orthographicSize > MAX_ZOOM)
                            {
                                camera_GameObject.GetComponent<Camera>().orthographicSize = MAX_ZOOM;
                            }

                            DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
                        }
                        DragStartPosition = GetWorldPositionOfFinger(1);
                        Finger0Position = GetWorldPositionOfFinger(0);
                    }
                }
            }
        }
            
    }

    Vector2 GetWorldPosition()
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetWorldPositionOfFinger(int FingerIndex)
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(FingerIndex).position);
    }

    private bool IsMouseOverUI()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        else
        {
            return IsPointerOverUIObject();
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public void SetCamStartScene()
    {
        camera_GameObject.GetComponent<Camera>().orthographicSize = MAX_ZOOM + 10;
        camera_GameObject.GetComponent<Camera>().DOOrthoSize(MAX_ZOOM, 1f).SetEase(Ease.Linear);
    }
}
