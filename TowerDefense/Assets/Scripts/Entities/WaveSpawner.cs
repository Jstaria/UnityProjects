using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private List<GameObject> enemies = new();
    [SerializeField] private Board board;
    [SerializeField] private Grid grid;
    [SerializeField] private GridVectorField vField;
    [SerializeField] private EnemyDatabase enemyDatabase;

    [SerializeField] private int outOfBoundsScale = 5;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            enemies.Add(Instantiate(enemyDatabase.enemyData[0].Prefab, board.StartPosition, Quaternion.identity));
        }

        foreach (GameObject enemy in enemies)
        {
            Vector3Int gridPosition = grid.WorldToCell(enemy.transform.position);

            Vector3 direction = vField.GetVector(gridPosition) * (vField.PathValues.ContainsKey(new Vector3Int(gridPosition.x, 0, gridPosition.y)) ? 1 : outOfBoundsScale);

            enemy.GetComponent<Enemy>().SetForce(direction);
        }
    }
}
