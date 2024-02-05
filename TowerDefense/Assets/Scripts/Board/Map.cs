using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = new Board();
        board.GenerateBoard(6, 5, 1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
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
