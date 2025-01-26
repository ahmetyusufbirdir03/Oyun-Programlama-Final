using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Timer timer;

    public void Button_LoadLevel()
    {
        PlayerPrefs.SetInt("Life", 3);
        timer.StartTimer();
        SceneManager.LoadScene(1);
    }
}
