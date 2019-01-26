using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerBird : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;

    public BossSummonerBird(float actionTime = 1f)
    {
        this.actionTime = actionTime;
    }

    public override void Start()
    {
        GameObject Bird = Helper.CreateObject("Bird");
        Bird.transform.position = new Vector2(0.3f, 4);
        Bird.GetComponent<BirdController>().EventReturn = OnActionTimeEnd;
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
