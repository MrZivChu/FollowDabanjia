using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System;

enum ClickTransfromType
{
    initial = 0,
    reducePX = 1,
    addPX = 2,
    reducePY = 3,
    addPY = 4,
    reducePZ = 5,
    addPZ = 6,
    reduceRX = 7,
    addRX = 8,
    reduceRY = 9,
    addRY = 10,
    reduceRZ = 11,
    addRZ = 12,
    reduceSX = 13,
    addSX = 14,
    reduceSY = 15,
    addSY = 16,
    reduceSZ = 17,
    addSZ = 18
}

public class MainUI : MonoBehaviour
{
    public GameObject tipObject;
    public ThreeDOperate threeDOperate;

    public GraphicRaycaster personCanvasGraphicRaycaster;

    public ObjectOperate objectOperate;

    public Button showBtn;
    public GameObject operationBtns;
    public GameObject SelectRoomPanel;
    public GameObject AddGoodsPanel;
    public Button changeRoomBtn;
    public Button addGoodsBtn;

    public GameObject operateObjPanel;
    public GameObject setParamPanel;

    public GameObject showGoodDetailPanel;

    public Button moveBtn;
    public Button rotateBtn;
    public Button scaleBtn;
    public Button deleteBtn;
    public Button showGoodDetailBtn;


    public Button changReduce;
    public Button changAdd;
    public InputField changInputField;

    public Button kuanReduce;
    public Button kuanAdd;
    public InputField kuanInputField;

    public Button gaoReduce;
    public Button gaoAdd;
    public InputField gaoInputField;

    public Button xReduce;
    public Button xAdd;
    public InputField xInputField;

    public Button yReduce;
    public Button yAdd;
    public InputField yInputField;


    public Button zReduce;
    public Button zAdd;
    public InputField zInputField;

    public Button xRotateReduce;
    public Button xRotateAdd;
    public InputField xRotateInputField;

    public Button yRotateReduce;
    public Button yRotateAdd;
    public InputField yRotateInputField;


    public Button zRotateReduce;
    public Button zRotateAdd;
    public InputField zRotateInputField;

    ClickTransfromType clickTransfromType = ClickTransfromType.initial;

    Dictionary<string, List<Goods>> goodsDic = new Dictionary<string, List<Goods>>();


