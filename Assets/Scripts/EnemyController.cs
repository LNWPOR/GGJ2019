using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float lifetime = 15f;
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float MinAngularVelocity = 20;
    [SerializeField]
    private float torque = 20f;
    [SerializeField]
    private int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddTorque(torque);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Dead();
        if(gameObject.GetComponent<Rigidbody2D>().angularVelocity < MinAngularVelocity  )
        {
            gameObject.GetComponent<Rigidbody2D>().AddTorque(torque);
        }
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
