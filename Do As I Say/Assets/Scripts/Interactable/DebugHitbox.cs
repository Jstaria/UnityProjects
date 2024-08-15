using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHitbox : MonoBehaviour
{
    public GameObject square;

    private void Start()
    {
        GetComponent<HitBox>().OnHitBoxTrigger += Spawn;
    }

    public void Spawn()
    {
        square = Instantiate(square, transform);
    }
}
