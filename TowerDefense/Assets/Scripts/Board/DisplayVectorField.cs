using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DisplayVectorField : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private GridVectorField grid;

    List<GameObject> list = new();

    public void Display()
    {
        foreach (KeyValuePair<Vector3Int, Vector3Int> field in grid.FieldPositionValues)
        {
            list.Add(Instantiate(pointer, new Vector3(field.Key.x, 0, field.Key.z) + new Vector3(.5f, 0, .5f), Quaternion.identity, transform));

            GameObject texture = list[list.Count - 1];
            float rotationAngle = 0;

            texture.transform.localScale = new Vector3(texture.transform.localScale.x, Mathf.Abs(texture.transform.localScale.y), texture.transform.localScale.z);
            Vector3Int lookDirection = field.Value;

            if (lookDirection.x == -1) { rotationAngle = 0; }
            if (lookDirection.x == 1) { rotationAngle = 180; texture.transform.localScale = new Vector3(texture.transform.localScale.x, -Mathf.Abs(texture.transform.localScale.y), texture.transform.localScale.z); }
            if (lookDirection.z == -1) { rotationAngle = 90; }
            if (lookDirection.z == 1) { rotationAngle = -90; }

            texture.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
            //texture.transform.position = lookDirection + transform.position;
        }
    }
}

