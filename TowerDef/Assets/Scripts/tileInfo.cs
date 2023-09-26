using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    Start,
    End,
    Path,
    Lawn
}

public class tileInfo : MonoBehaviour
{
    [SerializeField] SpriteRenderer tileObj;
    [SerializeField] TileState currentState;
    
    internal Vector3 tilePos { get; set; }

    internal TileState TileState 
    { 
        get 
        { 
            return currentState; 
        } 
        set 
        { 
            currentState = value;

            switch (currentState)
            {
                case TileState.Start:
                    tileObj.color = Color.green;
                    break;

                case TileState.End:
                    tileObj.color = Color.red;
                    break;

                case TileState.Path:
                    tileObj.color = Color.gray;
                    break;

                default:
                    tileObj.color = Color.yellow;
                    break;
            }
        } 
    }
    internal int x { get; set; }
    internal int y { get; set; }

    private void Start()
    {
        tilePos = transform.position;
    }
}
