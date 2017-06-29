using UnityEngine;
using System.Collections;

public class SpawnRoom : MonoBehaviour
{
    public static string roomPrefabPath = string.Empty;
    private void Awake()
    {
        if (!string.IsNullOrEmpty(roomPrefabPath))
        {
            GameObject room = Instantiate(Resources.Load<GameObject>(roomPrefabPath)) as GameObject;
            room.transform.position = Vector3.zero;

            string playPath = roomPrefabPath;
            int endIndex = playPath.LastIndexOf("/");
            playPath = roomPrefabPath.Substring(0, endIndex);
            GameObject player = Instantiate(Resources.Load(playPath + "/FPSController")) as GameObject;
        }
    }
}
