using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

class Goods
{
    public Sprite sprite;
    public string name;
    public float money;
}

public class MainUI : MonoBehaviour
{
    public List<Button> MainMenu = null;

    public Transform WallImage;
    Object tgameObject;
    Camera camera;

    List<Goods> wallGoods = new List<Goods>(){
      new Goods(){ sprite = null , name ="wall1", money = 100 }
    };
    List<Goods> floorGoods = new List<Goods>(){
      new Goods(){ sprite = null , name ="floor1", money = 10000 },
      new Goods(){ sprite = null , name ="floor2", money = 2000 }
    };
    List<Goods> ceilGoods = new List<Goods>(){
      new Goods(){ sprite = null , name ="floor1", money = 10000 },
      new Goods(){ sprite = null , name ="floor2", money = 2000 },
      new Goods(){ sprite = null , name ="floor2", money = 2000 }
    };
    List<Goods> fonitureGoods = new List<Goods>()
    {
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 },
        new Goods (){ sprite = null ,name ="sofa",money =5000 }
    };
    private void Start()
    {
        tgameObject = Resources.Load<GameObject>("Cube");
        for (int i = 0; i < MainMenu.Count; i++)
        {
            EventTriggerListener.Get(MainMenu[i].gameObject, i).onClick = ShowPanel;
        }
    }

    public GameObject content;
    void ShowPanel(GameObject go, object data)
    {
        int theType = (int)data;
        List<Goods> list = null;
        if (theType == 0)
        {
            list = wallGoods;
        }
        else if (theType == 1)
        {
            list = floorGoods;
        }
        else if (theType == 2)
        {
            list = fonitureGoods;
        }
        else
        {
            list = ceilGoods;
        }
        int childCount = content.transform.childCount;
        int showCount = list.Count;

        int fuyongCount = 0;
        int newCount = 0;
        int hideCount = 0;
        if (childCount < showCount)
        {
            fuyongCount = childCount;
            newCount = showCount - fuyongCount;
            hideCount = 0;
        }
        else
        {
            newCount = 0;
            fuyongCount = showCount;
            hideCount = childCount - showCount;
        }
        if (fuyongCount > 0)
        {
            for (int i = 0; i < fuyongCount; i++)
            {
                Image image = content.transform.GetChild(i).GetComponent<Image>();
                image.sprite = list[i].sprite;
                image.gameObject.SetActive(true);
            }
        }

        if (newCount > 0)
        {
            GameObject tobj = Instantiate(WallImage.gameObject) as GameObject;
            tobj.transform.parent = content.transform;
            //  EventTriggerListener.Get ()
            tobj.gameObject.SetActive(true);
        }

        if (hideCount > 0)
        {
            for (int i = fuyongCount; i < childCount; i++)
            {
                content.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    Dictionary<string, object> spawn = null;

    void ShowData()
    {
        spawn = new Dictionary<string, object>()
      {
         { "wall",tgameObject},
         { "ceil",tgameObject},
         { "Floor",tgameObject},
         { "door",tgameObject}
      };

    }
    void SpawnObj(GameObject go, object data)
    {
        //if (spawn.ContainsKey("wall"))
        //{
        //    string value = spawn["wall"];
        //}
        //InitObj(tgameObject);
    }
    private void InitObj(Object obj)
    {
        float h = Input.mousePosition.x / Screen.width - 0.5f;
        float v = Input.mousePosition.y / Screen.height - 0.5f;
        GameObject spawnObj = Instantiate(obj) as GameObject;
        spawnObj.transform.position = RectTransformUtility.WorldToScreenPoint(camera, Input.mousePosition);
        spawnObj.transform.position = new Vector3(h, 0, v);
        spawnObj.transform.localRotation = Quaternion.identity;
        spawnObj.transform.localScale = Vector3.one;
    }

}
