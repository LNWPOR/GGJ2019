using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    [SerializeField]
    private float Velocity = 10f;
    [SerializeField]
    private int damage = 100;
    [SerializeField]
    private float targetTime = 3;
    public System.Action EventReturn;
    private Vector3 moveDirection, planetPos, BirdToPlanet, MyPosition, PlayerPos, PlayerToPlanet;
    public AudioClip impact;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetTime -= Time.deltaTime;
        MyPosition = gameObject.transform.position;
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        planetPos = GameManager.GetInstance().GetPlanet().transform.position;

        moveDirection = Vector3.Normalize(PlayerPos - MyPosition);
        BirdToPlanet = Vector3.Normalize(planetPos - MyPosition);
        PlayerToPlanet = Vector3.Normalize(planetPos - PlayerPos);

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(Vector3.SignedAngle(-PlayerToPlanet,-BirdToPlanet, Vector3.forward) > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }

        if (targetTime <= 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDirection.x*Velocity, moveDirection.y*Velocity));
        }
    }

    public void Dead()
    {
        //EventReturn();
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
        else
        {
            //Debug.Log("C");
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-BirdToPlanet.x * 10, -BirdToPlanet.y * 10);
        }   
    }
}
