using UnityEngine;
using System.Collections;

public enum Direction
{
    Tdefault = 0,
    LeftRigth = 1,
    TopBottom = 2,
    ForwardBack = 3
}

public class ObjectOperate : MonoBehaviour
{
    public MainUI mainUI;
    void InitArrow(Transform theTarget)
    {
        if (theTarget)
        {
            targetObj = theTarget;
            theDirection = Direction.Tdefault;
            Vector3 spawnVector = targetObj.position;
            spawnVector.x += 14;
            spawnVector.y += 10;
            spawnVector.z += 18;
            arrowObj.position = spawnVector;
            arrowObj.gameObject.SetActive(true);
        }
    }


    Transform targetObj;
    public Transform arrowObj;
    float preMouseX = 0;
    float preMouseY = 0;
    float preMouseZ = 0;

    Direction theDirection = Direction.Tdefault;

    public float speed = 0.3f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("leftright"))
                {
                    preMouseX = mousePosition.x;
                    theDirection = Direction.LeftRigth;
                }
                else if (hit.transform.CompareTag("topbottom"))
                {
                    preMouseY = mousePosition.y;
                    theDirection = Direction.TopBottom;
                }
                else if (hit.transform.CompareTag("forwardback"))
                {
                    preMouseZ = mousePosition.y;
                    theDirection = Direction.ForwardBack;
                }
                else if (hit.transform.CompareTag("jiaju"))
                {
                    InitArrow(hit.transform);
                    mainUI.InitObjectData(hit.transform);
                }
                else
                {
                    //arrowObj.gameObject.SetActive(false);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            theDirection = Direction.Tdefault;
        }
        if (Input.GetMouseButton(0))
        {
            if (theDirection == Direction.LeftRigth)
            {
                float nowMouseX = Input.mousePosition.x;
                float distance = nowMouseX - preMouseX;
                preMouseX = nowMouseX;
                Vector3 vv = targetObj.position;
                vv.x += distance * speed;
                targetObj.position = vv;

                vv = arrowObj.position;
                vv.x += distance * speed;
                arrowObj.position = vv;
            }
            else if (theDirection == Direction.TopBottom)
            {
                float nowMouseY = Input.mousePosition.y;
                float distance = nowMouseY - preMouseY;
                preMouseY = nowMouseY;
                Vector3 vv = targetObj.position;
                vv.y += distance * speed;
                targetObj.position = vv;

                vv = arrowObj.position;
                vv.y += distance * speed;
                arrowObj.position = vv;
            }
            else if (theDirection == Direction.ForwardBack)
            {
                float nowMouseZ = Input.mousePosition.y;
                float distance = nowMouseZ - preMouseZ;
                preMouseZ = nowMouseZ;
                Vector3 vv = targetObj.position;
                vv.z += distance * speed;
                targetObj.position = vv;

                vv = arrowObj.position;
                vv.z += distance * speed;
                arrowObj.position = vv;
            }
        }
    }
}
