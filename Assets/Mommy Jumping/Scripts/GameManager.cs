using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public Player player;
    public PlayerInfo currentPlayerMode;
    public int startingPlatform;
    public float xSpawnOffset;
    public float minYspawnPos;
    public float maxYspawnPos;
    public Platform[] platformPrefabs;
    public CollecableItem[] collecableItems;
    private Platform m_lastPlatformSpawned;
    private List<int> m_platformLandedIds;
    public List<PlayerInfo> listGameMode;
    private float m_halfCamSizeX;
    private int m_score;
    private const int EASY_SCORE = 100;
    private const int NORMAL_SCORE = 200;
    private const int HARD_SCORE = 300;

    
    Dictionary<GameMode, PlayerInfo> dictGameMode;

    public Platform LastPlatformSpawned { get => m_lastPlatformSpawned; set => m_lastPlatformSpawned = value; }
    public List<int> PlatformLandedIds { get => m_platformLandedIds; set => m_platformLandedIds = value; }
    public int Score { get => m_score; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_platformLandedIds = new List<int>();
        m_halfCamSizeX = Helper.Get2DCamSize().x / 2;

        dictGameMode = new Dictionary<GameMode, PlayerInfo>();
        foreach (var item in listGameMode)
        {
            dictGameMode.Add(item.gameMode, item);
        }

        currentPlayerMode = dictGameMode[GameMode.None];
    }

    public override void Start()
    {
        base.Start();
        state = GameState.Starting;
        Invoke("PlatformInit", 0.5f);
        
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }

    public void PlayGame()
    {
        if (GUIManager.Ins)
        {
            GUIManager.Ins.ShowGamePlay(true);
        }
        Invoke("PlayGameIvk", 1f);
    }

    private void PlayGameIvk()
    {
        state = GameState.Playing;
        if (player)
        {
            player.Jump();
        }
    }
    private void PlatformInit()
    {
        m_lastPlatformSpawned = player.PlatformLanded;
        for (int i = 0; i < startingPlatform; i++)
        {
            SpawnPlatform();
        }
    }

    public bool IsPlatformLanded(int id)
    {
        if(m_platformLandedIds == null || m_platformLandedIds.Count <= 0) return false;

        return m_platformLandedIds.Contains(id);
    }
    public void SpawnPlatform()
    {
        if (!player || platformPrefabs == null || platformPrefabs.Length <= 0) return;
        float spawnPosX =
            Random.Range(-(m_halfCamSizeX - xSpawnOffset), (m_halfCamSizeX - xSpawnOffset));

        float distanceBetweenPlatform = Random.Range(minYspawnPos, maxYspawnPos);
        float spawnPosY = m_lastPlatformSpawned.transform.position.y + distanceBetweenPlatform;

        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);

        int randIdx = Random.Range(0, platformPrefabs.Length);
        var platformPrefab = platformPrefabs[randIdx];
        if (state == GameState.Starting)
        {
            platformPrefab = platformPrefabs[0];
        }
        
        if(!platformPrefab) return;

        var platformClone = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        platformClone.Id = m_lastPlatformSpawned.Id + 1;
        m_lastPlatformSpawned = platformClone;
    }

    public void SpawnCollectable(Transform spawnPoint)
    {
        if (collecableItems == null || collecableItems.Length <= 0 || state != GameState.Playing) return;
        int randIdx = Random.Range(0, collecableItems.Length);
        var collectItem = collecableItems[randIdx];

        if (collectItem == null) return;
        float randCheck = Random.Range(0f, 1f);
        if (randCheck <= collectItem.spawnRate && collectItem.collectablePrefab)
        {
            var cClone = Instantiate(collectItem.collectablePrefab, spawnPoint.position, Quaternion.identity);
            cClone.transform.SetParent(spawnPoint);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        if (state != GameState.Playing) return;

        m_score += scoreToAdd;
        if (m_score >= EASY_SCORE && m_score < NORMAL_SCORE)
        {
            currentPlayerMode = dictGameMode[GameMode.Easy];
        }
        else if (m_score >= NORMAL_SCORE && m_score < HARD_SCORE)
        {
            currentPlayerMode = dictGameMode[GameMode.Normal];
        } 
        else if(m_score >= HARD_SCORE) 
        {
            currentPlayerMode = dictGameMode[GameMode.Hard];
        }

        Pref.bestScore = m_score;
        if (GUIManager.Ins)
        {
            GUIManager.Ins.UpdateScore(m_score);
        }
    }
}
