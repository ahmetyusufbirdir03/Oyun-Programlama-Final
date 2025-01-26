using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private float timerValue;
    private bool isRunning = false;

    private void Awake()
    {
        // Eðer bu sýnýftan baþka bir örnek varsa, yenisini yok et.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Singleton instance atamasý
        Instance = this;

        // Sahne deðiþiminde yok olmasýný engelle
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
