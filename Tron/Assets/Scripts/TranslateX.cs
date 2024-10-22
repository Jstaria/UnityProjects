using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateX : MonoBehaviour
{
    [SerializeField] private Spring spring;

    // Start is called before the first frame update
    void Start()
    {
        spring.RestPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(spring.Position, transform.position.y, transform.position.z);

        if (Input.GetMouseButtonDown(0))
        {
            spring.Velocity += 100;
        }
        if (Input.GetMouseButtonDown(1))
        {
            spring.Velocity += -100;
        }
    }
}
