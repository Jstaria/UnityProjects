using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<MyData> randomNumber = new NetworkVariable<MyData>(
        new MyData
        {
            _int = 0,
            _bool = true,
            positions = new Vector3[10]
        },
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private Transform spawnObject;

    private Transform spawnedObject;

    [SerializeField] private Transform camera;
    [SerializeField] private Rigidbody rb;

    struct MyData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public Vector3[] positions;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref positions);
        }
    }

    public override void OnNetworkSpawn()
    {
        camera.gameObject.SetActive(IsOwner);

        randomNumber.OnValueChanged += (MyData previousValue, MyData newValue) =>
        {
            Debug.Log(OwnerClientId + "; Random number: " + newValue._int);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 vel = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.T))
        {
            //MyData data = randomNumber.Value;

            //data._int = Random.Range(0, 1000);

            //randomNumber.Value = data;

            //TestServerRPC();

            //TestClientRpc();

            spawnedObject = Instantiate(spawnObject);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObject.gameObject);
        }


            if (Input.GetKey(KeyCode.W)) vel.z = 1f;
        if (Input.GetKey(KeyCode.S)) vel.z = -1f;
        if (Input.GetKey(KeyCode.A)) vel.x = -1f;
        if (Input.GetKey(KeyCode.D)) vel.x = 1f;

        float speed = 10;

        rb.velocity += vel.normalized * speed * Time.deltaTime;
    }

    // Clients can use to send to server
    [ServerRpc]
    private void TestServerRPC()
    {
        Debug.Log("TestServerRPC" + OwnerClientId);
    }

    // Can use this to send to only certain clients using the ClientRPC data object
    // Server can use to send to clients
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRPC");
    }
}
