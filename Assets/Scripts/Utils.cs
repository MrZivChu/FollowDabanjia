using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Utils : MonoBehaviour
{
    public static void SetObjectHighLight(GameObject go, bool isLight, Color color)
    {
        if (isLight)
        {
            Color lightColor = color;
            if (lightColor == Color.clear)
            {
                lightColor = new Color(1, 248f / 255, 0, 1);
            }
            FlashingController flashingController = go.GetComponent<FlashingController>();
            if (flashingController == null)
            {
                flashingController = go.AddComponent<FlashingController>();
                flashingController.flashingDelay = 0;
                flashingController.flashingFrequency = 2;
            }
            flashingController.flashingStartColor = lightColor;
            flashingController.flashingEndColor = lightColor;
            HighlightableObject highlightableObject = go.GetComponent<HighlightableObject>();
            if (highlightableObject)
            {
                highlightableObject.FlashingParams(lightColor, lightColor, 2);
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

    public static void ChangeShaderAlbedo(GameObject target, Texture texture)
    {
        MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            if (material != null)
            {
                material.SetTexture("_MainTex", texture);
            }
        }
    }

    public static void ChangeShaderNormalMap(GameObject target, Texture texture)
    {
        MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            if (material != null)
            {
                material.SetTexture("_BumpMap", texture);
            }
        }
    }

    public static void ChangeShaderOcclusion(GameObject target, Texture texture)
    {
        MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            if (material != null)
            {
                material.SetTexture("_OcclusionMap", texture);
            }
        }
    }

    public static void ChangeShaderEmission(GameObject target, Color newColor) {
        MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
        if (meshRenderer != null) {
            Material material = meshRenderer.material;
            if (material != null) {
                material.SetColor("_EmissionColor", newColor);
            }
        }
    }

    public static void WorldToRectTransfom(Canvas tcanvas, Vector3 screenPosition, GameObject uiObj)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tcanvas.transform as RectTransform, screenPosition, tcanvas.worldCamera, out pos);
        RectTransform rect = uiObj.transform as RectTransform;
        rect.anchoredPosition = pos;
    }
}
