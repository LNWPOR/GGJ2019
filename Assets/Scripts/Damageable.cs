using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    private int hitpoint = 100;

    public DamageableObject(int hp)
    {
        this.hitpoint = hp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Hit (int damage)
    {
        int remainingHP = hitpoint - damage;
        this.hitpoint = remainingHP;
        if (remainingHP <= 0)
        {
            this.Dead();
        }
        return remainingHP;
    }

    private void Dead ()
    {
        
    }
}


public interface IDamageable
{
    DamageableObject DamageableObjectRef
    {
        get;
        set;
    }
}