using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;

public class Goods
{
    public string id;
    public string spriteName;
    public string name;
    public string home;
    public string prefabName;
    public string chang;
    public string kuan;
    public string gao;
}

public class MainUI : MonoBehaviour
{

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
                            good.prefabName = DirecName + "/" + childJD["prefabName"].ToString();
                            good.spriteName = DirecName + "/" + childJD["spriteName"].ToString();
                            good.chang = childJD["chang"].ToString();
                            good.kuan = childJD["kuan"].ToString();
                            good.gao = childJD["gao"].ToString();
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
                if (result >= 1)
                {
                    Vector3 scale = operateObj.localScale;
                    scale.x = result;
                    operateObj.localScale = scale;
                    objectOperate.SetArrowPosition();
                }
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
                if (result >= 1)
                {
                    Vector3 scale = operateObj.localScale;
                    scale.z = result;
                    operateObj.localScale = scale;
                    objectOperate.SetArrowPosition();
                }
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
                if (result >= 1)
                {
                    Vector3 scale = operateObj.localScale;
                    scale.y = result;
                    operateObj.localScale = scale;
                    objectOperate.SetArrowPosition();
                }
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
                objectOperate.SetArrowPosition();
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
                objectOperate.SetArrowPosition();
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
                objectOperate.SetArrowPosition();
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
                operateObj.Rotate(operateObj.transform.right, result);
                objectOperate.SetArrowPosition();
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
                operateObj.Rotate(operateObj.transform.up, result);
                objectOperate.SetArrowPosition();
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
                operateObj.Rotate(operateObj.transform.forward, result);
                objectOperate.SetArrowPosition();
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
            setParamPanel.SetActive(false);
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
        AddGoodsPanel.SetActive(false);
        SelectRoomPanel.SetActive(!SelectRoomPanel.activeSelf);
        personCanvasGraphicRaycaster.enabled = !SelectRoomPanel.activeSelf;
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
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            if (scale.x - distance >= 1)
            {
                scale.x -= distance;
                operateObj.localScale = scale;
                changInputField.text = operateObj.localScale.x.ToString("0.00");
                objectOperate.SetArrowPosition();
            }
        }
    }

    void AddChange(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.x += distance;
            operateObj.localScale = scale;
            changInputField.text = operateObj.localScale.x.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void ReduceKuan(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            if (scale.z - distance >= 1)
            {
                scale.z -= distance;
                operateObj.localScale = scale;
                kuanInputField.text = operateObj.localScale.z.ToString("0.00");
                objectOperate.SetArrowPosition();
            }
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
            objectOperate.SetArrowPosition();
        }
    }

    void ReduceGao(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            if (scale.y - distance >= 1)
            {
                scale.y -= distance;
                operateObj.localScale = scale;
                gaoInputField.text = operateObj.localScale.y.ToString("0.00");
                objectOperate.SetArrowPosition();
            }
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
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
            objectOperate.SetArrowPosition();
        }
    }

    void ReduceRotateX(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateX -= distance;
            operateObj.Rotate(operateObj.transform.right, -distance);
            xRotateInputField.text = rotateX.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void AddRotateX(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateX += distance;
            operateObj.Rotate(operateObj.transform.right, distance);
            xRotateInputField.text = rotateX.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void ReduceRotateY(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateY -= distance;
            operateObj.Rotate(operateObj.transform.up, -distance);
            yRotateInputField.text = rotateY.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void AddRotateY(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateY += distance;
            operateObj.Rotate(operateObj.transform.up, distance);
            yRotateInputField.text = rotateY.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void ReduceRotateZ(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateZ -= distance;
            operateObj.Rotate(operateObj.transform.forward, -distance);
            zRotateInputField.text = rotateZ.ToString("0.00");
            objectOperate.SetArrowPosition();
        }
    }

    void AddRotateZ(GameObject go, object param)
    {
        if (operateObj)
        {
            rotateZ += distance;
            operateObj.Rotate(operateObj.transform.forward, distance);
            zRotateInputField.text = rotateZ.ToString("0.00");
            objectOperate.SetArrowPosition();
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
            Object obj = Resources.Load("SelectGoodCell");
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
                Object obj = Resources.Load<GameObject>("Furniture/" + tgood.prefabName);
                if (obj)
                {
                    GameObject dragObject = Instantiate(obj) as GameObject;
                    dragObject.transform.localPosition = Vector3.zero;
                    AddGoodsPanel.SetActive(false);
                    objectOperate.canDrag = true;
                    if (objectOperate.targetObj)
                    {
                        Utils.SetObjectHighLight(objectOperate.targetObj.gameObject, false);
                    }
                    objectOperate.targetObj = dragObject.transform;

                    GoodInfo goodInfo = dragObject.GetComponent<GoodInfo>();
                    if (goodInfo == null)
                    {
                        goodInfo = dragObject.AddComponent<GoodInfo>();
                    }
                    goodInfo.currentGood = tgood;
                    Utils.SetObjectHighLight(dragObject, true);
                }
            }
        };
        cell.GetComponent<Image>().sprite = Resources.Load<Sprite>("Furniture/" + data.spriteName);
        cell.SetActive(true);
    }
}
