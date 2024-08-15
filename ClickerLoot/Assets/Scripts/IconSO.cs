using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class IconSO : ScriptableObject
{
    public List<Icon> icons;

    public Icon GetIcon(int id)
    {
        Icon icon = icons[0];
        foreach (var item in icons)
        {
            if (item.id == id) icon = item;
        }

        return icon;
    }
}

[Serializable]
public class Icon
{
    public int id;
    public Texture2D icon;
}
