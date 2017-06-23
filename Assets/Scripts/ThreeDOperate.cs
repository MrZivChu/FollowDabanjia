using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
    none = 0,
    LeftRigth = 1,
    TopBottom = 2,
    ForwardBack = 3
}

public class ThreeDOperate : MonoBehaviour
{
    public ObjectOperate objectOperate;

    public Camera theCamera;
    public MainUI mainUI;

    public Transform scaleRight;
    public Transform scaleTop;
    public Transform scaleForward;

    Direction theDirection = Direction.none;
    public float speed = 0.01f;

    float preMouseX = 0;
    float preMouseY = 0;
    float preMouseZ = 0;


    float initScaleRight;
    float initScaleTop;
    float initScaleForward;
    private void Start()
    {
        initScaleRight = scaleRight.localScale.z;
        initScaleTop = scaleTop.localScale.z;
        initScaleForward = scaleForward.localScale.z;
    }
    [HideInInspector]
    public int index = 0;
    public List<Transform> PSR = new List<Transform>();

    public void SetArrowPosition()
    {
        if (objectOperate.targetObj)
        {
            Vector3 spawnVector = objectOperate.targetObj.position;
            //spawnVector.x += 0.08f;
            //spawnVector.y += 0.5f;
            //spawnVector.z += 0.2f;
            if (current3DObj != null)
            {
                current3DObj.position = spawnVector;
            }
        }
    }

    Transform pre3DObj;
    [HideInInspector]
    public Transform current3DObj;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (objectOperate.targetObj)
            {
                if (pre3DObj != null)
                {
                    pre3DObj.gameObject.SetActive(false);
                }
                if (index < PSR.Count)
                {
                    current3DObj = PSR[index];
                }
                else
                {
                    current3DObj = null;
                }
                pre3DObj = current3DObj;
                if (current3DObj != null)
                {
                    current3DObj.gameObject.SetActive(true);
                    SetArrowPosition();
                }
                index++;
                if (index <= PSR.Count)
                {
                    objectOperate.canOperate = false;
                }
                else if (index == PSR.Count + 1)
                {
                    objectOperate.canOperate = true;
                    index = 0;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);
            int layerValue = 1 << LayerMask.NameToLayer("arrow");
            if (Physics.Raycast(ray, out hit, 1000, layerValue))
            {
                if (hit.transform.CompareTag("leftright"))
                {
                    theDirection = Direction.LeftRigth;
                }
                else if (hit.transform.CompareTag("topbottom"))
                {
                    theDirection = Direction.TopBottom;
                }
                else if (hit.transform.CompareTag("forwardback"))
                {
                    theDirection = Direction.ForwardBack;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            theDirection = Direction.none;
            preMouseX = 0;
            preMouseY = 0;
            preMouseZ = 0;
            if (index == 2)
            {
                Vector3 v1 = scaleRight.localScale;
                v1.z = initScaleRight;
                scaleRight.localScale = v1;

                Vector3 v2 = scaleTop.localScale;
                v2.z = initScaleTop;
                scaleTop.localScale = v2;

                Vector3 v3 = scaleForward.localScale;
                v3.z = initScaleForward;
                scaleForward.localScale = v3;
            }
        }
        if (Input.GetMouseButton(0))
        {
            float distance = 0;
            float distanceX = 0;
            float distanceY = 0;
            float distanceZ = 0;
            float nowMouseX = Input.mousePosition.x;
            float nowMouseY = Input.mousePosition.y;
            float nowMouseZ = Input.mousePosition.z;
            if (preMouseX != 0)
            {
                distanceX = nowMouseX - preMouseX;
                distanceY = nowMouseY - preMouseY;
                distanceZ = nowMouseZ - preMouseZ;
            }
            distance = distanceX + distanceY + distanceZ;

            preMouseX = nowMouseX;
            preMouseY = nowMouseY;
            preMouseZ = nowMouseZ;

            if (theDirection == Direction.LeftRigth)
            {
                if (index == 1)
                {
                    Vector3 vv = objectOperate.targetObj.position;
                    vv.x += distance * speed;
                    objectOperate.targetObj.position = vv;

                    vv = current3DObj.position;
                    vv.x += distance * speed;
                    current3DObj.position = vv;
                }
                else if (index == 2)
                {
                    Vector3 vv = objectOperate.targetObj.localScale;
                    vv.x += distance * speed;
                    objectOperate.targetObj.localScale = vv;

                    vv = scaleRight.localScale;
                    vv.z += distance * speed;
                    scaleRight.localScale = vv;
                }
                else if (index == 3)
                {
                    objectOperate.targetObj.Rotate(Vector3.up, -distance * speed * 10);
                }
                mainUI.InitObjectData();
            }
            else if (theDirection == Direction.TopBottom)
            {
                if (index == 1)
                {
                    Vector3 vv = objectOperate.targetObj.position;
                    vv.y += distance * speed;
                    objectOperate.targetObj.position = vv;

                    vv = current3DObj.position;
                    vv.y += distance * speed;
                    current3DObj.position = vv;
                }
                else if (index == 2)
                {
                    Vector3 vv = objectOperate.targetObj.localScale;
                    vv.y += distance * speed;
                    objectOperate.targetObj.localScale = vv;

                    vv = scaleTop.localScale;
                    vv.z += distance * speed;
                    scaleTop.localScale = vv;
                }
                else if (index == 3)
                {
                    objectOperate.targetObj.Rotate(Vector3.forward, -distance * speed * 10);
                }
                mainUI.InitObjectData();
            }
            else if (theDirection == Direction.ForwardBack)
            {
                if (index == 1)
                {
                    Vector3 vv = objectOperate.targetObj.position;
                    vv.z += distance * speed;
                    objectOperate.targetObj.position = vv;

                    vv = current3DObj.position;
                    vv.z += distance * speed;
                    current3DObj.position = vv;
                }
                else if (index == 2)
                {
                    Vector3 vv = objectOperate.targetObj.localScale;
                    vv.z += distance * speed;
                    objectOperate.targetObj.localScale = vv;

                    vv = scaleForward.localScale;
                    vv.z += distance * speed;
                    scaleForward.localScale = vv;
                }
                else if (index == 3)
                {
                    objectOperate.targetObj.Rotate(Vector3.right, distance * speed * 10);
                }
                mainUI.InitObjectData();
            }
        }
    }
}
