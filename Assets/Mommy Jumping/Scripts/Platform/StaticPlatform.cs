using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlatform : Platform
{
    public int limitJump = 5;
    private int currentJump;

    public int CurrentJump { get => currentJump; set => currentJump = value; }

    private void Update()
    {
        if (currentJump < limitJump) return;
        Destroy(gameObject, 0.1f);
    }
}
