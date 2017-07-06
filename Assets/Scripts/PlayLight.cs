using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayLight : MonoBehaviour {

    public GameObject lightObj;

    public GameObject baiLight;
    public GameObject heiLight;

    public bool isLigt = false;

    public void TurnOn() {
        if (lightObj) {
            isLigt = true;
            lightObj.SetActive(true);
            baiLight.SetActive(true);
            heiLight.SetActive(false);
        }
    }

    public void TurnOff() {
        if (lightObj) {
            isLigt = false;
            lightObj.SetActive(false);
            baiLight.SetActive(false);
            heiLight.SetActive(true);
        }
    }
}
