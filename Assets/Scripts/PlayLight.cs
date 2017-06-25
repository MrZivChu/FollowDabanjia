using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayLight : MonoBehaviour {

    public GameObject lightObj;
    public GameObject target;

    [HideInInspector]
    public bool isLigt = false;


    void Start() {
        isLigt = true;
    }

    public void TurnOn() {
        if (lightObj) {
            isLigt = true;
            lightObj.SetActive(true);
            Utils.ChangeShaderEmission(target, new Color(0.90f, 0.86f, 0.61f, 1f));
        }
    }

    public void TurnOff() {
        if (lightObj) {
            isLigt = false;
            lightObj.SetActive(false);
            Utils.ChangeShaderEmission(target, Color.red);
        }
    }

    private void OnDestroy() {
        if (lightObj) {
            Utils.ChangeShaderEmission(target, new Color(0.90f, 0.86f, 0.61f, 1f));
        }
    }
}
