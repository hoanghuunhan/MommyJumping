using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Playing,
    Gameover
}
public enum GameMode
{
    None,
    Easy,
    Hard,
    Normal
}

public enum GameTag
{
    Platform,
    Player,
    LeftCorner,
    RightCorner,
    Collectable,
    GroundCheck
}

public enum PrefKey
{
    BestScore
}


[System.Serializable]
public class CollecableItem
{
    public Collectable collectablePrefab;
    [Range(0f,1f)]
    public float spawnRate;
}