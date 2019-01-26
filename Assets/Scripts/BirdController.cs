using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float Velocity = 1f;
    [SerializeField]
    private int damage = 100;
    [SerializeField]
    private float InitiateTime = 3;
    private float targetTime = 3;
    [SerializeField]
    private float RotationSpeed = 100;
    public System.Action EventReturn;
    Vector3 moveDirection, planetPos,before, planetToBird;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetTime -= Time.deltaTime;
        moveDirection = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position);
        planetPos = GameManager.GetInstance().planet.transform.position;
        planetToBird = Vector3.Normalize(planetPos - gameObject.transform.position);
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (targetTime <= 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDirection.x*Velocity, moveDirection.y*Velocity));
        }
        if(Mathf.Abs(Vector3.Distance(planetPos, gameObject.transform.position) - Vector3.Distance(planetPos, before)) <=0.1)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-planetToBird.x*Velocity, -planetToBird.y*Velocity));
        }
        before = gameObject.transform.position;
    }

    public void Dead()
    {
        EventReturn();
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
