using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Map : MonoBehaviour
{
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        board.GenerateBoard(16, 12, 1, transform.position);
    }

    private void GetInput()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;

        foreach (BoardPiece piece in board.BoardPieces)
        {
            Gizmos.DrawWireCube(piece.Position, Vector3.one);
        }
    }
}
