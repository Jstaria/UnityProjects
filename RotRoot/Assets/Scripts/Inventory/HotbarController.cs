using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private int HotbarSlotCount = 5;
    [SerializeField] private GameObject HotbarSlotPrefab;
    [SerializeField] private Vector2Int HotbarLocation;
    [SerializeField] private int slotSize;
    [SerializeField] private GameObject hotBarSelect;

    private List<SlotUI> hotbarSlots;
    private int currentHotbarNum;

    public delegate void OnHotbarChange(SlotUI slot);
    public event OnHotbarChange OnScroll;

    public void Init()
    {
        hotBarSelect = Instantiate(hotBarSelect, transform);
        RectTransform rect = hotBarSelect.GetComponent<RectTransform>();

        rect.localScale = Vector2.one * (slotSize / rect.rect.width);

        hotbarSlots = new List<SlotUI>();

        for (int i = 0; i < HotbarSlotCount; i++)
        {
            GameObject slot = Instantiate(HotbarSlotPrefab, transform);
            RectTransform rectTransform = slot.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(slotSize, slotSize);
            rectTransform.localPosition = new Vector3(HotbarLocation.x, HotbarLocation.y - i * slotSize, 0);

            SlotUI slotUI = slot.GetComponent<SlotUI>();

            slotUI.Init(rectTransform);

            hotbarSlots.Add(slotUI);
        }
    }

    private void Update()
    {
        foreach (SlotUI slotUI in hotbarSlots)
        {
            slotUI.UpdateSlot();
        }

        bool changedFlag = false;

        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta > 0)
        {
            currentHotbarNum = Mathf.Max(currentHotbarNum - 1, 0);
            changedFlag = true;
        }
        else if (scrollDelta < 0)
        {
            currentHotbarNum = Mathf.Min(currentHotbarNum + 1, HotbarSlotCount - 1);
            changedFlag = true;
        }

        if (changedFlag) 
            Swap();
    }

    public void SetDefault() => Swap();

    private void Swap()
    {
        //Debug.Log("Swapped to: " + currentHotbarNum);
        OnScroll(hotbarSlots[currentHotbarNum]);
        hotBarSelect.GetComponent<RectTransform>().position = hotbarSlots[currentHotbarNum].GetComponent<RectTransform>().position;
    }

    public void SetItem(int slot, Item item)
    {
        item.SetStats();
        hotbarSlots[Mathf.Clamp(slot, 0, HotbarSlotCount - 1)].SetSlotObj(item);
    }
}
