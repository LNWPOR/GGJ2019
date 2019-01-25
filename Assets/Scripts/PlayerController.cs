﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AttachableObject, IDamageable
{
    private int hitpoint = 100;
    public int Hitpoint { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Hit(int damage)
    {
        int remainingHP = this.hitpoint - damage;
        this.hitpoint = remainingHP;
        if (remainingHP <= 0)
        {
            this.Dead();
        }
        return remainingHP;
    }

    public void Dead()
    {
        // Handle dead
    }
}
