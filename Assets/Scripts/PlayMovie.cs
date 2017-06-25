using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PlayType {
    none = 0,
    image = 1,
    mesh = 2
}

public enum PlayStatus {
    none = 0,
    playing = 1,
    stop = 2,
    pause = 3
}

public class PlayMovie : MonoBehaviour {
    public MovieTexture movieTexture;
    public PlayType playType = PlayType.none;
    public bool isStartPlay = false;//是否程序运行就开始播放
    public bool loop = true;

    public Image imgae = null;
    public Renderer theRenderer = null;

    [HideInInspector]
    public PlayStatus playStatus = PlayStatus.none;

    //提供接口用于当关闭视频的时候是否关闭mesh
    public bool isCloseMesh = false;

    Texture imageOrignalTexture;
    Texture meshOrignalTexture;

    void Start() {
        if (movieTexture != null) {
            if (playType == PlayType.image) {
                if (imgae != null) {
                    imageOrignalTexture = imgae.material.mainTexture;
                    imgae.material.mainTexture = movieTexture;
                }
            } else if (playType == PlayType.mesh) {
                if (theRenderer != null) {
                    meshOrignalTexture = theRenderer.material.mainTexture;
                    theRenderer.material.mainTexture = movieTexture;
                }
            }
            movieTexture.loop = loop;
            if (isStartPlay) {
                Play();
            } else {
                Pause();
            }
        }
    }

    public void Play() {
        if (movieTexture != null) {
            movieTexture.Play();
            playStatus = PlayStatus.playing;
            if (theRenderer) {
                theRenderer.enabled = true;
            }
        }
    }

    public void Stop() {
        if (movieTexture != null) {
            movieTexture.Stop();
            playStatus = PlayStatus.stop;
        }
    }

    public void Pause() {
        if (movieTexture != null) {
            movieTexture.Pause();
            playStatus = PlayStatus.pause;
            if (isCloseMesh == true) {
                if (theRenderer) {
                    theRenderer.enabled = false;
                }
            }
        }
    }


    private void OnDestroy() {
        if (imgae != null) {
            imgae.material.mainTexture = imageOrignalTexture;
        }
        if (theRenderer != null) {
            theRenderer.material.mainTexture = meshOrignalTexture;
        }
    }
}
