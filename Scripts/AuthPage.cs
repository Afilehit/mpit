using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class AuthPage : MonoBehaviour
{
    public GameObject Login;
    public GameObject Register;
    public Text SwitchText;

    public void Switch(){
        if(Login.active)
        {
            Login.SetActive(false);
            Register.SetActive(true);
            SwitchText.text = "Войти";
        }
        else {
            Login.SetActive(true);
            Register.SetActive(false);
            SwitchText.text = "Регистрация";
        }
    }
}
