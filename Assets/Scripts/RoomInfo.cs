using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomInfo : MonoBehaviour
{
    Dictionary<int, GameObject> hasSpawnObject = new Dictionary<int, GameObject>();

    public Transform spawnPosition;
    public Transform replacePosition;
    public GameObject SelectRoom;

    [HideInInspector]
    public List<Room> roomList = new List<Room>() { };
    [HideInInspector]
    public int index = 0;
    public TouchRawImage touchRawImage;

    public void Init()
    {
        InstanceObj(roomList[index].roomPrefabName);
    }

    public void Ok()
    {
        gameObject.SetActive(false);
        SelectRoom.SetActive(false);
        if (currentRoom)
        {
            if (replacePosition.childCount > 0)
            {
                for (int i = 0; i < replacePosition.childCount; i++)
                {
                    Destroy(replacePosition.GetChild(i).gameObject);
                }
            }
            currentRoom.transform.parent = replacePosition;
            currentRoom.transform.localPosition = Vector3.zero;
        }
        currentRoom = null;

        foreach (var item in hasSpawnObject)
        {
            if (item.Key != index)
            {
                Destroy(item.Value);
            }
        }
        hasSpawnObject.Clear();
    }

    public void NextRoom()
    {
        index++;
        if (index >= roomList.Count)
        {
            index = 0;
        }
        InstanceObj(roomList[index].roomPrefabName);
    }

    public void PreRoom()
    {
        index--;
        if (index < 0)
        {
            index = roomList.Count - 1;
        }
        InstanceObj(roomList[index].roomPrefabName);
    }

    public void Close()
    {
        currentRoom = null;
        gameObject.SetActive(false);
        foreach (var item in hasSpawnObject)
        {
            Destroy(item.Value);
        }
        hasSpawnObject.Clear();
    }

    GameObject currentRoom;
    public void InstanceObj(string prefabName)
    {
        foreach (var item in hasSpawnObject)
        {
            item.Value.SetActive(false);
        }
        if (hasSpawnObject.ContainsKey(index))
        {
            currentRoom = hasSpawnObject[index];
            DragRotateZWH dragRotate = currentRoom.GetComponent<DragRotateZWH>();
            if (dragRotate == null)
            {
                dragRotate = currentRoom.AddComponent<DragRotateZWH>();
            }
            dragRotate.enabled = false;
            touchRawImage.dr = dragRotate;
            currentRoom.SetActive(true);
        }
        else
        {
            Object obj = Resources.Load<GameObject>(prefabName);
            if (obj)
            {
                currentRoom = Instantiate(obj) as GameObject;
                DragRotateZWH dragRotate = currentRoom.GetComponent<DragRotateZWH>();
                if (dragRotate == null)
                {
                    dragRotate = currentRoom.AddComponent<DragRotateZWH>();
                }
                dragRotate.enabled = false;
                touchRawImage.dr = dragRotate;
                currentRoom.transform.parent = spawnPosition;
                currentRoom.transform.localPosition = Vector3.zero;
                hasSpawnObject[index] = currentRoom;
            }
        }
    }
}

