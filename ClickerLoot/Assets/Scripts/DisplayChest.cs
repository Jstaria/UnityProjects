using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayChest : MonoBehaviour
{
    public int chestID;

    public ShelfManager shelfManager;

    public GameObject nonDisplayChest;

    public HitBox HitBox;

    public void OnClick()
    {
        shelfManager.selectedID = chestID;
        shelfManager.selectedChest = nonDisplayChest;
    }
}
