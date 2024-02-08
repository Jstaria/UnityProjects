using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);

        newObject.transform.position = position;

        bool foundNull = false;
        int index = placedGameObjects.Count;

        for (int i = 0; i < placedGameObjects.Count; i++)
        {
            if (placedGameObjects[i] != null) continue;

            placedGameObjects[i] = newObject;
            index = i;
            foundNull = true;
            break;
        }

        if (!foundNull) 
        { 
            placedGameObjects.Add(newObject);
        }
        
        return index;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;

        Destroy(placedGameObjects[gameObjectIndex]);

        for (int i = placedGameObjects.Count - 1; i > 0; i--)
        {
            if (placedGameObjects[i] == null)
            {
                placedGameObjects.RemoveAt(i);
            } 
            else { return; }
        }

        if (placedGameObjects.Count == 1) { placedGameObjects.Clear(); return; }
    }
}
