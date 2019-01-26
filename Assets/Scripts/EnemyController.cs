using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 15f;
    [SerializeField]
    private int healthPoint = 1;
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
        if (lifetime == 0) EnemyDie();
        if(gameObject.GetComponent<Rigidbody2D>().angularVelocity < MinAngularVelocity  )
        {
            gameObject.GetComponent<Rigidbody2D>().AddTorque(torque);
        }
    }

    private void EnemyDie()
    {
        Destroy(gameObject);
    }

    public void Damage()
    {
        healthPoint -= 1;
        if (healthPoint == 0) EnemyDie();
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
