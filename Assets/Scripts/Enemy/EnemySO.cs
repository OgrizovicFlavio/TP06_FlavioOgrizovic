using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Platformer/EnemySO", order = 3)]

public class EnemySO : ScriptableObject
{
    [Header("Enemy Movement")]
    [SerializeField] public float movementSpeed = 2.5f;

    [Header("Enemy Stats")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float baseDamage = 25f;

    [Header("Behaviour")]
    [SerializeField] public float waitTime = 0.5f;
}
