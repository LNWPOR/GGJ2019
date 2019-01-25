using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeapon : Attacher
{
    public int damage = 20;
    public double speed = 10;

    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.Detach();
        }
    }

    private void Shoot()
    {

    }


}
