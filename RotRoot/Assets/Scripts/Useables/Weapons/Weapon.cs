using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] protected WeaponStats stats;
    [SerializeField] protected LayerMask hittable;

    protected bool canUse;
    protected bool inAttack;

    public override void SetStats()
    {
        Health = stats.weaponHealth;
        MaxHealth = Health;
    }
}

