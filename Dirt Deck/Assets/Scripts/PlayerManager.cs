using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Transform> spawnPositions; 
    public Dictionary<uint, GameObject> player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
