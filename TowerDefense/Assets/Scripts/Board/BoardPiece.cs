using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece
{
    public GameObject slotRef { get; private set; }
    public bool IsActive { get; private set; }
    public Vector2 Position { get; set; }

    public void SetObject(GameObject obj)
    {
        slotRef = obj;
        IsActive = true;
    }
}
