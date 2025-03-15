using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour, IAttackable
{
    [SerializeField] private EntityStats defaultStats;
    protected EntityStats stats;

    public bool IsAlive { get; set; }
    public bool IsAttackable { get; set; }

    private void Start()
    {
        stats = EntityStats.CreateInstance<EntityStats>();
        stats.CopyFrom(defaultStats);
    }

    public virtual void TakeDamage(int damage)
    {
        stats.health -= damage;

        IsAlive = stats.health > 0;

        if (!IsAlive) OnDeath();
    }

    protected virtual void OnDeath() => gameObject.SetActive(false);
}