    private void Start()
    {
        EventTriggerListener.Get(showBtn.gameObject).onClick = ShowBtns;
        EventTriggerListener.Get(changeRoomBtn.gameObject).onClick = onChangeRoomPanel;
        EventTriggerListener.Get(addGoodsBtn.gameObject).onClick = onAddGoodsPanel;

        EventTriggerListener.Get(deleteBtn.gameObject).onClick = DeleteObject;
        EventTriggerListener.Get(showGoodDetailBtn.gameObject).onClick = ShowGoodDetailObject;
        EventTriggerListener.Get(moveBtn.gameObject).onClick = MoveObject;
        EventTriggerListener.Get(rotateBtn.gameObject).onClick = RotateObject;
        EventTriggerListener.Get(scaleBtn.gameObject).onClick = ScaleObject;

        EventTriggerListener.Get(changReduce.gameObject).onClick = ReduceChange;
        EventTriggerListener.Get(changAdd.gameObject).onClick = AddChange;
        EventTriggerListener.Get(kuanReduce.gameObject).onClick = ReduceKuan;
        EventTriggerListener.Get(kuanAdd.gameObject).onClick = AddKuan;
        EventTriggerListener.Get(gaoReduce.gameObject).onClick = ReduceGao;
        EventTriggerListener.Get(gaoAdd.gameObject).onClick = AddGao;

        EventTriggerListener.Get(xReduce.gameObject).onClick = ReduceX;
        EventTriggerListener.Get(xAdd.gameObject).onClick = AddX;
        EventTriggerListener.Get(yReduce.gameObject).onClick = ReduceY;
        EventTriggerListener.Get(yAdd.gameObject).onClick = AddY;
        EventTriggerListener.Get(zReduce.gameObject).onClick = ReduceZ;
        EventTriggerListener.Get(zAdd.gameObject).onClick = AddZ;

        EventTriggerListener.Get(xRotateReduce.gameObject).onClick = ReduceRotateX;
        EventTriggerListener.Get(xRotateAdd.gameObject).onClick = AddRotateX;
        EventTriggerListener.Get(yRotateReduce.gameObject).onClick = ReduceRotateY;
        EventTriggerListener.Get(yRotateAdd.gameObject).onClick = AddRotateY;
        EventTriggerListener.Get(zRotateReduce.gameObject).onClick = ReduceRotateZ;
        EventTriggerListener.Get(zRotateAdd.gameObject).onClick = AddRotateZ;

        EventTriggerListener.Get(changReduce.gameObject).onDown = (go, param) => { isPress = false; clickTransfromType = ClickTransfromType.reduceSX; };
        EventTriggerListener.Get(changReduce.gameObject).onUp = (go, param) => { clickTransfromType = ClickTransfromType.initial; tempTime = 0; };
        EventTriggerListener.Get(changAdd.gameObject).onDown = (go, param) => { isPress = false; clickTransfromType = ClickTransfromType.addSX; };
        EventTriggerListener.Get(changAdd.gameObject).onUp = (go, param) => { clickTransfromType = ClickTransfromType.initial; tempTime = 0; };

        EventTriggerListener.Get(yRotateReduce.gameObject).onDown = (go, param) => { isPress = false; clickTransfromType = ClickTransfromType.reduceRY; };
        EventTriggerListener.Get(yRotateReduce.gameObject).onUp = (go, param) => { clickTransfromType = ClickTransfromType.initial; tempTime = 0; };
        EventTriggerListener.Get(yRotateAdd.gameObject).onDown = (go, param) => { isPress = false; clickTransfromType = ClickTransfromType.addRY; };
        EventTriggerListener.Get(yRotateAdd.gameObject).onUp = (go, param) => { clickTransfromType = ClickTransfromType.initial; tempTime = 0; };


        changInputField.onEndEdit.AddListener(ChangOnEndEdit);
        kuanInputField.onEndEdit.AddListener(KuanOnEndEdit);
        gaoInputField.onEndEdit.AddListener(GaoOnEndEdit);

        xInputField.onEndEdit.AddListener(XOnEndEdit);
        yInputField.onEndEdit.AddListener(YOnEndEdit);
        zInputField.onEndEdit.AddListener(ZOnEndEdit);

        xRotateInputField.onEndEdit.AddListener(XRotateOnEndEdit);
        yRotateInputField.onEndEdit.AddListener(YRotateOnEndEdit);
        zRotateInputField.onEndEdit.AddListener(ZRotateOnEndEdit);

        TextAsset textAsset = Resources.Load<TextAsset>("goods");
        string content = textAsset.text;
        JsonData res = JsonMapper.ToObject(content);
        if (res != null && res.Count > 0)
        {
            for (int i = 0; i < res.Count; i++)
            {
                JsonData parentJD = res[i];
                if (parentJD != null && parentJD.Count > 0)
                {
                    //存放对应的家具预设的文件夹名称
                    string DirecName = parentJD["directName"].ToString();
                    parentJD = parentJD["goods"];
                    for (int j = 0; j < parentJD.Count; j++)
                    {
                        JsonData childJD = parentJD[j];
                        if (childJD != null)
                        {
                            Goods good = new Goods();
                            good.id = childJD["id"].ToString();
                            good.name = childJD["name"].ToString();
                            good.home = childJD["home"].ToString();
                            good.prefabName = childJD["prefabName"].ToString();
                            good.spriteName = childJD["spriteName"].ToString();
                            good.chang = childJD["chang"].ToString();
                            good.kuan = childJD["kuan"].ToString();
                            good.gao = childJD["gao"].ToString();
                            good.goodType = (GoodType)(Convert.ToInt32(childJD["goodType"].ToString()));
                            JsonData jd = childJD["tags"];
                            if (jd != null && jd.Count > 0)
                            {
                                good.tags = new List<string>();
                                for (int m = 0; m < jd.Count; m++)
                                {
                                    good.tags.Add(jd[m].ToString());
                                }
                            }

                            if (goodsDic.ContainsKey(DirecName))
                            {
                                goodsDic[DirecName].Add(good);
                            }
                            else
                            {
                                goodsDic[DirecName] = new List<Goods>() { good };
                            }
                        }
                    }
                }
            }
        }

        List<string> furnitureNames = new List<string>() { "Bed", "Curtains", "KitchenSet", "Lamps", "Paintings", "Pillows", "SofaChair", "TablesTV" };
        for (int m = 0; m < TopButtonList.Count; m++)
        {
            List<Goods> tt = new List<Goods>();
            if (m >= 0 && m < furnitureNames.Count)
            {
                if (goodsDic.ContainsKey(furnitureNames[m]))
                {
                    tt = goodsDic[furnitureNames[m]];
                }
            }
            EventTriggerListener.Get(TopButtonList[m].gameObject, tt).onClick = ShowGoods;
            if (m == 0)
            {
                TopButtonList[0].GetComponent<Toggle>().isOn = true;
                ShowGoods(TopButtonList[m].gameObject, tt);
            }
        }
    }

