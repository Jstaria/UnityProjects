using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBoxes : MonoBehaviour
{
    public GameObject boxPrefab;
    public int numBoxes = 100;

    private List<GameObject> boxes = new List<GameObject>();
    public Transform left;
    public Transform right;
    public Transform top;
    public Transform bottom;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numBoxes; i++)
        {
            Vector3 pos = new Vector3(Random.Range(left.position.x, right.position.x), 4, Random.Range(bottom.position.z, top.position.z));

            boxes.Add(Instantiate(boxPrefab, pos, Quaternion.Euler(0, Random.Range(0, 180), 0), transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
