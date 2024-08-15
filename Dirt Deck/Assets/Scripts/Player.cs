using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<uint> playerID = new NetworkVariable<uint>();

    public override void OnNetworkSpawn()
    {
        PlayerManager.Instance.player.Add(playerID.Value, this.gameObject);
        transform.position = PlayerManager.Instance.spawnPositions[PlayerManager.Instance.player.Count - 1].position;
    }
}
