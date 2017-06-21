using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GoodType
{
    none = 0,
    spawnObj = 1,//生成对象
    changeImg = 2 //更换贴图
}

public class Goods
{
    public string id;//物体的编号
    public string spriteName;//物体对应的预览图或者是贴图
    public string name;//物体的名称
    public string home;//物体的生产厂家
    public string prefabName;//物体对应的预设名称
    public string chang;//物体的长
    public string kuan;//物体的宽
    public string gao;//物体的高
    public List<string> tags; //物体能够放置在哪些标记的物体上
    public GoodType goodType; //物体的类型，（例如墙纸知识更改贴图，桌子椅子等则是生成对象）
}

public class GoodInfo : MonoBehaviour
{
    public Goods currentGood;
}
