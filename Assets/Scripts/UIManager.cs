using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioMenu;

    [Header("Music Slider")]
    [SerializeField] private GameObject[] barImg;
    [SerializeField] private GameObject auidoOnButton;
    [SerializeField] private GameObject auidoOffButton;



    private void Start()
    {
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetInt("Volume", 3);
        }   
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        for (int i = 0; i < barImg.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("Volume"))
            {
                barImg[i].SetActive(true);
            }
            else
            {
                barImg[i].SetActive(false);
            }
        }
    }

    public void Button_IncreaseVolume()
    {
        if(PlayerPrefs.GetInt("Volume") < 5)
        {
            PlayerPrefs.SetInt("Volume", PlayerPrefs.GetInt("Volume") + 1);
        }

    }

    public void Button_DecreaseVolume()
    {
        if (PlayerPrefs.GetInt("Volume") > 0)
        {
            PlayerPrefs.SetInt("Volume", PlayerPrefs.GetInt("Volume") - 1);
        }
    }

    public void Button_AudioOff()
    {
        PlayerPrefs.SetInt("Volume",0);
        auidoOffButton.SetActive(false);
        auidoOnButton.SetActive(true);
    }

    public void Button_AudioOn()
    {
        PlayerPrefs.SetInt("Volume", 3);
        auidoOffButton.SetActive(true);
        auidoOnButton.SetActive(false);
    }


    public void Button_Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Button_Audio()
    {
        pauseMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void Button_BackMenu()
    {
        pauseMenu.SetActive(true);
        audioMenu.SetActive(false);
    }



    public void Buttonn_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Button_MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
