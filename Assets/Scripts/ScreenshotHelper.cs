using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenshotHelper : MonoBehaviour
{

    public GameObject mainUI;
    public Button photoBtn;

    private void Start()
    {
        EventTriggerListener.Get(photoBtn.gameObject).onClick = StartPhoto;
    }

    public void StartPhoto(GameObject obj, object param)
    {
        mainUI.SetActive(false);
        StartCoroutine(Photo());
    }

    IEnumerator Photo()
    {
        Application.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot.png", 0);
        yield return new WaitForEndOfFrame();
        mainUI.SetActive(true);
    }
}
