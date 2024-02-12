using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemyData;
}


/// <summary>
/// Holds the Name, ID, Size, and Object data
/// </summary>
[Serializable]
public class EnemyData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
}

