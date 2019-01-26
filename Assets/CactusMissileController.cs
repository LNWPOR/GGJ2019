using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMissileController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float VelocityX = 10;
    [SerializeField]
    private int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(VelocityX, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public int Hit(int damage)
    {
        hitpoint -= damage;
        if (hitpoint <= 0) Dead();
        return hitpoint;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (
            damageable != null &&
            (collision.gameObject.tag == "Player")
            )
        {
            damageable.Hit(this.damage);
        }
    }
}
