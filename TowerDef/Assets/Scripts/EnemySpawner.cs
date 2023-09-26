using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private List<GameObject> enemies = new List<GameObject>();

    internal Grid Grid { get; set; }
    internal List<GameObject> Enemies { get { return enemies; } }

    // Start is called before the first frame update
    void Start()
    {
        enemies.Add(Instantiate(enemy, Grid.Path[0].tilePos + new Vector3(0, 0, -3), Quaternion.identity));
        enemies[0].GetComponent<EnemyMovement>().Path = Grid.Path;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Count; i++ )
        {
            if (enemies[i].GetComponent<EnemyMovement>().IsFinished)
            {
                enemies[i].IsDestroyed();
                enemies.RemoveAt(i);
                i--;
            }
        }
    }
}
