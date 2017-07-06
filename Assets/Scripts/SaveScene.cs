using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class GameObjectParam
{
    public Vector3 position;
    public Vector3 rotate;
    public Vector3 scale;
    public string prefabPath;
}



public class SaveScene : MonoBehaviour
{
    private static Dictionary<string, List<GameObject>> GameObjectDic = new Dictionary<string, List<GameObject>>();

    public static void AddGameObject(string prefabPath, GameObject obj)
    {
        if (GameObjectDic.ContainsKey(prefabPath))
        {
            GameObjectDic[prefabPath].Add(obj);
        }
        else
        {
            GameObjectDic[prefabPath] = new List<GameObject>() { obj };
        }
    }

    public static void DeleteGameObject(string prefabPath, GameObject obj)
    {
        if (GameObjectDic.ContainsKey(prefabPath))
        {
            List<GameObject> list = GameObjectDic[prefabPath];
            if (list.Contains(obj))
            {
                list.Remove(obj);
            }
        }
    }

    private void Awake()
    {
        filePath = Application.streamingAssetsPath + "/config.txt";
    }

    private void Start()
    {
        List<GameObjectParam> list = ReadConfigData(filePath);
        if (list != null && list.Count > 0)
        {
            GameObjectParam gop = null;
            for (int i = 0; i < list.Count; i++)
            {
                gop = list[i];
                UnityEngine.Object obj = Resources.Load<GameObject>(gop.prefabPath);
                if (obj)
                {
                    GameObject dragObject = Instantiate(obj) as GameObject;
                   

                    GoodInfo goodInfo = dragObject.GetComponent<GoodInfo>();
                    if (goodInfo == null)
                    {
                        goodInfo = dragObject.AddComponent<GoodInfo>();
                    }
                    goodInfo.rootObj = dragObject;
                    if (MainUI.goodsDic.ContainsKey(gop.prefabPath))
                    {
                        goodInfo.currentGood = MainUI.goodsDic[gop.prefabPath];
                    }

                    //获取对象的中心点和上顶点
                    goodInfo.centerY = dragObject.transform.position.y;
                    if (goodInfo.topObj != null)
                    {
                        goodInfo.topY = goodInfo.topObj.transform.position.y;
                    }

                    dragObject.transform.position = gop.position;
                    dragObject.transform.rotation = Quaternion.Euler(gop.rotate);
                    dragObject.transform.localScale = gop.scale;
                }
            }
        }
    }

    string filePath = string.Empty;
    private void OnApplicationQuit()
    {
        List<GameObjectParam> list = new List<GameObjectParam>();
        if (GameObjectDic != null && GameObjectDic.Count > 0)
        {
            GameObjectParam gop = null;
            foreach (var item in GameObjectDic)
            {
                string key = item.Key;
                List<GameObject> l = item.Value;
                if (l != null && l.Count > 0)
                {
                    GameObject gg = null;
                    for (int i = 0; i < l.Count; i++)
                    {
                        gg = l[i];
                        gop = new GameObjectParam();
                        gop.prefabPath = key;
                        gop.position = gg.transform.position;
                        print(gop.position);
                        gop.rotate = gg.transform.rotation.eulerAngles;
                        gop.scale = gg.transform.localScale;
                        list.Add(gop);
                    }
                }
            }
        }
        UpdateConfigData(filePath, list);
    }

    public static List<GameObjectParam> ReadConfigData(string filePath)
    {
        List<GameObjectParam> list = new List<GameObjectParam>();
        if (File.Exists(filePath))
        {
            string[] result = new string[4];
            result = File.ReadAllLines(filePath, Encoding.UTF8);
            if (result != null && result.Length > 0)
            {
                GameObjectParam pdfData = null;
                string tempdata = string.Empty;
                string[] tempArray = null;
                for (int i = 0; i < result.Length; i++)
                {
                    tempdata = result[i];
                    if (!string.IsNullOrEmpty(tempdata))
                    {
                        pdfData = new GameObjectParam();
                        tempArray = tempdata.Split(new string[] { "|" }, System.StringSplitOptions.None);
                        pdfData.prefabPath = tempArray[0];

                        string pp = tempArray[1].TrimStart('(').TrimEnd(')');
                        string[] aa = pp.Split(new string[] { "," }, System.StringSplitOptions.None);
                        pdfData.position = new Vector3(Convert.ToSingle(aa[0]), Convert.ToSingle(aa[1]), Convert.ToSingle(aa[2]));

                        pp = tempArray[2].TrimStart('(').TrimEnd(')');
                        string[] bb = pp.Split(new string[] { "," }, System.StringSplitOptions.None);
                        pdfData.rotate = new Vector3(Convert.ToSingle(bb[0]), Convert.ToSingle(bb[1]), Convert.ToSingle(bb[2]));

                        pp = tempArray[3].TrimStart('(').TrimEnd(')');
                        string[] cc = pp.Split(new string[] { "," }, System.StringSplitOptions.None);
                        pdfData.scale = new Vector3(Convert.ToSingle(cc[0]), Convert.ToSingle(cc[1]), Convert.ToSingle(cc[2]));

                        list.Add(pdfData);
                    }
                }
            }
        }
        return list;
    }

    public static void UpdateConfigData(string filePath, List<GameObjectParam> list)
    {
        if (File.Exists(filePath) && list != null && list.Count > 0)
        {
            List<string> templist = new List<string>();
            string tempStr = string.Empty;
            foreach (var item in list)
            {
                tempStr = item.prefabPath + "|" + item.position + "|" + item.rotate + "|" + item.scale;
                templist.Add(tempStr);
            }
            File.WriteAllLines(filePath, templist.ToArray());
        }
    }
}



