using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectUIOperate : MonoBehaviour
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

        changInputField.text = operateObj.localScale.x.ToString();
        kuanInputField.text = operateObj.localScale.z.ToString();
        gaoInputField.text = operateObj.localScale.y.ToString();

        xInputField.text = operateObj.position.x.ToString();
        yInputField.text = operateObj.position.y.ToString();
        zInputField.text = operateObj.position.z.ToString();

        changInputField.onValueChanged.AddListener(ChangOnValueChanged);
        kuanInputField.onValueChanged.AddListener(KuanOnValueChanged);
        gaoInputField.onValueChanged.AddListener(GaoOnValueChanged);

        xInputField.onValueChanged.AddListener(XOnValueChanged);
        yInputField.onValueChanged.AddListener(YOnValueChanged);
        zInputField.onValueChanged.AddListener(ZOnValueChanged);
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
    public float distance = 0.5f;
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

}
