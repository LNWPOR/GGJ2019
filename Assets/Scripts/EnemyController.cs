using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float lifetime = 15f;
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float MinAngularVelocity = 20;
    [SerializeField]
    public float torque = 50f;
    [SerializeField]
    private int damage = 100;
    public AudioClip impact;
    public System.Action EventReturn;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddTorque(torque);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lifetime -= Time.deltaTime;
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
        audioSource.PlayOneShot(impact, 0.7F);
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
            this.Dead();
        }
    }
}
