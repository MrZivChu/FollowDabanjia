using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectRootRoom : MonoBehaviour
{
    public SelectRoom selectRoom;
    public GameObject main2;

    public Button Btn1;
    public Button Btn2;
    public Button Btn3;

    private void Start()
    {
        EventTriggerListener.Get(Btn1.gameObject, 0).onClick = MyClick;
        EventTriggerListener.Get(Btn2.gameObject, 1).onClick = MyClick;
        EventTriggerListener.Get(Btn3.gameObject, 2).onClick = MyClick;
    }

    void MyClick(GameObject go, object param)
    {
        int type = (int)param;
        selectRoom.selectToggleIndex = type;
        main2.SetActive(true);
        gameObject.SetActive(false);
    }

}
