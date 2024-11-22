using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Platformer/PlayerSO", order = 1)]

public class PlayerSO : ScriptableObject
{
    [Header("Player Movement")]
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float jumpForce = 11f;

    [Header("Player Stats")]
    [SerializeField] public float maxHealth = 100f;

    [Header("Player Collisions")]
    [SerializeField] public float groundCheckRadius = 0.2f;
    [SerializeField] public LayerMask enemyLayerMask;
    [SerializeField] public LayerMask bossLayerMask;
    [SerializeField] public LayerMask worldLayerMask;
    [SerializeField] public LayerMask moneyLayerMask;
}
