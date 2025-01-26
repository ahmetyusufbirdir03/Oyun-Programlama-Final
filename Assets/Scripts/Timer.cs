using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private float timerValue;
    private bool isRunning = false;

    private void Awake()
    {
        // E�er bu s�n�ftan ba�ka bir �rnek varsa, yenisini yok et.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Singleton instance atamas�
        Instance = this;

        // Sahne de�i�iminde yok olmas�n� engelle
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isRunning)
        {
            timerValue += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timerValue = 0f;
    }

    public float GetTimerValue()
    {
        return timerValue;
    }
}
