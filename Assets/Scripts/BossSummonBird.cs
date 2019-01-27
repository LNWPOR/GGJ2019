using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerBird : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;
    private Vector2 position;

    public BossSummonerBird( Vector2 position, float actionTime = 1f)
    {
        this.actionTime = actionTime;
        this.position = position;
    }

    public override void Start()
    {
        GameObject Bird = Helper.CreateObject("Bird");
        Bird.transform.position = this.position;
        Bird.GetComponent<BirdController>().EventReturn = OnActionTimeEnd;
        new WaitForSeconds(3);
        OnActionTimeEnd();
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
