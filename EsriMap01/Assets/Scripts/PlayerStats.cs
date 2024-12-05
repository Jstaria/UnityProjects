using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject 
{
    public float playerWalkSpeed;
    public float playerRunSpeed;
    public float jumpForce;
    public float moveSmoothTime;
    public Vector2 sensitivity;
}
