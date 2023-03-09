using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecking : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var platformCol = collision.GetComponent<Platform>();
        if (!platformCol || !GameManager.Ins || !GameManager.Ins.LastPlatformSpawned) return;

        if (platformCol.Id == GameManager.Ins.LastPlatformSpawned.Id)
        {
            GameManager.Ins.SpawnPlatform();
        }
    }
}
