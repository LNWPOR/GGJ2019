using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AttachableObject
{
    public List<BossWeakpoint> weakpoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveWeakpoint(BossWeakpoint weakpoint)
    {
        weakpoints.Remove(weakpoint);
        if (weakpoints.Count == 0)
        {
            this.Dead();
        }
    }

    private void Dead()
    {
        GameManager.GetInstance().DoBossDead();
    }
}
