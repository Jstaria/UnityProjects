using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of our objects with data
/// </summary>
[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}

/// <summary>
/// Holds the Name, ID, Size, and Object data
/// </summary>
[Serializable]
public class ObjectData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public float ActiveRadius { get; private set; }
    [field: SerializeField] public AudioSource PlaceSound { get; private set; }
    [field: SerializeField] public AudioSource RemoveSound { get; private set; }

}
