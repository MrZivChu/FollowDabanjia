using UnityEngine;
using System.Collections;

public class TouchRawImage : MonoBehaviour
{
    float preDistance = 0;
    float currentDistance = 0;
    public GameObject rawImage;

    public Camera theCamera;

    bool isDraging = false;
    [HideInInspector]
    public DragRotateZWH dr;
    void Start()
    {
        EventTriggerListener.Get(rawImage.gameObject).onBeginDrag = (go, param) =>
        {
            dr.enabled = true;
        };
        EventTriggerListener.Get(rawImage.gameObject).onDrag = (go, param) =>
        {
            isDraging = true;
        };
        EventTriggerListener.Get(rawImage.gameObject).onEndDrag = (go, param) =>
        {
            isDraging = false;
            dr.enabled = false;
        };
    }

    float speed = 0.1f;
    void Update()
    {
        if (Input.touchCount > 1 && isDraging)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector3 tempPosition1 = Input.GetTouch(0).position;
                Vector3 tempPosition2 = Input.GetTouch(1).position;
                currentDistance = Vector2.Distance(tempPosition1, tempPosition2);
                float vv = currentDistance * speed;
                if (currentDistance > preDistance)
                {
                    float currentFiledOfView = Camera.main.fieldOfView - vv < 30 ? 30 : Camera.main.fieldOfView - vv;
                    theCamera.fieldOfView = currentFiledOfView;
                }
                else
                {
                    float currentFiledOfView = Camera.main.fieldOfView + vv > 150 ? 150 : Camera.main.fieldOfView + vv;
                    theCamera.fieldOfView = currentFiledOfView;
                }
                preDistance = currentDistance;
            }
        }
    }
}
