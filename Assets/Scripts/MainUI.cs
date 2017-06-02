using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

class Goods
{
    public Sprite sprite;
    public string name;
    public float price;
    public string prefabName;
}

enum GoodsType
{
    TDefault = 0,
    Wall = 1,
    Ceil = 2
}

public class MainUI : MonoBehaviour
{

    public Button deleteBtn;
    public Button moveBtn;
    public Button rotateBtn;
    public Button scaleBtn;

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

    private void Start()
    {
        EventTriggerListener.Get(deleteBtn.gameObject).onClick = DeleteObject;
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

        changInputField.onValueChanged.AddListener(ChangOnValueChanged);
        kuanInputField.onValueChanged.AddListener(KuanOnValueChanged);
        gaoInputField.onValueChanged.AddListener(GaoOnValueChanged);

        xInputField.onValueChanged.AddListener(XOnValueChanged);
        yInputField.onValueChanged.AddListener(YOnValueChanged);
        zInputField.onValueChanged.AddListener(ZOnValueChanged);

        for (int i = 0; i < LeftButtonList.Count; i++)
        {
            EventTriggerListener.Get(LeftButtonList[i].gameObject, i).onClick = ShowStore;
        }

        goodsDic = new Dictionary<GoodsType, List<Goods>>() {
            {
                GoodsType.Wall,new List<Goods>() {
                   new Goods(){ sprite = null , name ="wall1", price = 100 ,prefabName = "Wall1" }
                }
            },
            {
                GoodsType.Ceil,new List<Goods>() {
                   new Goods(){ sprite = null , name ="floor1", price = 10000 , prefabName = "Wall1" },
                   new Goods(){ sprite = null , name ="floor2", price = 2000 , prefabName = "Wall1" }
                }
            }

        };
    }

    public void InitObjectData(Transform tTarget)
    {
        operateObj = tTarget;
        if (operateObj)
        {
            changInputField.text = operateObj.localScale.x.ToString();
            kuanInputField.text = operateObj.localScale.z.ToString();
            gaoInputField.text = operateObj.localScale.y.ToString();

            xInputField.text = operateObj.position.x.ToString();
            yInputField.text = operateObj.position.y.ToString();
            zInputField.text = operateObj.position.z.ToString();
        }
    }

    void ChangOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 scale = operateObj.localScale;
                scale.x = result;
                operateObj.localScale = scale;
            }
        }
    }

    void KuanOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 scale = operateObj.localScale;
                scale.z = result;
                operateObj.localScale = scale;
            }
        }
    }


    void GaoOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 scale = operateObj.localScale;
                scale.y = result;
                operateObj.localScale = scale;
            }
        }
    }


    void XOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.x = result;
                operateObj.position = position;
            }
        }
    }


    void YOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.y = result;
                operateObj.position = position;
            }
        }
    }


    void ZOnValueChanged(string content)
    {
        if (!string.IsNullOrEmpty(content))
        {
            float result;
            bool isok = float.TryParse(content, out result);
            if (isok)
            {
                Vector3 position = operateObj.position;
                position.z = result;
                operateObj.position = position;
            }
        }
    }


    public Transform operateObj;
    public float distance = 1f;
    void DeleteObject(GameObject go, object param)
    {
        if (operateObj)
        {
            Destroy(operateObj);
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

    void ReduceChange(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.x -= distance;
            operateObj.localScale = scale;
            changInputField.text = operateObj.localScale.x.ToString();
        }
    }

    void AddChange(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.x += distance;
            operateObj.localScale = scale;
            changInputField.text = operateObj.localScale.x.ToString();
        }
    }

    void ReduceKuan(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.z -= distance;
            operateObj.localScale = scale;
            kuanInputField.text = operateObj.localScale.z.ToString();
        }
    }

    void AddKuan(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.z += distance;
            operateObj.localScale = scale;
            kuanInputField.text = operateObj.localScale.z.ToString();
        }
    }

    void ReduceGao(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.y -= distance;
            operateObj.localScale = scale;
            gaoInputField.text = operateObj.localScale.y.ToString();
        }
    }

    void AddGao(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 scale = operateObj.localScale;
            scale.y += distance;
            operateObj.localScale = scale;
            gaoInputField.text = operateObj.localScale.y.ToString();
        }
    }

    void ReduceX(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.x -= distance;
            operateObj.position = position;
            xInputField.text = operateObj.position.x.ToString();
        }
    }

    void AddX(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.x += distance;
            operateObj.position = position;
            xInputField.text = operateObj.position.x.ToString();
        }
    }

    void ReduceY(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.y -= distance;
            operateObj.position = position;
            yInputField.text = operateObj.position.y.ToString();
        }
    }

    void AddY(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.y += distance;
            operateObj.position = position;
            yInputField.text = operateObj.position.y.ToString();
        }
    }

    void ReduceZ(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.z -= distance;
            operateObj.position = position;
            zInputField.text = operateObj.position.z.ToString();
        }
    }

    void AddZ(GameObject go, object param)
    {
        if (operateObj)
        {
            Vector3 position = operateObj.position;
            position.z += distance;
            operateObj.position = position;
            zInputField.text = operateObj.position.z.ToString();
        }
    }


    public Transform storeParent;
    public List<Button> LeftButtonList = new List<Button>();
    Dictionary<GoodsType, List<Goods>> goodsDic = null;


    void ShowStore(GameObject go, object data)
    {
        storeParent.parent.gameObject.SetActive(true);
        int theType = (int)data;
        if (goodsDic.ContainsKey((GoodsType)theType))
        {
            Utils.SpawnCellForTable(storeParent, goodsDic[(GoodsType)theType], SpawnOrUpdate);
        }
    }

    void SpawnOrUpdate(GameObject go, Goods data, bool isSpawn)
    {
        GameObject cell = go;
        if (isSpawn)
        {
            Object obj = Resources.Load("StoreCell");
            cell = Instantiate(obj) as GameObject;
        }
        StoreCell sc = cell.GetComponent<StoreCell>();
        sc.InitCell(data.sprite, data.name, data.price.ToString(), data.prefabName);
        if (isSpawn)
        {
            cell.transform.parent = go.transform;
        }
        sc.callback = SpawnObject;
        cell.SetActive(true);
    }

    private void SpawnObject(GameObject go, object data, string prefabName)
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

        Object obj = Resources.Load<GameObject>(prefabName);
        GameObject spawnObj = Instantiate(obj) as GameObject;
        spawnObj.transform.position = ray.direction;
        spawnObj.transform.localRotation = Quaternion.identity;

        storeParent.parent.gameObject.SetActive(false);
    }
}
