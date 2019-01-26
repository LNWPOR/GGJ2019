using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerWithCallback : MonoBehaviour
{
    private System.Action listener;
    private float time;
    public void SetAndStartTimerWithCallback(System.Action action, float time)
    {
        listener = action;
        this.time = time;
        StartCoroutine(DoTimer());
    }

    private void OnTimerEnd()
    {
        listener?.Invoke();
    }

    public IEnumerator DoTimer()
    {
        yield return new WaitForSeconds(time);
        OnTimerEnd();
    }
}
