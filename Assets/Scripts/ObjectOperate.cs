using UnityEngine;
using System.Collections;

public class ObjectOperate : MonoBehaviour
{

    Object obj;
    void Start()
    {
        obj = Resources.Load("Cube");
    }

    GameObject go;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            go = Instantiate(obj) as GameObject;
        }
    }
}
