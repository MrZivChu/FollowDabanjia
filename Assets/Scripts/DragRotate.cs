using UnityEngine;
using System.Collections;

public class DragRotate : MonoBehaviour
{
    private bool onDrag = false;  //是否被拖拽
    public float speed = 6f;     //旋转速度
    private float tempSpeed;  //阻尼速度
    private float axisX;     //鼠标沿水平方向移动的增量
    private float axisY;    //鼠标沿垂直方向移动的增量
    private float cXY;     //鼠标移动的距离

    public void tOnMouseDown()
    {
        //鼠标按下的事件
        axisX = 0f;
        axisY = 0f;
    }

    public void tOnMouseDrag()
    {
        //鼠标拖拽时的操作
        onDrag = true;
        axisX = -Input.GetAxis("Mouse X");  //获得鼠标增量
        axisY = -Input.GetAxis("Mouse Y");
        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY); //计算鼠标移动长度
        if (cXY == 0f)
        {
            cXY = 1f;
        }
    }

    float Rigid()
    {  
        //计算阻尼速度
        if (onDrag)
        {
            tempSpeed = speed;
        }
        else
        {
            if (tempSpeed > 0f)
            {
                tempSpeed -= speed * 2 * Time.deltaTime / cXY; //通过除以鼠标移动长度实现拖拽越长速度减缓越慢
            }
            else
            {
                tempSpeed = 0f;
            }
        }
        return tempSpeed;
    }

    float distance = 0f;
    void Update()
    {
        //放大缩小  鼠标中建
        //float size = Input.GetAxis("Mouse ScrollWheel");
        //if (size > 0)
        //{
        //    if (transform.localScale.x < 0.18f)
        //    {
        //        transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        //    }

        //}
        //else if (size < 0)
        //{
        //    if (transform.localScale.x > 0.05)
        //    {
        //        transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        //    }
        //}

        gameObject.transform.Rotate(new Vector3(axisY, axisX, 0f) * Rigid(), Space.World);
        if (!Input.GetMouseButton(0))
        {
            onDrag = false;
        }
    }
}