    float addSpanValue = 0.5f;
    float tempTime = 0;
    float maxTempTime = 0.5f;
    bool isPress = false;
    private void Update()
    {
        if (clickTransfromType != ClickTransfromType.initial)
        {
            if (Input.GetMouseButton(0))
            {
                tempTime += Time.deltaTime;
                if (tempTime >= maxTempTime)
                {
                    isPress = true;
                    if (clickTransfromType == ClickTransfromType.reduceSX)
                    {
                        if (operateObj)
                        {
                            Vector3 scale = operateObj.localScale;
                            float tt = addSpanValue * Time.deltaTime;
                            float t = scale.x - tt;
                            if (t < 1)
                            {
                                t = 1;
                            }
                            SetScaleX(t);
                        }
                    }
                    else if (clickTransfromType == ClickTransfromType.addSX)
                    {
                        if (operateObj)
                        {
                            Vector3 scale = operateObj.localScale;
                            float tt = addSpanValue * Time.deltaTime;
                            SetScaleX(scale.x + tt);
                        }
                    }
                    else if (clickTransfromType == ClickTransfromType.reduceRY)
                    {
                        if (operateObj)
                        {
                            float tt = addSpanValue * Time.deltaTime * 30;
                            rotateY -= tt;
                            operateObj.Rotate(operateObj.transform.up, -tt);
                            yRotateInputField.text = rotateY.ToString("0.00");
                            threeDOperate.SetArrowPosition();
                        }
                    }
                    else if (clickTransfromType == ClickTransfromType.addRY)
                    {
                        if (operateObj)
                        {
                            float tt = addSpanValue * Time.deltaTime * 30;
                            rotateY += tt;
                            operateObj.Rotate(operateObj.transform.up, tt);
                            yRotateInputField.text = rotateY.ToString("0.00");
                            threeDOperate.SetArrowPosition();
                        }
                    }
                }
            }
        }
    }


    void SetScaleX(float value)
    {
        Vector3 scale = operateObj.localScale;
        scale.x = value;
        operateObj.localScale = scale;
        changInputField.text = operateObj.localScale.x.ToString("0.00");
        threeDOperate.SetArrowPosition();
    }

    public void InitObjectData(Transform tTarget)
    {
        operateObj = tTarget;
        if (operateObj)
        {
            setParamPanel.SetActive(true);
            operateObjPanel.SetActive(true);

            changInputField.text = operateObj.localScale.x.ToString("0.00");
            kuanInputField.text = operateObj.localScale.z.ToString("0.00");
            gaoInputField.text = operateObj.localScale.y.ToString("0.00");

            xInputField.text = operateObj.position.x.ToString("0.00");
            yInputField.text = operateObj.position.y.ToString("0.00");
            zInputField.text = operateObj.position.z.ToString("0.00");

            rotateX = 0;
            rotateY = 0;
            rotateZ = 0;
            xRotateInputField.text = "0";
            yRotateInputField.text = "0";
            zRotateInputField.text = "0";
        }
    }

