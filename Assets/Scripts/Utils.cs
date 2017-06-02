using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{

    public static void SpawnCellForTable<T>(Transform parent, List<T> dataList, Action<GameObject, T, bool> func)
    {
        if (parent == null)
            return;
        if (dataList == null)
            return;

        int hasCount = parent.childCount;
        int showCount = dataList.Count;

        int fuyongCount = 0;
        int spawnCount = 0;
        int hideCount = 0;

        if (showCount >= hasCount)
        {
            fuyongCount = hasCount;
            spawnCount = showCount - fuyongCount;
            hideCount = 0;
        }
        else
        {
            spawnCount = 0;
            fuyongCount = showCount;
            hideCount = hasCount - showCount;
        }

        if (fuyongCount > 0)
        {
            for (int i = 0; i < fuyongCount; i++)
            {
                GameObject go = parent.GetChild(i).gameObject;
                func(go, dataList[i], false);
            }
        }

        if (spawnCount > 0)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                func(parent.gameObject, dataList[fuyongCount + i], true);
            }
        }

        if (hideCount > 0)
        {
            for (int i = fuyongCount; i < hasCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
