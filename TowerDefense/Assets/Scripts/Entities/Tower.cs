using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Tower
{
    private int ID;
    private GameObject head;
    private AudioSource fire;
    private float activeRadius;

    private WaveSpawner waveSpawner;

    public Tower(int ID, GameObject head, WaveSpawner waveSpawner, float activeRadius)
    {
        this.ID = ID;
        this.head = head;
        this.waveSpawner = waveSpawner;
        this.activeRadius = activeRadius;
    }

    // Update is called once per frame
    public void Update()
    {
        GameObject currentEnemy = null;

        foreach (GameObject enemy in waveSpawner.Enemies)
        {
            if ((enemy.transform.position - head.transform.position).magnitude > activeRadius) continue;

            currentEnemy = enemy;
            break;
        }

        if (currentEnemy != null)
        {
            RotateTowardEnemy(head.transform, currentEnemy.transform.position);
        }
    }

    private void RotateTowardEnemy(Transform headTransform, Vector3 position)
    {
        headTransform.localRotation = Quaternion.Euler(0, (int)Time.deltaTime, 0);
    }
}
