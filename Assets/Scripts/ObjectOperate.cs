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
using UnityStandardAssets.CrossPlatformInput;
using uTools;

public class ObjectOperate : MonoBehaviour
{
    public MainUI mainUIScript;
    public ThreeDOperate threeDOperateScript;

    //当打开3D辅助工具的时候，对象是不好被操作的，也就是不好被拖动的，因为两者是冲突的
    [HideInInspector]
    public bool canOperate = true;

    Vector3 downPosition = Vector3.zero;

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
                downPosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(downPosition);
                if (Physics.Raycast(ray, out hit))
                {
                    print(hit.transform.name);
                    GoodInfo gif = hit.transform.GetComponent<GoodInfo>();
                    if (gif != null)
                    {
                        Transform rootObj = gif.rootObj.transform;
                        //if (rootObj != null && rootObj.CompareTag("jiaju"))
                        if (rootObj != null)
                        {
                            gif = rootObj.GetComponent<GoodInfo>();
                            //关闭上一个选中对象的选中效果
                            if (targetObj)
                            {
                                mainUIScript.tipObject.SetActive(false);
                                Utils.SetObjectHighLight(targetObj.gameObject, false, Color.clear);
                            }
                            //点击的是不同的对象则隐藏3D操作辅助工具
                            if (targetObj != rootObj)
                            {
                                threeDOperateScript.index = 0;
                                if (threeDOperateScript.current3DObj != null)
                                {
                                    threeDOperateScript.current3DObj.gameObject.SetActive(false);
                                }
                            }

                            targetObj = rootObj;
                            mainUIScript.operateObj = targetObj;
                            InitParam(rootObj, gif.currentGood);
                            //表示此物体可以被拖放
                            if (gif.currentGood.tags != null && gif.currentGood.tags.Count > 0)
                            {
                                mainUIScript.InitObjectData();
                                foreach (Transform tran in targetObj.GetComponentsInChildren<Transform>())
                                {
                                    tran.gameObject.layer = LayerMask.NameToLayer("temp");
                                }
                                isDragging = true;
                            }
                            //else
                            {
                                Utils.SetObjectHighLight(targetObj.gameObject, true, Color.clear);
                            }
                        }
                    }
                }
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                if (Input.mousePosition != downPosition)
                {
                    if (canPlace)
                    {
                        if (goods.goodType == GoodType.spawnObj)
                        {
                            mainUIScript.operateObj = targetObj;
                            mainUIScript.setParamPanel.SetActive(true);
                            mainUIScript.InitObjectData();
                        }
                        else if (goods.goodType == GoodType.changeImg)
                        {
                            Texture t1 = Resources.Load<Texture>(goods.albedo);
                            Utils.ChangeShaderAlbedo(raycastHit, t1);
                            Texture t2 = Resources.Load<Texture>(goods.normalMap);
                            Utils.ChangeShaderNormalMap(raycastHit, t2);
                            Texture t3 = Resources.Load<Texture>(goods.occlusion);
                            Utils.ChangeShaderOcclusion(raycastHit, t3);
                            GoodInfo tt = raycastHit.GetComponent<GoodInfo>();
                            if (tt == null)
                            {
                                //tt = raycastHit.AddComponent<GoodInfo>();
                                //tt.currentGood = goods;
                            }
                        }
                    }
                    else
                    {
                        if (goods.goodType == GoodType.spawnObj)
                        {
                            if (targetObj)
                            {
                                Destroy(targetObj.gameObject);
                            }
                            mainUIScript.setParamPanel.SetActive(false);
                        }
                    }
                }
                RecoveryRaycast();
                isDragging = false;
                canPlace = false;

