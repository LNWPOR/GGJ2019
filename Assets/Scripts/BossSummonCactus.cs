using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerCactus : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;
    private Vector2 position;
    private float velocity;
    private float angle;

    public BossSummonerCactus(Vector2 position, float velocity, float angle, float actionTime = 1f)
    {
        this.actionTime = actionTime;
        this.position = position;
        this.velocity = velocity;
        this.angle = angle;
    }

    public override void Start()
    {
        string name = "Cactus" + UnityEngine.Random.Range(0, 5).ToString();
        GameObject Cactus = Helper.CreateObject(name);
        Cactus.transform.position = this.position;
        Cactus.GetComponent<CactusMissileController>().Velocity = this.velocity;
        Cactus.GetComponent<CactusMissileController>().Angle = this.angle;
        Cactus.GetComponent<CactusMissileController>().EventReturn = OnActionTimeEnd;
        new WaitForSeconds(3);
        OnActionTimeEnd();
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
