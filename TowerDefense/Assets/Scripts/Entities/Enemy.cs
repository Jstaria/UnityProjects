using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    private int health;

    public int ID { get; private set; }
    public string Name { get; private set; }
    public int MaxHealth { get; private set; }
    public float Speed { get; private set; }

    public void Create(int ID, string Name, int MaxHealth)
    {

    }

    protected override void CalcSteeringForce()
    {
        
    }

    public void SetForce(Vector3 force)
    {
        //phyObj.ApplyForce(force);
        phyObj.Velocity = Vector3.Slerp(phyObj.Velocity, force, .01f);
    }
}
