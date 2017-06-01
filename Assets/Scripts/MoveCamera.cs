using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    Transform myTransform;
    Camera myCamera;
    private void Start()
    {
        myTransform = transform;
        myCamera = GetComponent<Camera>();
    }

    public void Up()
    {

    }

    public void Down()
    {

    }

    public void Left()
    {

    }

    public void Right()
    {

    }

    public void Near()
    {
        myCamera.fieldOfView -= 2;
    }

    public void Far()
    {
        myCamera.fieldOfView += 2;
    }




}