    void ChangOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                if (result < 1)
                {
                    result = 1;
                }
                SetScaleX(result);
            }
        }
    }

    void KuanOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                if (result < 1)
                {
                    result = 1;
                }
                Vector3 scale = operateObj.localScale;
                scale.z = result;
                operateObj.localScale = scale;
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void GaoOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                if (result < 1)
                {
                    result = 1;
                }
                Vector3 scale = operateObj.localScale;
                scale.y = result;
                operateObj.localScale = scale;
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void XOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.x = result;
                operateObj.localPosition = position;
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void YOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.y = result;
                operateObj.localPosition = position;
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void ZOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.z = result;
                operateObj.localPosition = position;
                threeDOperate.SetArrowPosition();
            }
        }
    }

    float rotateX;
    float rotateY;
    float rotateZ;
    void XRotateOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                result = rotateX - result;
                rotateX -= result;
                operateObj.Rotate(Vector3.right, result);
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void YRotateOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                result = rotateY - result;
                rotateY -= result;
                operateObj.Rotate(Vector3.up, result);
                threeDOperate.SetArrowPosition();
            }
        }
    }


    void ZRotateOnEndEdit(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                print(rotateZ + " = " + result);
                result = rotateZ - result;
                rotateZ -= result;
                operateObj.Rotate(Vector3.forward, result);
                threeDOperate.SetArrowPosition();
            }
        }
    }


    Transform operateObj;
    public float distance = 1f;
    void DeleteObject(GameObject go, object param)
    {
        if (operateObj)
        {
            Destroy(operateObj.gameObject);
            operateObjPanel.SetActive(false);
            objectOperate.canOperate = true;
            setParamPanel.SetActive(false);
            if (threeDOperate.current3DObj)
            {
                threeDOperate.current3DObj.gameObject.SetActive(false);
            }
            threeDOperate.index = 0;
        }
    }

    void ShowGoodDetailObject(GameObject go, object param)
    {
        if (operateObj)
        {
            GoodInfo goodInfo = operateObj.GetComponent<GoodInfo>();
            if (goodInfo)
            {
                ShowGoodDetail tShowGoodDetail = showGoodDetailPanel.GetComponent<ShowGoodDetail>();
                Goods tGoods = goodInfo.currentGood;
                tShowGoodDetail.SetValue(tGoods.name, tGoods.home, tGoods.chang, tGoods.kuan, tGoods.gao);
                showGoodDetailPanel.SetActive(true);
            }
        }
    }

    void MoveObject(GameObject go, object param)
    {

    }

    void RotateObject(GameObject go, object param)
    {

    }

    void ScaleObject(GameObject go, object param)
    {

    }

    void ShowBtns(GameObject go, object param)
    {
        operationBtns.SetActive(!operationBtns.activeSelf);
    }

    void onChangeRoomPanel(GameObject go, object param)
    {
        //AddGoodsPanel.SetActive(false);
        //SelectRoomPanel.SetActive(!SelectRoomPanel.activeSelf);
        //personCanvasGraphicRaycaster.enabled = !SelectRoomPanel.activeSelf;
    }

    void onAddGoodsPanel(GameObject go, object param)
    {
        SelectRoomPanel.SetActive(false);
        AddGoodsPanel.SetActive(!AddGoodsPanel.activeSelf);
        //personCanvasGraphicRaycaster.enabled = !AddGoodsPanel.activeSelf;
        //if (AddGoodsPanel.activeSelf)
        //{
        //    setParamPanel.SetActive(false);
        //    operateObjPanel.SetActive(false);
        //    if (objectOperate.targetObj)
        //    {
        //        Utils.SetObjectHighLight(objectOperate.targetObj.gameObject, false);
        //    }
        //}
    }

    void ReduceChange(GameObject go, object param)
    {
        if (operateObj && !isPress)
        {
            Vector3 scale = operateObj.localScale;
            float tt = scale.x - distance;
            if (tt < 1)
            {
                tt = 1;
            }
            SetScaleX(tt);
        }
    }

    void AddChange(GameObject go, object param)
    {
        if (operateObj && !isPress)
        {
            Vector3 scale = operateObj.localScale;
            float tt = scale.x + distance;
            SetScaleX(tt);
        }
    }

    void ReduceKuan(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            float tt = scale.z - distance;
            if (tt < 1)
            {
                tt = 1;
            }
            scale.z = tt;
            operateObj.localScale = scale;
            kuanInputField.text = operateObj.localScale.z.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddKuan(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.z += distance;
            operateObj.localScale = scale;
            kuanInputField.text = operateObj.localScale.z.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceGao(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            float tt = scale.y - distance;
            if (tt < 1)
            {
                tt = 1;
            }
            scale.y = tt;
            operateObj.localScale = scale;
            gaoInputField.text = operateObj.localScale.y.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddGao(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.y += distance;
            operateObj.localScale = scale;
            gaoInputField.text = operateObj.localScale.y.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceX(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.x -= distance;
            operateObj.position = position;
            xInputField.text = operateObj.position.x.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddX(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.x += distance;
            operateObj.position = position;
            xInputField.text = operateObj.position.x.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceY(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.y -= distance;
            operateObj.position = position;
            yInputField.text = operateObj.position.y.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddY(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.y += distance;
            operateObj.position = position;
            yInputField.text = operateObj.position.y.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceZ(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.z -= distance;
            operateObj.position = position;
            zInputField.text = operateObj.position.z.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddZ(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.z += distance;
            operateObj.position = position;
            zInputField.text = operateObj.position.z.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceRotateX(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateX -= distance;
            operateObj.Rotate(Vector3.right, -distance);
            xRotateInputField.text = rotateX.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddRotateX(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateX += distance;
            operateObj.Rotate(Vector3.right, distance);
            xRotateInputField.text = rotateX.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceRotateY(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateY -= distance;
            operateObj.Rotate(Vector3.up, -distance);
            yRotateInputField.text = rotateY.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddRotateY(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateY += distance;
            operateObj.Rotate(Vector3.up, distance);
            yRotateInputField.text = rotateY.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void ReduceRotateZ(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateZ -= distance;
            operateObj.Rotate(Vector3.forward, -distance);
            zRotateInputField.text = rotateZ.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    void AddRotateZ(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateZ += distance;
            operateObj.Rotate(Vector3.forward, distance);
            zRotateInputField.text = rotateZ.ToString("0.00");
            threeDOperate.SetArrowPosition();
        }
    }

    public Transform storeParent;
    public List<GameObject> TopButtonList = new List<GameObject>();


    void ShowGoods(GameObject go, object data)
    {
        List<Goods> list = (List<Goods>)data;
        Utils.SpawnCellForTable(storeParent, list, SpawnOrUpdate);
    }

    void SpawnOrUpdate(GameObject go, Goods data, bool isSpawn, int index)
    {
        GameObject cell = go;
        if (isSpawn)
        {
            UnityEngine.Object obj = Resources.Load("SelectGoodCell");
            if (obj)
            {
                cell = Instantiate(obj) as GameObject;
                cell.transform.parent = go.transform;
            }
        }

        Vector3 orignalPosition = Vector3.zero;
        EventTriggerListener.Get(cell, data).onBeginDrag = (tgo, tdata) =>
        {
            orignalPosition = Input.mousePosition;
        };

        EventTriggerListener.Get(cell, data).onDrag = (tgo, tdata) =>
        {
            Vector3 currentPosition = Input.mousePosition;
            if (currentPosition.y - orignalPosition.y > 10)
            {
                Goods tgood = (Goods)tdata;

                AddGoodsPanel.SetActive(false);
                tipObject.SetActive(false);
                if (objectOperate.targetObj)
                {
                    Utils.SetObjectHighLight(objectOperate.targetObj.gameObject, false, Color.clear);
                }
                setParamPanel.SetActive(false);
                operateObjPanel.SetActive(false);

                if (tgood.goodType == GoodType.spawnObj)
                {
                    UnityEngine.Object obj = Resources.Load<GameObject>(tgood.prefabName);
                    if (obj)
                    {
                        GameObject dragObject = Instantiate(obj) as GameObject;
                        //dragObject.transform.localPosition = Vector3.zero;
                        GoodInfo goodInfo = dragObject.GetComponent<GoodInfo>();
                        if (goodInfo == null)
                        {
                            goodInfo = dragObject.AddComponent<GoodInfo>();
                        }
                        goodInfo.currentGood = tgood;
                        objectOperate.targetObj = dragObject.transform;
                        objectOperate.canOperate = true;
                        threeDOperate.index = 0;
                        if (threeDOperate.current3DObj != null)
                        {
                            threeDOperate.current3DObj.gameObject.SetActive(false);
                        }
                        foreach (Transform tran in objectOperate.targetObj.GetComponentsInChildren<Transform>())
                        {
                            tran.gameObject.layer = LayerMask.NameToLayer("temp");
                        }
                    }
                }
                else if (tgood.goodType == GoodType.changeImg)
                {
                    objectOperate.targetObj = null;
                }
                objectOperate.goodInfo = tgood;
                objectOperate.canDrag = true;
            }
        };
        cell.GetComponent<Image>().sprite = Resources.Load<Sprite>(data.spriteName);
        cell.SetActive(true);
    }
}
