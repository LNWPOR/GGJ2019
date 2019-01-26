using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerCactus : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;

    public BossSummonerCactus(float actionTime = 1f)
    {
        this.actionTime = actionTime;
    }

    public override void Start()
    {
        GameObject Cactus = Helper.CreateObject("Cactus");
        Cactus.transform.position = new Vector2(-10, 0);
        Cactus.GetComponent<Rigidbody2D>().velocity = new Vector2(3, 0);
        Cactus.GetComponent<CactusMissileController>().EventReturn = OnActionTimeEnd;
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
