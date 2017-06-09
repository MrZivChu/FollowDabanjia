using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowGoodDetail : MonoBehaviour
{
    public Text tname;
    public Text home;
    public Text chang;
    public Text kuan;
    public Text gao;

    void Start()
    {
        EventTriggerListener.Get(gameObject).onClick = (go, param) =>
        {
            gameObject.SetActive(false);
        };
    }

    public void SetValue(string ttname, string thome, string tchang, string tkuan, string tgao)
    {
        tname.text = ttname;
        home.text = thome;
        chang.text = tchang;
        kuan.text = tkuan;
        gao.text = tgao;
    }

}
