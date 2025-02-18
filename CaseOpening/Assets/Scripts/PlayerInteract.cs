using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CursorStatus
{
    Hovering,
    Clicked,
    None
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask interactable;

    [SerializeField] private Image cursor;

    private float clickTimer;

    public CursorStatus CursorStatus {  get; private set; }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;

        if (clickTimer < 0)
            CursorStatus = CursorStatus.None;

        if (Physics.Raycast(ray, out hit, stats.interactDistance, interactable))
        {
            if (clickTimer < 0)
                CursorStatus = CursorStatus.Hovering;

            if (Input.GetMouseButtonDown(0))
            {
                clickTimer = stats.clickTimer;

                CursorStatus = CursorStatus.Clicked;

                Transform transform = hit.transform;

                Interactable interactable = transform.GetComponentInChildren<Interactable>();

                interactable.InteractWith();
            }
        }

        clickTimer -= Time.deltaTime;

        switch(CursorStatus)
        {
            case CursorStatus.Hovering:
                cursor.sprite = stats.HoverSprite;
                break;

            case CursorStatus.Clicked:
                cursor.sprite = stats.ClickSprite;
                break;

            case CursorStatus.None:
                cursor.sprite = stats.CursorSprite;
                break;
        }     
    }
}
