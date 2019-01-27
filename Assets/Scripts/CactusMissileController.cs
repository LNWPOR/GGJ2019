using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMissileController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    public float Velocity = 10;
    public float Angle = 180;
    [SerializeField]
    private int damage = 100;
    [SerializeField]
    private float LifeTime = 3;
    public System.Action EventReturn;
    private Vector3 moveDirection, planetPos, before, MyPosToPlanet, MyPosition, PlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) Dead();

        MyPosition = gameObject.transform.position;
        planetPos = GameManager.GetInstance().GetPlanet().transform.position;
        MyPosToPlanet = Vector3.Normalize(planetPos - MyPosition);

        float angle = Mathf.Atan2(MyPosToPlanet.y, MyPosToPlanet.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle+180, Vector3.forward);

        transform.RotateAround(planetPos, new Vector3(0,0,1), Velocity * Time.deltaTime);
    }

    public void Dead()
    {
        //EventReturn();
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
