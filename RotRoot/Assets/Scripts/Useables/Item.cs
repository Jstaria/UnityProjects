using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Item : MonoBehaviour, IUseable
{
    [SerializeField] private new string name;

    public virtual float Health { get { return health; } set => health = value; }
    public virtual float MaxHealth { get { return maxHealth; } set => maxHealth = value; }
    public virtual bool InUse { get; set; }
    public virtual bool LockRotation { get; set; }

    protected float health;
    protected float maxHealth;

    public virtual void SetStats() { }

    public virtual void OnBreak() => Destroy(gameObject);

    public virtual void PrimaryUse() {}

    public virtual void SecondaryUse() {}
}
