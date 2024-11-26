using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float progress;

    [SerializeField] private float speed = 40f;

    // Update is called once per frame
    void Update()
    {
        progress = Time.deltaTime * speed;
        transform.position = Vector3.Lerp(transform.position, endPos, progress);

        if (endPos.sqrMagnitude - transform.position.sqrMagnitude < .01)
        {
            GameObject.Destroy(this.gameObject);
        } 
    }

    public void SetEndPos(Vector3 target)
    {
        endPos = target;
    }
}
