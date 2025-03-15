using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject 
{
    [Header("Movement")]
    public float playerWalkSpeed;
    public float playerRunSpeed;
    public float jumpForce;
    public float moveSmoothTime;
    public Vector2 sensitivity;
    public float acceleration;

    [Header("UI")]
    public float interactDistance;

    //public Sprite HoverSprite;
    //public Sprite ClickSprite;
    //public Sprite CursorSprite;
    //public float clickTimer;
}
