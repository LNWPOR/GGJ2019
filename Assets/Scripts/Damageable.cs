using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable
{
    int Hitpoint { get; set; }
    int Hit(int damage);
    void Dead();
}