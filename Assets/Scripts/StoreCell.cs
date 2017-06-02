using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StoreCell : MonoBehaviour
{
    public Image bg;
    public Text goodName;
    public Text goodPrice;
    public string prefabName;

    public Action<GameObject, object, string> callback;
    private void Start()
    {
        EventTriggerListener.Get(gameObject).onClick = (go, data) =>
        {
            if (callback != null)
            {
                callback(go, data, prefabName);
            }
        };
    }

    public void InitCell(Sprite tsprite, string tname, string tprice,string tprefabName)
    {
        bg.sprite = tsprite;
        goodName.text = tname;
        goodPrice.text = tprice;
        prefabName = tprefabName;
    }

}
