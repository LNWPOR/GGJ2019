using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerBird : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;
    private Vector2 position;

    public BossSummonerBird(float actionTime = 1f, Vector2 position)
    {
        this.actionTime = actionTime;
        this.position = position;
    }

    public override void Start()
    {
        GameObject Bird = Helper.CreateObject("Bird");
        Bird.transform.position = this.position;
        Bird.GetComponent<BirdController>().EventReturn = OnActionTimeEnd;
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
