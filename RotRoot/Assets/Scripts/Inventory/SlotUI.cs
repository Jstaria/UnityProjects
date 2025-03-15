using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP;
    [SerializeField] private GameObject healthIndicator;

    public GameObject InventorySlotReference { get; private set; }

    protected RectTransform rectTransform;
    protected float maxHeight;

    public void Init(RectTransform rectTransform)
    {
        maxHeight = rectTransform.rect.height;
        this.rectTransform = rectTransform;

        healthIndicator = Instantiate(healthIndicator, transform);
        
        RectTransform healthRT = healthIndicator.GetComponent<RectTransform>();

        healthRT.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
        healthRT.position = rectTransform.position;
    }

    public void UpdateSlot()
    {
        if (InventorySlotReference == null) return;

        Item item = InventorySlotReference.GetComponent<Item>();

        float health = item.Health;
        float maxHealth = item.MaxHealth;

        healthIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.sizeDelta.x, maxHeight - maxHeight / maxHealth * health);
    }

    public void SetUIText(string text) => TMP.text = text;
    public void SetSlotObj(Item obj)
    {
        InventorySlotReference = obj.gameObject;
        SetUIText(obj.name);
    }
}
