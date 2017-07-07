using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class GameObjectParam
{
    public Vector3 position;
    public Vector3 rotate;
    public Vector3 scale;
    public string prefabPath;
}

public class ImageParam
{
    public string albedo;
    public string normalMap;
    public string occlusion;
    public string prefabPath;
}

public class SaveScene : MonoBehaviour
{
    Dictionary<string, List<GameObject>> gameObjectDic = new Dictionary<string, List<GameObject>>();

    public void AddGameObject(string prefabPath, GameObject obj)
    {
        if (gameObjectDic.ContainsKey(prefabPath))
        {
            gameObjectDic[prefabPath].Add(obj);
        }
        else
        {
            gameObjectDic[prefabPath] = new List<GameObject>() { obj };
        }
    }

    public void DeleteGameObject(string prefabPath, GameObject obj)
    {
        if (gameObjectDic.ContainsKey(prefabPath))
        {
            List<GameObject> list = gameObjectDic[prefabPath];
            if (list.Contains(obj))
            {
                list.Remove(obj);
            }
        }
    }

    Dictionary<string, string> imageDic = new Dictionary<string, string>();
    public void ChangeImage(string objPath, string albedo, string normalMap, string occlusion)
    {
        imageDic[objPath] = albedo + "," + normalMap + "," + occlusion;
    }

    List<string> deleteCantDragObjList = new List<string>();
    public void AddDeleteCantDragObj(string objPath)
    {
        if (!deleteCantDragObjList.Contains(objPath))
        {
            deleteCantDragObjList.Add(objPath);
        }
    }

    public static string roomPrefabPath = string.Empty;
    private void Awake()
    {
        if (!string.IsNullOrEmpty(roomPrefabPath))
        {
            roomPrefabPath = roomPrefabPath.Replace("/", "");
            gameObjectFilePath = Application.streamingAssetsPath + "/" + roomPrefabPath + "objects.txt";
            imageFilePath = Application.streamingAssetsPath + "/" + roomPrefabPath + "images.txt";
            deleteCantDragObjFilePath = Application.streamingAssetsPath + "/" + roomPrefabPath + "cantdragdelete.txt";
        }
    }

    private void Start()
    {
        List<GameObjectParam> list = ReadGameObjectConfigData(gameObjectFilePath);
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

                    AddGameObject(gop.prefabPath, dragObject);
                }
            }
        }

        List<ImageParam> list2 = ReadImageConfigData(imageFilePath);
        if (list2 != null && list2.Count > 0)
        {
            ImageParam ip = null;
            for (int i = 0; i < list2.Count; i++)
            {
                ip = list2[i];
                GameObject tt = GameObject.Find(ip.prefabPath);
                if (tt != null)
                {
                    Texture t1 = Resources.Load<Texture>(ip.albedo);
                    Utils.ChangeShaderAlbedo(tt, t1);
                    Texture t2 = Resources.Load<Texture>(ip.normalMap);
                    Utils.ChangeShaderNormalMap(tt, t2);
                    Texture t3 = Resources.Load<Texture>(ip.occlusion);
                    Utils.ChangeShaderOcclusion(tt, t3);

                    ChangeImage(ip.prefabPath, ip.albedo, ip.normalMap, ip.occlusion);
                }
            }
        }

        List<string> list3 = ReadDeleteCantDragConfigData(deleteCantDragObjFilePath);
        if (list3 != null && list3.Count > 0)
        {
            for (int i = 0; i < list3.Count; i++)
            {
                GameObject tt = GameObject.Find(list3[i]);
                if (tt != null)
                {
                    AddDeleteCantDragObj(list3[i]);
                    Destroy(tt);
                }
            }
        }
    }

    string gameObjectFilePath = string.Empty;
    string imageFilePath = string.Empty;
    string deleteCantDragObjFilePath = string.Empty;
    private void OnApplicationQuit()
    {
        StartSaveScene();
    }

    public void StartSaveScene()
    {
        List<GameObjectParam> list = new List<GameObjectParam>();
        if (gameObjectDic != null && gameObjectDic.Count > 0)
        {
            GameObjectParam gop = null;
            foreach (var item in gameObjectDic)
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
        UpdateGameObjectConfigData(gameObjectFilePath, list);


        List<ImageParam> list2 = new List<ImageParam>();
        if (imageDic != null && imageDic.Count > 0)
        {
            ImageParam gop = null;
            foreach (var item in imageDic)
            {
                gop = new ImageParam();
                gop.prefabPath = item.Key;
                string temp = item.Value;
                string[] aa = temp.Split(new string[] { "," }, System.StringSplitOptions.None);
                gop.albedo = aa[0];
                gop.normalMap = aa[1];
                gop.occlusion = aa[2];
                list2.Add(gop);
            }
        }
        UpdateImageConfigData(imageFilePath, list2);


        UpdateDeleteCantDragConfigData(deleteCantDragObjFilePath, deleteCantDragObjList);
    }

    public List<GameObjectParam> ReadGameObjectConfigData(string filePath)
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

    public void UpdateGameObjectConfigData(string filePath, List<GameObjectParam> list)
    {
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { }
        }
        List<string> templist = new List<string>();
        if (File.Exists(filePath) && list != null && list.Count > 0)
        {
            string tempStr = string.Empty;
            foreach (var item in list)
            {
                tempStr = item.prefabPath + "|" + item.position + "|" + item.rotate + "|" + item.scale;
                templist.Add(tempStr);
            }
        }
        File.WriteAllLines(filePath, templist.ToArray());
    }

    public void UpdateImageConfigData(string filePath, List<ImageParam> list)
    {
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { }
        }
        List<string> templist = new List<string>();
        if (File.Exists(filePath) && list != null && list.Count > 0)
        {
            string tempStr = string.Empty;
            foreach (var item in list)
            {
                tempStr = item.prefabPath + "|" + item.albedo + "|" + item.normalMap + "|" + item.occlusion;
                templist.Add(tempStr);
            }
        }
        File.WriteAllLines(filePath, templist.ToArray());
    }

    public List<ImageParam> ReadImageConfigData(string filePath)
    {
        List<ImageParam> list = new List<ImageParam>();
        if (File.Exists(filePath))
        {
            string[] result = new string[4];
            result = File.ReadAllLines(filePath, Encoding.UTF8);
            if (result != null && result.Length > 0)
            {
                ImageParam data = null;
                string tempdata = string.Empty;
                string[] tempArray = null;
                for (int i = 0; i < result.Length; i++)
                {
                    tempdata = result[i];
                    if (!string.IsNullOrEmpty(tempdata))
                    {
                        data = new ImageParam();
                        tempArray = tempdata.Split(new string[] { "|" }, System.StringSplitOptions.None);
                        data.prefabPath = tempArray[0];
                        data.albedo = tempArray[1];
                        data.normalMap = tempArray[2];
                        data.occlusion = tempArray[3];
                        list.Add(data);
                    }
                }
            }
        }
        return list;
    }


    public void UpdateDeleteCantDragConfigData(string filePath, List<string> list)
    {
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { }
        }
        List<string> result = new List<string>();
        if (File.Exists(filePath) && list != null && list.Count > 0)
        {
            result = list;
        }
        File.WriteAllLines(filePath, result.ToArray());
    }

    public List<string> ReadDeleteCantDragConfigData(string filePath)
    {
        List<string> list = new List<string>();
        if (File.Exists(filePath))
        {
            string[] result = new string[4];
            result = File.ReadAllLines(filePath, Encoding.UTF8);
            if (result != null && result.Length > 0)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    list.Add(result[i]);
                }
            }
        }
        return list;
    }

}



