using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public List<Sprite> listSprite = new List<Sprite>();
    public Image backBgImage;
    public Image forwardBgImage;

    public static int index;
    public Slider slider;

    int forwardImgIndex = 0;
    int backImgIndex = 0;
    private void Awake()
    {
        slider.value = 0;
        forwardBgImage.sprite = listSprite[forwardImgIndex];
        backBgImage.sprite = listSprite[backImgIndex];
    }

    void Start()
    {
        StartCoroutine(LoadLevelAsyn(index));
        InvokeRepeating("ShowImage", 0.5f, 1.5f);
    }

    void ShowImage()
    {
        ChangeImage();
        forwardBgImage.gameObject.SetActive(true);
        int nextFillIndex = Random.Range(0, listFillMethod.Count);
        if (nextFillIndex == preFillIndex)
        {
            nextFillIndex++;
        }
        if (nextFillIndex >= listFillMethod.Count)
        {
            nextFillIndex = 0;
        }
        forwardBgImage.fillMethod = listFillMethod[nextFillIndex];
        forwardBgImage.fillAmount = 1;
        canRun = true;
    }

    void ChangeImage()
    {
        int nextImgIndex = ++backImgIndex;
        if (nextImgIndex >= listSprite.Count)
        {
            nextImgIndex = 0;
        }
        backImgIndex = nextImgIndex;
        forwardImgIndex = backImgIndex - 1;
        if (forwardImgIndex == -1)
        {
            forwardImgIndex = listSprite.Count - 1;
        }
        backBgImage.sprite = listSprite[backImgIndex];
        forwardBgImage.sprite = listSprite[forwardImgIndex];
    }

    List<Image.FillMethod> listFillMethod = new List<Image.FillMethod>() {
        Image.FillMethod.Horizontal,Image.FillMethod.Radial180,Image.FillMethod.Radial360,Image.FillMethod.Radial90,Image.FillMethod.Vertical
    };
    int preFillIndex = 0;

    bool canRun = false;
    float tempTime = 0;
    float speed = 0.05f;
    private void Update()
    {
        if (canRun)
        {
            tempTime += Time.deltaTime * speed;
            forwardBgImage.fillAmount -= tempTime;
            if (forwardBgImage.fillAmount <= 0)
            {
                canRun = false;
                tempTime = 0;
                forwardBgImage.gameObject.SetActive(false);
                forwardBgImage.sprite = listSprite[backImgIndex];
                forwardBgImage.fillAmount = 1;
            }
        }
    }

    //让loading效果更加圆滑
    //http://blog.csdn.net/huang9012/article/details/38659011
    IEnumerator LoadLevelAsyn(int index)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(index);

        int displayProgress = 0;
        int toProgress = 0;
        op.allowSceneActivation = false;
        //把AsyncOperation.allowSceneActivation设为false就可以禁止Unity加载完毕后自动切换场景
        //但allowSceneActivation设置成false,百分比最多到0.9
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                slider.value = (displayProgress * 0.01f);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        //最后一步,此时场景已经结束
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            slider.value = (displayProgress * 0.01f);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
}
