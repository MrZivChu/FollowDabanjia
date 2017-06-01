using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    Button btn;
    private void Start()
    {
        EventTriggerListener.Get(btn.gameObject).onClick = (go,data) => { };
    }
}