                textureBG = null;
                raycastHit = null;
                mainUIScript.tipObject.SetActive(false);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            if (isDragging && mousePosition != downPosition)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerValue = ~(1 << LayerMask.NameToLayer("temp"));
                if (Physics.Raycast(ray, out hit, 1000, layerValue))
                {
                    //判断是否能放置在此物体上面
                    string tag = hit.transform.tag;
                    print(hit.transform.name + " = " + hit.transform.tag);
                    canPlace = goods.tags.Contains(tag);
                    if (canPlace)
                    {
                        mainUIScript.tipObject.SetActive(false);
                        if (goods.goodType == GoodType.spawnObj)
                        {
                            Utils.SetObjectHighLight(targetObj.gameObject, true, Color.clear);
                            SetPosition(hit.point, tag);
                        }
                        else
                        {
                            raycastHit = hit.transform.gameObject;
                        }
                    }
                    else
                    {
                        mainUIScript.tipObject.SetActive(true);
                        if (goods.goodType == GoodType.spawnObj)
                        {
                            Utils.SetObjectHighLight(targetObj.gameObject, true, Color.red);
                            SetPosition(hit.point, tag);
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (targetObj)
            {
                if (targetObj.tag.Contains("TV"))
                {
                    PlayMovie pm = targetObj.GetComponentInChildren<PlayMovie>();
                    if (pm)
                    {
                        if (pm.playStatus == PlayStatus.pause)
                        {
                            pm.Play();
                        }
                        else if (pm.playStatus == PlayStatus.playing)
                        {
                            pm.Pause();
                        }
                    }
                }
                else if (targetObj.tag.Contains("Light"))
                {
                    PlayLight pl = targetObj.GetComponentInChildren<PlayLight>();
                    if (pl)
                    {
                        if (!pl.isLigt)
                        {
                            pl.TurnOn();
                        }
                        else
                        {
                            pl.TurnOff();
                        }
                    }
                }
                else if (targetObj.tag.Contains("Fanner"))
                {
                    uTweenRotation tempuTweenRotation = targetObj.GetComponentInChildren<uTweenRotation>();
                    if (tempuTweenRotation)
                    {
                        tempuTweenRotation.enabled = !tempuTweenRotation.enabled;
                    }
                }
            }
        }
    }

    //用于设置墙纸
    GameObject raycastHit;

    [HideInInspector]
    public bool isDragging = false;
    //是否是内置物体
    [HideInInspector]
    public bool isInGood = false;
    bool canPlace = false;

    [HideInInspector]
    public Transform targetObj;
    Goods goods = null;
    GoodType goodType = GoodType.spawnObj;
    float centerY = 0;
    float topY = 0;
    public void InitParam(Transform toperateObj, Goods tgoods)
    {
        targetObj = toperateObj;
        goods = tgoods;
        goodType = tgoods.goodType;

        isInGood = !(goods.tags != null && goods.tags.Count > 0);
        if (toperateObj)
        {
            GoodInfo ttgoodInfo = toperateObj.GetComponent<GoodInfo>();
            if (ttgoodInfo != null)
            {
                topY = ttgoodInfo.topY;
                centerY = ttgoodInfo.centerY;
            }
        }
    }


    public void CloseHighLight()
    {
        if (targetObj)
        {
            Utils.SetObjectHighLight(targetObj.gameObject, false, Color.clear);
        }
    }

    public void IgnoreRaycast()
    {
        if (targetObj != null)
        {
            foreach (Transform tran in targetObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = LayerMask.NameToLayer("temp");
            }
        }
    }

    public void RecoveryRaycast()
    {
        if (targetObj != null)
        {
            foreach (Transform tran in targetObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    public void SetPosition(Vector3 point, string hitTag)
    {
        if (hitTag == "topwall" && goods.tags.Contains(hitTag))//表示是吊灯
        {
            point.y = point.y - (topY - centerY);
        }
        else if (hitTag == "ceiwall" && goods.tags.Contains(hitTag))
        {
            print(point);
        }
        else
        {
            point.y = centerY + point.y;
        }
        targetObj.position = point;
    }

    Texture textureBG = null;
    private void OnGUI()
    {
        if (isDragging)
        {
            if (goodType == GoodType.changeImg)
            {
                if (textureBG == null)
                {
                    textureBG = Resources.Load<Texture>(goods.spriteName);
                }
                Vector3 mousePosition = Input.mousePosition;
                GUI.DrawTexture(new Rect(mousePosition.x - 40, Screen.height - mousePosition.y - 40, 80, 80), textureBG);
            }
        }
    }
}
