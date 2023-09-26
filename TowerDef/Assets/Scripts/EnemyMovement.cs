using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = .5f;
    private int count;

    internal bool IsFinished { get; private set; }

    internal List<tileInfo> Path { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsFinished = false;
        count = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Path == null) { return; }

        if (count == Path.Count) { IsFinished = true;  return; }

        if (transform.position != Path[count].tilePos)
        {
            transform.position = Vector3.Lerp(transform.position, Path[count].tilePos, speed);
        }
        else if (transform.position == Path[count].tilePos)
        {
            count++;
        }
        
    }
}
