using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonerEnemy : ActionBased
{
    private readonly float actionTime;
    public GameObject prefab;
    private Vector2 position;
    private float torque;

    public BossSummonerEnemy(Vector2 position, float torque, float actionTime = 1f)
    {
        this.actionTime = actionTime;
        this.position = position;
        this.torque = torque;
    }

    public override void Start()
    {
        GameObject Enemy = Helper.CreateObject("Enemy");
        Enemy.transform.position = this.position;
        Enemy.GetComponent<EnemyController>().torque = this.torque;
        Enemy.GetComponent<EnemyController>().EventReturn = OnActionTimeEnd;
        new WaitForSeconds(3);
        OnActionTimeEnd();
    }

    public void OnActionTimeEnd()
    {
        InvokeEndEvent();
    }
}
