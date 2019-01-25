using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 15f;
    private int healthPoint = 1;
    private const float MinAngularVelocity = 30;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddTorque(60);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime == 0) EnemyDie();
        if(gameObject.GetComponent<Rigidbody2D>().angularVelocity < MinAngularVelocity  )
        {
            gameObject.GetComponent<Rigidbody2D>().AddTorque(60);
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
}
