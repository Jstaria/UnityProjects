using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public bool IsAlive { get; set; }
    public bool IsAttackable { get; set; }
    public void TakeDamage(int damage);
}
