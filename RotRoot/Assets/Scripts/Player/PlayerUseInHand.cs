using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerUseInHand : MonoBehaviour
{
    [SerializeField] private Transform handTransform;

    public Item ItemInHand { get => currentObj != null ? currentObj?.GetComponent<Item>() : null; }
    private GameObject currentObj;
    private SlotUI currentSlotUI;

    public void UseUpdate()
    {
        if (ItemInHand == null) return;

        if (ItemInHand != null)
            ItemInHand.InUse = false;
        if (Input.GetMouseButton(1))
            ItemInHand?.SecondaryUse();
        if (Input.GetMouseButtonDown(0))
            ItemInHand?.PrimaryUse();
    }

    public void SetItemInHand(SlotUI slot)
    {
        if (currentObj != null)
        {
            Destroy(currentObj);
        }

        if (slot != null)
        {
            currentSlotUI = slot;

            if (slot.InventorySlotReference != null)
            {
                string name = slot.InventorySlotReference.name;

                currentObj = Instantiate(slot.InventorySlotReference, handTransform);
                currentObj.name = name;
            }
        }

        

        //if (slot == currentSlotUI) return;
        //// Make this shit work!!
        //string name;
        //if (currentObj != null)
        //    name = currentObj.name;
        //
        //if (currentSlotUI != null && currentObj != null)
        //{
        //    currentSlotUI.SetSlotObj(currentObj.GetComponent<Item>());
        //    Destroy(currentObj);
        //}
        //
        //currentSlotUI = slot;
        //
        //if (slot.InventorySlotReference == null) return;
        //
        //GameObject obj = slot.InventorySlotReference.GetComponent<Item>().gameObject;
        //
        //name = obj?.name;
        //
        //if (obj == null) return;
        //
        //currentObj = Instantiate(obj, handTransform);
        //currentObj.name = name;
        //
        //slot.SetSlotObj(currentObj.GetComponent<Item>());  
    }
}
