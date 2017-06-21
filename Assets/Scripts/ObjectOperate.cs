#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID
#endif


#if UNITY_IPHONE && !UNITY_EDITOR
#define IPHONE
#endif

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class ObjectOperate : MonoBehaviour
{
    public MainUI mainUI;
    public ThreeDOperate threeDOperate;
    bool canPlace = false;

    [HideInInspector]
    public bool canOperate = true;

    void Update()
    {
        if (!canOperate)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
            {//当前触摸在UI上
            }
            else//当前没有触摸在UI上
            {
                RaycastHit hit;
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.root.CompareTag("jiaju"))
                    {
                        if (targetObj)
                        {
                            mainUI.tipObject.SetActive(false);
                            Utils.SetObjectHighLight(targetObj.gameObject, false, Color.clear);
                        }
                        if (targetObj != hit.transform.root)
                        {
                            threeDOperate.index = 0;
                            if (threeDOperate.current3DObj != null)
                            {
                                threeDOperate.current3DObj.gameObject.SetActive(false);
                            }
                        }
                        targetObj = hit.transform.root;
                        goodInfo = targetObj.GetComponent<GoodInfo>().currentGood;
                        mainUI.InitObjectData(targetObj);
                        foreach (Transform tran in targetObj.GetComponentsInChildren<Transform>())
                        {
                            tran.gameObject.layer = LayerMask.NameToLayer("temp");
                        }
                        canDrag = true;
                    }
                }
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (canDrag)
            {
                if (canPlace)
                {
                    if (goodInfo.goodType == GoodType.spawnObj)
                    {
                        mainUI.setParamPanel.SetActive(true);
                        mainUI.operateObjPanel.SetActive(true);
                        mainUI.InitObjectData(targetObj);
                    }
                    else if (goodInfo.goodType == GoodType.changeImg)
                    {
                        Texture t = Resources.Load<Texture>(goodInfo.prefabName);
                        Utils.ChangeShaderAlbedo(raycastHit, t);
                        GoodInfo tt = raycastHit.GetComponent<GoodInfo>();
                        if (tt == null)
                        {
                            tt = raycastHit.AddComponent<GoodInfo>();
                        }
                        tt.currentGood = goodInfo;
                    }
                    if (targetObj != null)
                    {
                        foreach (Transform tran in targetObj.GetComponentsInChildren<Transform>())
                        {
                            tran.gameObject.layer = LayerMask.NameToLayer("Default");
                        }
                    }
                }
                else
                {
                    if (targetObj)
                    {
                        Destroy(targetObj.gameObject);
                    }
                    mainUI.setParamPanel.SetActive(false);
                    mainUI.operateObjPanel.SetActive(false);
                }
                if (raycastHit != null)
                {
                    Utils.SetObjectHighLight(raycastHit, false, Color.clear);
                }
                canDrag = false;
                textureBG = null;
                raycastHit = null;
                goodInfo = null;
                mainUI.tipObject.SetActive(false);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (canDrag)
            {
                RaycastHit hit;
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerValue = ~(1 << LayerMask.NameToLayer("temp"));
                if (Physics.Raycast(ray, out hit, 1000, layerValue))
                {
                    if (goodInfo.goodType == GoodType.spawnObj)
                    {
                        Vector3 point = hit.point;
                        Collider cc = targetObj.GetComponent<Collider>();
                        if (cc)
                        {
                            point.y = cc.bounds.size.y / 4f;
                        }
                        targetObj.position = point;
                    }
                    else if (goodInfo.goodType == GoodType.changeImg)
                    {

                    }

                    string tag = hit.transform.tag;
                    bool isHas = goodInfo.tags.Contains(tag);
                    canPlace = isHas;
                    if (isHas)
                    {
                        raycastHit = hit.transform.gameObject;
                        mainUI.tipObject.SetActive(false);
                        if (targetObj)
                        {
                            Utils.SetObjectHighLight(targetObj.gameObject, true, Color.clear);
                        }
                    }
                    else
                    {
                        mainUI.tipObject.SetActive(true);
                        if (targetObj)
                        {
                            Utils.SetObjectHighLight(targetObj.gameObject, true, Color.red);
                        }
                    }
                }
            }
        }
    }

    GameObject raycastHit;
    Texture textureBG = null;
    [HideInInspector]
    public Transform targetObj;
    [HideInInspector]
    public Goods goodInfo;
    [HideInInspector]
    public bool canDrag = false;

    private void OnGUI()
    {
        if (canDrag)
        {
            if (goodInfo != null && goodInfo.goodType == GoodType.changeImg)
            {
                if (textureBG == null)
                {
                    textureBG = Resources.Load<Texture>(goodInfo.spriteName);
                }
                Vector3 mousePosition = Input.mousePosition;
                GUI.DrawTexture(new Rect(mousePosition.x - 40, Screen.height - mousePosition.y - 40, 80, 80), textureBG);
            }
        }
    }
}
