﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Attacher
{

    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        //gameObject.GetComponent<Rigidbody2D>().simulated = true;
    }


}
