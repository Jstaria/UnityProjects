using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySingleton : Singleton<InventorySingleton>
{
    [SerializeField] private GameObject newInventory;
    [SerializeField] private IconSO icons;
    [SerializeField] private GameObject contentUI;

    public List<GameObject> newlySpawnedItems;
    public List<GameObject> spawnedIcons;

    public bool isInvOpen;
    public void ShowNewItems(List<Item> items)
    {
        isInvOpen = true;
        newInventory.SetActive(true);

        items.Sort();
        items.Reverse();

        for (int i = 0; i < items.Count; i++)
        {
            //
            //Instantiate(icons.GetIcon(items[i].id).icon;

            GameObject newObj = new GameObject(string.Format("{0}", items[i].id), typeof(RawImage));
            newObj.GetComponent<RawImage>().texture = icons.GetIcon(items[i].id).icon;
            spawnedIcons.Add(Instantiate(newObj, contentUI.transform.position, Quaternion.identity));
            spawnedIcons[i].transform.SetParent(contentUI.transform, false);
        }
    }

    public void HideInventory()
    {
        newInventory.SetActive(false);
        isInvOpen = false;

        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            GameObject.Destroy(spawnedIcons[i]);
        }

        spawnedIcons.Clear();
    }
}
