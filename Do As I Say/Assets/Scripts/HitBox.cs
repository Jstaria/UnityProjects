using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public delegate void HitBoxTrigger();

    public event HitBoxTrigger OnHitBoxTrigger;

    [SerializeField] private float width = .5f;
    [SerializeField] private float height = .5f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Update()
    {
        minX = transform.position.x - width;
        maxX = transform.position.x + width;
        minY = transform.position.y - height;
        maxY = transform.position.y + height;
    }

    public void InvokeHitboxEvent()
    {
        if (OnHitBoxTrigger == null) return;

        OnHitBoxTrigger();
    }

    public bool Contains(Vector2 position)
    {
        return
            position.x >= minX && position.x <= maxX &&
            position.y >= minY && position.y <= maxY;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLineList(new Vector3[]
        {
            new Vector3(minX,minY),
            new Vector3(minX,maxY),
            new Vector3(minX,maxY),
            new Vector3(maxX,maxY),
            new Vector3(maxX,maxY),
            new Vector3(maxX,minY),
            new Vector3(maxX,minY),
            new Vector3(minX,minY),
        });
    }
}
