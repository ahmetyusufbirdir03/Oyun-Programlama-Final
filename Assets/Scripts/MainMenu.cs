using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Timer timer;
    
    public Slider ses_slider;

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
}
