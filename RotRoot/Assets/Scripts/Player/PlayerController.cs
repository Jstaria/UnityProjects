using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(PlayerLook))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerUseInHand playerUse;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerStats playerStats;

    public PlayerUseInHand PlayerHand { get => playerUse; }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        playerMovement.MovementUpdate();
    }

    void Update()
    {
        UpdatePlayerLook();
        playerUse.UseUpdate();
    }

    private void UpdatePlayerLook()
    {
        Vector2 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition / 4);
        Vector2 direction = playerLook.LastDirection;

        bool LockRotationFlag = playerUse.ItemInHand != null && playerUse.ItemInHand.LockRotation;

        if (!LockRotationFlag)
        {
            if (playerUse.ItemInHand != null && playerUse.ItemInHand.InUse)
                direction = mousePos - (Vector2)transform.position;
            else if (playerMovement.CurrentState != MovementState.Idle)
                direction = playerMovement.Direction;
        }

        playerLook.LookUpdate(direction);
    }
}
