using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerCactus : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;
    private Vector2 position;
    private Vector2 velocity;

    public BossSummonerCactus(float actionTime = 1f, Vector2 position, Vector2 velocity)
    {
        this.actionTime = actionTime;
        this.position = position;
        this.velocity = velocity;
    }

    public override void Start()
    {
        GameObject Cactus = Helper.CreateObject("Cactus");
        Cactus.transform.position = this.position;
        Cactus.GetComponent<Rigidbody2D>().velocity = this.velocity;
        Cactus.GetComponent<CactusMissileController>().EventReturn = OnActionTimeEnd;
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
