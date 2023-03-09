using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public Rigidbody2D m_rb;
    private Platform m_platformLanded;
    private float m_movingLimitX;

    

    public float MovingLimitX { get => m_movingLimitX; set => m_movingLimitX = value; }
    public Platform PlatformLanded { get => m_platformLanded; set => m_platformLanded = value; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovingHandle();
    }

    public void UpdateGravityScale()
    {
        m_rb.gravityScale = playerInfo.gravityScale;
    }

    public void Jump()
    {
        if (!GameManager.Ins || GameManager.Ins.state != GameState.Playing) return;
        if(!m_rb || m_rb.velocity.y > 0 || !m_platformLanded) return;

        if (playerInfo.gameMode != GameManager.Ins.currentPlayerMode.gameMode)
        {
            playerInfo = GameManager.Ins.currentPlayerMode;
            m_rb.gravityScale = playerInfo.gravityScale;
        }

        if (m_platformLanded is BreakablePlatform)
        {
            m_platformLanded.PlatformAction();
        }
        m_rb.velocity = new Vector2(m_rb.velocity.x, playerInfo.jumpForce);

        if (AudioController.Ins)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.jump);
        }
    }

    private void MovingHandle()
    {
        if (!GamepadController.Ins || !m_rb || !GameManager.Ins || GameManager.Ins.state != GameState.Playing) return;

        if (GamepadController.Ins.CanMoveLeft)
        {
            m_rb.velocity = new Vector2(-playerInfo.moveSpeed, m_rb.velocity.y);
        } 
        else if (GamepadController.Ins.CanMoveRight)
        {
            m_rb.velocity = new Vector2(playerInfo.moveSpeed, m_rb.velocity.y);
        } 
        else
        {
            m_rb.velocity = new Vector2(0, m_rb.velocity.y);
        }

        m_movingLimitX = Helper.Get2DCamSize().x / 2;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -m_movingLimitX, m_movingLimitX),
            transform.position.y,
            transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Collectable.ToString()))
        {
            var collectable = collision.GetComponent<Collectable>();
            if (collectable)
            {
                collectable.Trigger();
            }
        }
    }
}
