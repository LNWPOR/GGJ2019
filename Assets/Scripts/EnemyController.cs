using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int lifetime = 1500;
    private int healthPoint = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddTorque(new Vector3(0,0,60));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= 1;
        if (lifetime == 0) EnemyDie();
    }

    private void EnemyDie()
    {
        Destroy(this.gameObject);
    }

    public void Hurt()
    {
        healthPoint -= 1;
        if (healthPoint == 0) EnemyDie();
    }
}
