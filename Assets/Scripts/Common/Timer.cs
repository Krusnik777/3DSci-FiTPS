using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject timerCollector;

    public bool IsLoop;

    public event UnityAction EventOnTimeRanOut;
    public event UnityAction EventOnTick;

    private float maxTime;
    public float MaxTime => maxTime;
    private float currentTime;
    public float CurrentTime => currentTime;
    private bool isPaused;
    public bool IsPaused => isPaused;
    public bool IsCompleted => currentTime <= 0;

    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (timerCollector == null) timerCollector = new GameObject("Timers");

        Timer timer = timerCollector.AddComponent<Timer>();

        timer.maxTime = time;
        timer.IsLoop = isLoop;

        return timer;
    }

    public static Timer CreateTimer(float time)
    {
        if (timerCollector == null) timerCollector = new GameObject("Timers");

        Timer timer = timerCollector.AddComponent<Timer>();

        timer.maxTime = time;

        return timer;
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Complete()
    {
        isPaused = false;

        currentTime = 0;
    }

    public void CompleteWithoutEvent()
    {
        Destroy(this);
    }

    public void Restart(float time)
    {
        maxTime = time;
        currentTime = maxTime;
    }

    public void Restart()
    {
        currentTime = maxTime;
    }

    private void Update()
    {
        if (isPaused) return;

        currentTime -= Time.deltaTime;

        EventOnTick?.Invoke();

        if (currentTime <= 0)
        {
            currentTime = 0;

            EventOnTimeRanOut?.Invoke();

            if (IsLoop) currentTime = maxTime;
        }
    }
}
