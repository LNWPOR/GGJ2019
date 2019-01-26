using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : ActionBased
{
    private readonly float actionTime;

    public TestAction(float actionTime = 1f)
    {
        this.actionTime = actionTime;
    }

    public override void Start()
    {
        Debug.Log("TestAction in coming..");
        GameObject timer = Helper.CreateObject("Timer");
        timer.GetComponent<TimerWithCallback>().SetAndStartTimerWithCallback(OnActionTimeEnd, actionTime);
    }

    private void OnActionTimeEnd()
    {
        Debug.Log("Kaboom!!!");
        InvokeEndEvent();
    }
}
