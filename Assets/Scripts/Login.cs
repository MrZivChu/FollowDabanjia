using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class Login : MonoBehaviour
{

    public InputField phone;
    public InputField pwd;
    public Toggle isRememberPwdTog;
    public Text forgetPwd;
    public Text register;

    public Button loginBtn;


    private void Start()
    {
        EventTriggerListener.Get(loginBtn.gameObject).onClick = StartLogin;
    }

    void StartLogin(GameObject go, object param)
    {
        string p1 = phone.text;
        string p2 = pwd.text;
        Loading.index = 2;
        SceneManager.LoadScene("Loading");
    }
}
