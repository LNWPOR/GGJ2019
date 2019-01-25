using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLET_TYPE
{
    PLAYER,
    ENEMY
}

public class Bullet : MonoBehaviour
{
    public int damage = 100;
    public BULLET_TYPE bulletType;

    public Bullet(int damage, BULLET_TYPE bulletType)
    {
        this.damage = damage;
        this.bulletType = bulletType;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            damageable != null &&
            (collision.gameObject.tag == "Player" && this.bulletType == BULLET_TYPE.ENEMY) ||
            (collision.gameObject.tag == "Enemy" && this.bulletType == BULLET_TYPE.PLAYER)
        ) {
            damageable.Hit(this.damage);
        }
    }
}
