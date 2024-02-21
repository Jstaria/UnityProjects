using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private List<GameObject> enemies = new();
    [SerializeField] private Board board;
    [SerializeField] private Grid grid;
    [SerializeField] private GridVectorField vField;
    [SerializeField] private EnemyDatabase enemyDatabase;

    [SerializeField] private int outOfBoundsScale = 5;

    private List<TileNode> nodes = new();

    public List<GameObject > Enemies { get { return enemies; } }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 random = new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.2f, .2f));

            enemies.Add(Instantiate(enemyDatabase.enemyData[0].Prefab, board.StartPosition + random, Quaternion.identity));
            nodes.Add(board.Root);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            //Vector3Int gridPosition = grid.WorldToCell(enemy.transform.position);

            //Vector3 direction = vField.GetVector(gridPosition) * (vField.PathValues.ContainsKey(new Vector3Int(gridPosition.x, 0, gridPosition.y)) ? 1 : outOfBoundsScale);
            Vector3 direction = Vector3.zero;

            if (nodes[i].NextNode != null)
            {
                direction = ((nodes[i].NextNode.Position - nodes[i].Position)).normalized;

                if (nodes[i].NextNode.IsColliding(enemy.transform.position, .3f))
                {
                    if (nodes[i].NextNode != null)
                    {
                        nodes[i] = nodes[i].NextNode;
                    }
                }
            }

            enemy.GetComponent<Enemy>().SetForce(direction);
        }
    }
}
