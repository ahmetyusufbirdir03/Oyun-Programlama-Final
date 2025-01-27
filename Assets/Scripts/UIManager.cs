using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // AudioMixer için gerekli

public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject endGameMenu;

    [Header("Music Slider")]
    [SerializeField] private GameObject[] barImg;
    [SerializeField] private GameObject auidoOnButton;
    [SerializeField] private GameObject auidoOffButton;

    [Header("End Game UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject star1,star2,star3;
    [SerializeField] private GameObject menuButton;


    Timer timer;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetInt("Volume", 3);
        }

        UpdateVolume(); // Başlangıçta sesi uygula

        timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                if (audioMenu.activeInHierarchy)
                {
                    audioMenu.SetActive(false);
                }

                pauseMenu.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0;
            }
        
            else
            {
                pauseMenu.SetActive(false);
                audioMenu.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

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
            UpdateVolume(); // Yeni ses seviyesini uygula
        }

    }

    public void Button_DecreaseVolume()
    {
        if (PlayerPrefs.GetInt("Volume") > 0)
        {
            PlayerPrefs.SetInt("Volume", PlayerPrefs.GetInt("Volume") - 1);
            UpdateVolume(); // Yeni ses seviyesini uygula
        }
    }

    public void Button_AudioOff()
    {
        PlayerPrefs.SetInt("Volume",0);
        UpdateVolume();
        auidoOffButton.SetActive(false);
        auidoOnButton.SetActive(true);
    }

    public void Button_AudioOn()
    {
        PlayerPrefs.SetInt("Volume", 3);
        UpdateVolume();
        auidoOffButton.SetActive(true);
        auidoOnButton.SetActive(false);
    }

    private void UpdateVolume()
    {  
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource item in audioSources)
        {
            item.volume = PlayerPrefs.GetInt("Volume") / 5f;
        }
    }

    public void Button_Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenu.SetActive(true);
        audioMenu.SetActive(false);
    }

    public void Buttonn_Restart()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerPrefs.SetInt("Life", 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Button_MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }


    public void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endGameMenu.SetActive(true);
        timer.StopTimer();

        int minutes = Mathf.FloorToInt(timer.GetTimerValue() / 60f); // Dakika hesab�
        int seconds = Mathf.FloorToInt(timer.GetTimerValue() % 60f); // Saniye hesab�

        string timePast = $"{minutes:D2}:{seconds:D2}";
        timerText.text = timePast;

        StartCoroutine(HandleStars(timer.GetTimerValue()));
    }

    private IEnumerator HandleStars(float time)
    {
        // Y�ld�zlara s�reye ba�l� olarak karar ver
        if (timer.GetTimerValue() > 300)
        {
            star1.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Yar�m saniye bekle
        }
        else if (timer.GetTimerValue() > 180)
        {
            star1.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Yar�m saniye bekle
            star2.SetActive(true);
        }
        else
        {
            star1.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Yar�m saniye bekle
            star2.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Yar�m saniye bekle
            star3.SetActive(true);
        }

        // Y�ld�zlar�n a��lma animasyonu burada bitebilir
        yield return new WaitForSeconds(0.5f);
        menuButton.SetActive(true);

    }

}
