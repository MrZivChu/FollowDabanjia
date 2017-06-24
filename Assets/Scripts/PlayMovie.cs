using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayMovie : MonoBehaviour
{
    public MovieTexture movTexture;
    Image imgae = null;
    Renderer theRenderer = null;

    Texture orignalTexture1;
    Texture orignalTexture2;

    void Start()
    {
        imgae = GetComponent<Image>();
        if (imgae != null)
        {
            orignalTexture1 = imgae.material.mainTexture;
            imgae.material.mainTexture = movTexture;
        }
        else
        {
            theRenderer = GetComponent<Renderer>();
            if (theRenderer != null)
            {
                orignalTexture2 = theRenderer.material.mainTexture;
                theRenderer.material.mainTexture = movTexture;
            }
        }
        movTexture.loop = true;
        movTexture.Play();
    }

    private void OnDestroy()
    {
        if (imgae != null)
        {
            imgae.material.mainTexture = orignalTexture1;
        }
        if (theRenderer != null)
        {
            theRenderer.material.mainTexture = orignalTexture2;
        }
    }
}
