using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Room
{
    public string roomPrefabName;
    public Sprite roomSprite;
}

public class SelectRoom : MonoBehaviour
{
    [HideInInspector]
    public int selectToggleIndex = 0;
    public List<Toggle> toggleList = new List<Toggle>();


    public GameObject main1;
    public Button backBtn;

    public List<GameObject> BtnList = new List<GameObject>();
    Dictionary<int, List<Room>> dic = new Dictionary<int, List<Room>>();


    public Toggle tog1;
    public Toggle tog2;
    public GameObject p1;
    public GameObject p2;

    private void Start()
    {
        for (int i = 0; i < BtnList.Count; i++)
        {
            EventTriggerListener.Get(BtnList[i].gameObject, i).onClick = Click;
            string roomDirName = "style" + (i + 1);
            Sprite[] spriteArrary = Resources.LoadAll<Sprite>(roomDirName);
            dic[i] = new List<Room>() { };
            if (spriteArrary != null && spriteArrary.Length > 0)
            {
                for (int j = 0; j < spriteArrary.Length; j++)
                {
                    Room room = new Room();
                    room.roomSprite = spriteArrary[j];
                    room.roomPrefabName = roomDirName + "/" + spriteArrary[j].name;
                    dic[i].Add(room);
                }
            }
        }
        toggleList[selectToggleIndex].isOn = true;
        EventTriggerListener.Get(backBtn.gameObject).onClick = (go, param) =>
        {
            main1.SetActive(true);
            gameObject.SetActive(false);
        };


        tog1.onValueChanged.AddListener((isOn) => {
            p1.SetActive(isOn);
        });
        tog2.onValueChanged.AddListener((isOn) => {
            p2.SetActive(isOn);
        });
    }

    List<Room> clickListRoom = new List<Room>();
    public Transform group;
    void Click(GameObject go, object data)
    {
        int index = (int)data;
        if (dic.ContainsKey(index))
        {
            clickListRoom = dic[index];
            Utils.SpawnCellForTable(group, clickListRoom, SpawnOrUpdate);
        }
    }

    public GameObject roomInfo;
    void SpawnOrUpdate(GameObject go, Room data, bool isSpawn, int index)
    {
        GameObject cell = go;
        if (isSpawn)
        {
            Object obj = Resources.Load("SelectRoomCell");
            if (obj)
            {
                cell = Instantiate(obj) as GameObject;
                cell.transform.parent = go.transform;
            }
        }
        EventTriggerListener.Get(cell, data).onClick = (tgo, tdata) =>
        {
            RoomInfo ri = roomInfo.GetComponent<RoomInfo>();
            ri.roomList = clickListRoom;
            ri.index = index;
            ri.Init();
            roomInfo.SetActive(true);
        };
        cell.GetComponent<Image>().sprite = data.roomSprite;
        cell.SetActive(true);
    }

    public void SelectRoomOK(string roomPrefabPath)
    {
        SpawnRoom.roomPrefabPath = roomPrefabPath;
        Loading.index = 3;
        SceneManager.LoadScene("Loading");
    }
}
