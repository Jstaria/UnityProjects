using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private HotbarController hotbarController;

    [SerializeField] private Item axe;

    void Start()
    {
        hotbarController.Init();

        hotbarController.SetItem(0, axe);
        hotbarController.OnScroll += controller.PlayerHand.SetItemInHand;

        hotbarController.SetDefault();
    }

    void Update()
    {
        
    }
}
