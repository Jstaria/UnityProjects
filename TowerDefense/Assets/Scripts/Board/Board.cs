using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private BoardPiece[,] boardPieces;
    private Vector2 position;
    private Vector2 offsetOffset;

    public BoardPiece[,] BoardPieces { get { return boardPieces; } }

    public void GenerateBoard(int width, int height, int scale, Vector3 position)
    {
        boardPieces = new BoardPiece[width, height];
        this.position = position;
        offsetOffset = Vector2.zero;

        if (width % 2 == 0) { offsetOffset.x = .5f; }
        if (height % 2 == 0) { offsetOffset.y = .5f; }

        Vector2 offset = new Vector2(-(width * scale) / 2, -(height * scale) / 2) + offsetOffset;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                boardPieces[i, j] = new BoardPiece();
                boardPieces[i, j].Position = new Vector2(i * scale, j * scale) + offset + (Vector2)position;
            }
        }
    }
}
