using UnityEngine;
using System.Collections;

public class DragRotateZWH : MonoBehaviour
{
    void Update()
    {
        float axisX = -Input.GetAxis("Mouse X");
        if (axisX != 0)
        {
            gameObject.transform.Rotate(transform.up, axisX * 10);
        }
    }
}
