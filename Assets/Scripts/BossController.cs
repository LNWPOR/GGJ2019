using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AttachableObject
{
    public Dictionary<GameObject, int> weakpoints = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveWeakpoint(GameObject weakpoint)
    {
        weakpoints.Remove(weakpoint);
        if (weakpoints.Count == 0)
        {
            this.Dead();
        }
    }

    private void Dead()
    {
        // TODO: Handle dead
    }
}
