using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityStats : ScriptableObject
{
    public int health;
    public int moveSpeed;
    public int maxHealth;
    public int attackDamage;

    public void CopyFrom(EntityStats stats)
    {
        health = stats.health;
        moveSpeed = stats.moveSpeed;
        maxHealth = stats.maxHealth;
        attackDamage = stats.attackDamage;
    }
}
