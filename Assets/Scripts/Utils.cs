using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{
    public static void SetObjectHighLight(GameObject go, bool isLight)
    {
        if (isLight)
        {
            FlashingController flashingController = go.GetComponent<FlashingController>();
            if (flashingController == null)
            {
                flashingController = go.AddComponent<FlashingController>();
                flashingController.flashingDelay = 0;
                flashingController.flashingFrequency = 2;
                flashingController.flashingStartColor = new Color(0, 1, 1, 1);
                flashingController.flashingEndColor = new Color(0, 1, 1, 1);
            }
            HighlightableObject highlightableObject = go.GetComponent<HighlightableObject>();
            if (highlightableObject)
            {
                highlightableObject.FlashingOn();
            }
        }
        else
        {
            HighlightableObject highlightableObject = go.GetComponent<HighlightableObject>();
            if (highlightableObject)
            {
                highlightableObject.FlashingOff();
            }
        }
    }


    public static void SpawnCellForTable<T>(Transform parent, List<T> dataList, Action<GameObject, T, bool, int> func)
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
                func(go, dataList[i], false, i);
            }
        }

        if (spawnCount > 0)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                func(parent.gameObject, dataList[fuyongCount + i], true, fuyongCount + i);
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
