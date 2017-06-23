using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum GoodType
{
    none = 0,
    spawnObj = 1,//生成对象
    changeImg = 2 //更换贴图
}

[Serializable]
public class Goods
{
    public string id;//物体的编号
    public string spriteName;//物体对应的预览图
    public string name;//物体的名称
    public string home;//物体的生产厂家
    public string prefabName;//物体对应生成的预设名称
    public string chang;//物体的长
    public string kuan;//物体的宽
    public string gao;//物体的高
    public List<string> tags; //物体能够放置在哪些标记的物体上
    public GoodType goodType = GoodType.spawnObj; //物体的类型，（例如墙纸知识更改贴图，桌子椅子等则是生成对象）
    public string albedo;//物体的Albedo贴图
    public string normalMap;//物体的法线贴图
    public string occlusion;//物体的环境光遮蔽贴图
}

public class GoodInfo : MonoBehaviour
{
    public Goods currentGood;

    public GameObject rootObj;//用于记录一个家具根节点的对象
    public GameObject topObj;//用于记录顶部部件的位置(主要用于吊灯等放置于屋顶的家具)

    [HideInInspector]
    public float centerY;
    [HideInInspector]
    public float topY;
    
}
