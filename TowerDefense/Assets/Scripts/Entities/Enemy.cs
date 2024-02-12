using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    protected override void CalcSteeringForce()
    {
        
    }

    public void SetForce(Vector3 force)
    {
        phyObj.ApplyForce(force);
    }
}
