using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    private float _elapsedTime = 0.0f;
    private bool _isRunning = false;

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        _elapsedTime = 0.0f;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _elapsedTime = 0.0f;
        _isRunning = false;
        UpdateTimerUI();
    }

    public float GetElapsedTime()
    {
        return _elapsedTime;
    }
    
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = GetFormattedTime(_elapsedTime);
        }
    }
    
    private string GetFormattedTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        return timeSpan.ToString(@"mm\:ss");
    }
}