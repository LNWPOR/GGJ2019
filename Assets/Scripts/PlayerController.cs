using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private DamageableObject damageableObject;
    public DamageableObject DamageableObjectRef
    {
        get
        {
            return this.damageableObject;
        }
        set
        {
            this.damageableObject = value;
        }
    }

   // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
