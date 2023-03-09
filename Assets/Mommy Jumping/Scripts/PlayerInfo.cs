
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public GameMode gameMode;
    public float jumpForce;
    public float moveSpeed;
    public float gravityScale;
}
