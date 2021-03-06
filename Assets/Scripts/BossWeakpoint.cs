﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpoint : MonoBehaviour, IDamageable {
    public BossController boss;
    private int hitpoint = 100;
    public int Hitpoint { get; set; }
    public AudioClip impact;

    AudioSource audioSource;
    public GameObject particle;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }


    public int Hit(int damage) {
        int remainingHP = this.hitpoint - damage;
        this.hitpoint = remainingHP;
        audioSource.PlayOneShot(impact, 0.7F);
        if (remainingHP <= 0) {
            this.Dead();
        }
        return remainingHP;
    }

    public void Dead() {
        Debug.Log("Ded");
        boss.RemoveWeakpoint(this);
        GameObject particleGenerated = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(particleGenerated, 1f);
        Destroy(gameObject);

    }
}
