﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public float gravity = 9.81f;

    void Start()
    {
        //  Disable global gravity
        Physics2D.gravity = new Vector2(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        MonoBehaviour[] gameobjectList = GameObject.FindObjectsOfType<MonoBehaviour>();
        
        foreach(MonoBehaviour gameObject in gameobjectList)
        {
            Rigidbody2D targetGameObjRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            //  Check if target has Rigidbody2D then apply gravity
            if( targetGameObjRigidbody2D != null 
                && gameObject.GetComponent<BirdController>() == null 
                && gameObject.GetComponent<CactusMissileController>() == null)
            {
                Vector2 objToPlanetDir = (this.transform.position - gameObject.transform.position);
                Rigidbody2D targetRigidBody = gameObject.GetComponent<Rigidbody2D>();

                float mass = targetRigidBody.mass;
                targetRigidBody.AddForce(objToPlanetDir.normalized * gravity * targetRigidBody.mass);
            }
        }
    }
}
