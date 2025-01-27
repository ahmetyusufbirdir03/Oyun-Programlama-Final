using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Timer timer;  
    public Slider ses_slider;

    public GameObject[] menues;

    private void Update()
    {
        UpdateVolume();
    }

    public void Button_LoadLevel()
    {
        PlayerPrefs.SetInt("Life", 3);
        timer.StartTimer();
        SceneManager.LoadScene(1);
    }

    private void UpdateVolume()
    {  
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource item in audioSources)
        {
            item.volume = ses_slider.value;
        }
    }

    public void OpenAudioMenu()
    {
        OpenMenu(menues[1]);   
    }

    public void OpenControlMenu()
    {
        OpenMenu(menues[2]);
    }

    public void Button_Back()
    {
        OpenMenu(menues[0]);
    }



    public void OpenMenu(GameObject menu)
    {
        for(int i=0; i < menues.Length; i++)
        {
            if(menues[i] == menu)
            {
                menues[i].SetActive(true);
            }
            else
            {
                menues[i].SetActive(false);
            }
        }
    }
}
