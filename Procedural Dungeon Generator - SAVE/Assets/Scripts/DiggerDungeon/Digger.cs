using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger
{ 
    Vector2Int position { get; set; }
    public Digger(Vector2Int startPos)
    {
        position = startPos;
    }

    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction toMove = (Direction)Random.Range(0, directionMovementMap.Count);
        position += directionMovementMap[toMove];
        return position;
    }
}
