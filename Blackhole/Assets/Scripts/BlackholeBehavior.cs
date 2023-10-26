using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackholeBehavior : MonoBehaviour
{
    [SerializeField] private float mass;
    [SerializeField] private float radius;

    public float Radius { get { return radius; } }

    public Vector2 GetAttractionForce(Vector2 position)
    {
        Vector2 force = Vector2.zero;

        force = ((Vector2)transform.position - position);
        force = force.normalized * Mathf.Pow(2, (-force.magnitude / 100) + 10) * mass;

        return force;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
